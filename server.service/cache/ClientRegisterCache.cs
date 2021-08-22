using server.service.model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace common.cache
{
    public sealed class ClientRegisterCache
    {
        private static readonly Lazy<ClientRegisterCache> lazy = new(() => new ClientRegisterCache());
        public static ClientRegisterCache Instance => lazy.Value;

        private readonly ConcurrentDictionary<long, RegisterCacheModel> cache = new();

        private static long Id = 0;

        private ClientRegisterCache()
        {
            Helper.SetInterval(() =>
            {
                long time = Helper.GetTimeStamp();

                foreach (RegisterCacheModel item in cache.Values)
                {
                    if (time - item.LastTime > 60 * 1000)
                    {
                        _ = cache.TryRemove(item.Id, out _);
                    }
                }
            }, 1000);
        }

        public RegisterCacheModel Get(long id)
        {
            _ = cache.TryGetValue(id, out RegisterCacheModel toReg);
            return toReg;
        }

        public RegisterCacheModel GetBySameGroup(string groupid, string name)
        {
            return cache.Values.FirstOrDefault(c => c.GroupId == groupid && c.Name == name);
        }

        public List<RegisterCacheModel> GetAll()
        {
            return cache.Values.ToList();
        }

        public long Add(RegisterCacheModel model, long id = 0)
        {
            if (id == 0)
            {
                Interlocked.Increment(ref Id);
                id = Id;
            }
            model.Id = id;
            model.GroupId = string.IsNullOrWhiteSpace(model.GroupId) ? Helper.GetMd5Hash(Guid.NewGuid().ToString()) : model.GroupId;
            _ = cache.AddOrUpdate(id, model, (a, b) => model);
            return id;
        }

        public long UpdateTcpInfo(long id, Socket socket, int port)
        {
            RegisterCacheModel data = Get(id);
            if (data != null)
            {
                data.LastTime = Helper.GetTimeStamp();
                data.TcpSocket = socket;
                data.TcpPort = port;
            }
            return id;
        }

        public void Remove(long id)
        {
            _ = cache.TryRemove(id, out _);
        }

        public void UpdateTime(long id)
        {
            RegisterCacheModel model = Get(id);
            if (model != null)
            {
                model.LastTime = Helper.GetTimeStamp();
            }
        }

    }
}
