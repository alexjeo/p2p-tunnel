using client.ui.events;
using client.ui.plugins.p2pMessage;
using common;
using common.extends;
using server.model;
using server.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace client.ui.plugins.request
{
    public class RequestEventHandlers
    {
        private static readonly Lazy<RequestEventHandlers> lazy = new Lazy<RequestEventHandlers>(() => new RequestEventHandlers());
        public static RequestEventHandlers Instance => lazy.Value;

        private readonly Dictionary<RequestExcuteTypes, IRequestExcutePlugin[]> plugins = null;

        private RequestEventHandlers()
        {
            plugins = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(c => c.GetTypes())
                .Where(c => c.GetInterfaces().Contains(typeof(IRequestExcutePlugin)))
                .Select(c => (IRequestExcutePlugin)Activator.CreateInstance(c)).GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.ToArray());

            _ = Task.Factory.StartNew(() =>
              {
                  while (true)
                  {
                      if (!requestCache.IsEmpty)
                      {
                          long time = Helper.GetTimeStamp();
                          foreach (TcpRequestMessageCache item in requestCache.Values)
                          {
                              if (time - item.Time > item.Timeout)
                              {
                                  _ = requestCache.TryRemove(item.Id, out TcpRequestMessageCache cache);
                                  if (cache != null)
                                  {
                                      cache.FailCallback(new TcpRequestMessageFailModel
                                      {
                                          Type = TcpRequestMessageFailType.TIMEOUT,
                                          Msg = "请求超时"
                                      });
                                  }
                              }
                          }
                      }
                      Thread.Sleep(1);
                  }

              }, TaskCreationOptions.LongRunning);
        }

        #region 请求
        private readonly ConcurrentDictionary<long, TcpRequestMessageCache> requestCache = new();
        private long requestId = 0;

        //收到请求
        public void OnTcpRequestMsessage(OnTcpRequestMsessageEventArg arg)
        {
            RequestExcuteTypes type = arg.Data.RequestType;

            if (type == RequestExcuteTypes.RESULT)
            {
                OnTcpRequestMsessageResult(arg);
            }
            else if (plugins.ContainsKey(type))
            {
                IRequestExcutePlugin plugin = plugins[type].Last();
                byte[] result = plugin.Excute(arg);

                if (result.Length > 0)
                {
                    P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
                    {
                        Socket = arg.Packet.TcpSocket,
                        Data = new MessageRequestModel
                        {
                            Data = result,
                            RequestId = arg.Data.RequestId,
                            RequestType = RequestExcuteTypes.RESULT,
                            Code = MessageRequestResultCodes.OK
                        }
                    });
                }
            }
            else
            {
                P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
                {
                    Socket = arg.Packet.TcpSocket,
                    Data = new MessageRequestModel
                    {
                        Data = "没有相应的插件执行你的请求".ProtobufSerialize(),
                        RequestId = arg.Data.RequestId,
                        RequestType = RequestExcuteTypes.FAIL,
                        Code = MessageRequestResultCodes.NOTFOUND
                    }
                });
            }
        }
        //收到请求回调
        public void OnTcpRequestMsessageResult(OnTcpRequestMsessageEventArg arg)
        {
            _ = requestCache.TryRemove(arg.Data.RequestId, out TcpRequestMessageCache cache);
            if (cache != null)
            {
                if (arg.Data.Code == MessageRequestResultCodes.OK)
                {
                    cache.Callback(arg.Data);
                }
                else
                {
                    cache.FailCallback(arg.Data.Data.ProtobufDeserialize<TcpRequestMessageFailModel>());
                }
            }
        }
        //发起请求
        public void SendTcpRequestMsessage(Socket socket, IRequestExcuteMessage data, Action<MessageRequestModel> callback, Action<TcpRequestMessageFailModel> failCallback)
        {
            _ = Interlocked.Increment(ref requestId);
            _ = requestCache.TryAdd(requestId, new TcpRequestMessageCache
            {
                Callback = callback,
                FailCallback = failCallback,
                Id = requestId,
                Time = Helper.GetTimeStamp()
            });

            P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
            {
                Socket = socket,
                Data = new MessageRequestModel
                {
                    Data = data.ProtobufSerialize(),
                    RequestId = requestId,
                    RequestType = data.Type,
                    Code = MessageRequestResultCodes.OK,

                }
            });
        }

        public void SendTcpRequestMsessage(Socket socket, IRequestExcuteMessage data)
        {
            _ = Interlocked.Increment(ref requestId);

            P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
            {
                Socket = socket,
                Data = new MessageRequestModel
                {
                    Data = data.ProtobufSerialize(),
                    RequestId = requestId,
                    RequestType = data.Type
                }
            });
        }
        #endregion
    }




    #region 请求
    public class TcpRequestMessageCache
    {
        public long Id { get; set; } = 0;
        public long Time { get; set; } = 0;
        public int Timeout { get; set; } = 15 * 60 * 1000;
        public Action<MessageRequestModel> Callback { get; set; } = null;
        public Action<TcpRequestMessageFailModel> FailCallback { get; set; } = null;
    }

    public class TcpRequestMessageFailModel
    {
        public TcpRequestMessageFailType Type { get; set; } = TcpRequestMessageFailType.ERROR;
        public string Msg { get; set; } = string.Empty;
    }

    public enum TcpRequestMessageFailType
    {
        ERROR, TIMEOUT
    }

    public class OnTcpRequestMsessageEventArg : EventArgs
    {
        public PluginExcuteModel Packet { get; set; }
        public MessageRequestModel Data { get; set; }
    }
    #endregion
}
