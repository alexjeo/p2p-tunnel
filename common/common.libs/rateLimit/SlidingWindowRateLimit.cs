﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace common.libs.rateLimit
{
    public class SlidingWindowRateLimit<TKey> : IRateLimit<TKey>
    {
        private readonly ConcurrentDictionary<TKey, SlidingRateInfo> limits = new ConcurrentDictionary<TKey, SlidingRateInfo>();
        private int rate = 0;
        private bool disponsed = true;
        private int windowLength = 0;
        private int mask = 0;
        private Func<long> timeFunc;

        public void Init(int rate, RateLimitTimeType type)
        {
            this.rate = rate;
            disponsed = false;

            switch (type)
            {
                case RateLimitTimeType.Second:
                    windowLength = 20;
                    mask = 1000 / 20;
                    timeFunc = DateTimeHelper.GetTimeStamp;
                    break;
                case RateLimitTimeType.Minute:
                    windowLength = 60;
                    mask = 60 / 60;
                    timeFunc = DateTimeHelper.GetTimeStampSec;
                    break;
                case RateLimitTimeType.Hour:
                    windowLength = 60;
                    mask = 60 / 60;
                    timeFunc = DateTimeHelper.GetTimeStampMinute;
                    break;
                case RateLimitTimeType.Day:
                    windowLength = 24;
                    mask = 24 / 24;
                    timeFunc = DateTimeHelper.GetTimeStampHour;
                    break;
                default:
                    break;
            }
        }

        public void SetRate(TKey key, int num)
        {
            if (disponsed == false)
            {
                if (limits.TryGetValue(key, out SlidingRateInfo info) == false)
                {
                    info = new SlidingRateInfo { Rate = num, Items = new SlidingRateItemInfo[windowLength], CurrentRate = 0 };
                    limits.TryAdd(key, info);
                }
                else
                {
                    info.Rate = num;
                }
            }
        }

        public bool Try(TKey key, int num)
        {
            if (disponsed) return false;

            try
            {
                Monitor.Enter(this);
                SlidingRateInfo info = Get(key);
                Move(info);
                bool res = info.CurrentRate < info.Rate;
                if (res)
                {
                    info.CurrentRate += num;
                    info.Items[0].Rate += num;
                }
                return res;
            }
            catch (Exception)
            {
            }
            finally
            {
                Monitor.Exit(this);
            }
            return false;
        }

        public async Task<bool> TryWait(TKey key, int num)
        {
            if (disponsed) return false;

            int last = num;
            do
            {
                try
                {
                    Monitor.Enter(this);
                    SlidingRateInfo info = Get(key);
                    Move(info);

                    //消耗掉能消耗的
                    int canEat = info.Rate - info.CurrentRate;
                    if (last < canEat)
                    {
                        canEat = last;
                    }
                    last -= canEat;

                    info.CurrentRate += canEat;
                    info.Items[0].Rate += canEat;

                    Monitor.Exit(this);
                    if (last > 0)
                    {
                        await Task.Delay(15);
                    }
                }
                catch (Exception)
                {
                    Monitor.Exit(this);
                }
            } while (last > 0);
            return true;
        }

        private void Move(SlidingRateInfo info)
        {
            long time = timeFunc();
            int index = (int)(time - info.Items[0].Time) / mask;
            if (index > 0)
            {
                info.CurrentRate = 0;
                if (index > windowLength)
                {
                    Array.Clear(info.Items, 0, info.Items.Length);
                }
                else
                {
                    info.Items.AsSpan(0, info.Items.Length - index).CopyTo(info.Items.AsSpan(index));
                    Array.Clear(info.Items, 0, index);
                }
                info.Items[0] = new SlidingRateItemInfo { Time = time };
                for (int i = 0; i < info.Items.Length; i++)
                {
                    if (info.Items[i] != null)
                    {
                        info.CurrentRate += info.Items[i].Rate;
                    }
                }
            }
        }
        private SlidingRateInfo Get(TKey key)
        {
            if (limits.TryGetValue(key, out SlidingRateInfo info) == false)
            {
                info = new SlidingRateInfo { Rate = rate, Items = new SlidingRateItemInfo[windowLength], CurrentRate = 0 };
                info.Items[0] = new SlidingRateItemInfo { Time = timeFunc() };
                limits.TryAdd(key, info);
            }
            return info;
        }

        public void Remove(TKey key)
        {
            limits.TryRemove(key, out _);
        }

        public void Disponse()
        {
            limits.Clear();
            disponsed = true;
        }

        class SlidingRateInfo
        {
            public int Rate { get; set; }
            public int CurrentRate { get; set; }
            public SlidingRateItemInfo[] Items { get; init; }
        }
        class SlidingRateItemInfo
        {
            public long Time { get; set; }
            public int Rate { get; set; } = 0;
        }
    }
}
