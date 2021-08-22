using client.service.serverPlugins.clients;
using client.ui.extends;
using common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client.service.serverPlugins.forward.tcp
{
    internal class TcpForwardServer
    {
        private static readonly Lazy<TcpForwardServer> lazy = new(() => new TcpForwardServer());
        public static TcpForwardServer Instance => lazy.Value;

        private TcpForwardServer()
        {
        }

        private bool IsStart { get; set; } = false;
        private long requestId = 0;
        public event EventHandler<TcpForwardRequestModel> OnRequest;
        public ConcurrentDictionary<long, Socket> clients = new();
        public ConcurrentDictionary<int, Socket> services = new();

        public event EventHandler<ListeningChangeModel> OnListeningChange;

        public void Start(TcpForwardRecordModel mapping)
        {
            if (services.ContainsKey(mapping.SourcePort))
            {
                return;
            }

            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Any, mapping.SourcePort));
            socket.Listen(int.MaxValue);

            IsStart = true;

            _ = services.TryAdd(mapping.SourcePort, socket);
            OnListeningChange?.Invoke(this, new ListeningChangeModel
            {
                SourcePort = mapping.SourcePort,
                Listening = true
            });

            int sourcePort = mapping.SourcePort;
            int targetPort = mapping.TargetPort;
            string targetIp = mapping.TargetIp;
            ClientInfo targetClient = mapping.Client;
            TcpForwardAliveTypes aliveType = mapping.AliveType;

            ManualResetEvent acceptDone = new(false);
            _ = Task.Factory.StartNew(() =>
            {
                while (IsStart)
                {
                    _ = acceptDone.Reset();
                    try
                    {
                        _ = socket.BeginAccept(new AsyncCallback(Accept), new ClientModel2
                        {
                            TargetClient = targetClient,
                            TargetPort = targetPort,
                            AliveType = aliveType,
                            TargetIp = targetIp,
                            SourceSocket = socket,
                            SourcePort = sourcePort,
                            AcceptDone = acceptDone
                        });
                    }
                    catch (Exception)
                    {
                        services.TryRemove(sourcePort, out _);
                        acceptDone.Dispose();
                        break;
                    }
                    _ = acceptDone.WaitOne();
                }

            }, TaskCreationOptions.LongRunning);

        }


        private void Accept(IAsyncResult result)
        {
            ClientModel2 server = (ClientModel2)result.AsyncState;
            server.AcceptDone.Set();
            try
            {
                Socket socket = server.SourceSocket.EndAccept(result);
                _ = Interlocked.Increment(ref requestId);
                long _requestId = requestId;
                _ = clients.TryAdd(_requestId, socket);

                ClientModel2 client = new()
                {
                    RequestId = _requestId,
                    TargetPort = server.TargetPort,
                    AliveType = server.AliveType,
                    TargetIp = server.TargetIp,
                    SourceSocket = socket,
                    TargetClient = server.TargetClient
                };
                if (server.AliveType == TcpForwardAliveTypes.UNALIVE)
                {
                    Receive(client, client.SourceSocket.ReceiveAll());
                }
                else
                {
                    BindReceive(client);
                }
            }
            catch (Exception)
            {
                Stop(server.SourcePort);
                server.AcceptDone.Dispose();
            }
        }

        public void BindReceive(ClientModel2 client)
        {
            Task.Factory.StartNew(() =>
            {
                while (client.SourceSocket.Connected && clients.ContainsKey(client.RequestId))
                {
                    try
                    {
                        Receive(client, client.SourceSocket.ReceiveAll());
                    }
                    catch (Exception)
                    {
                        _ = clients.TryRemove(client.RequestId, out _);
                        break;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Receive(ClientModel2 client, byte[] data)
        {
            Socket socket = null;
            if (client.TargetClient != null)
            {
                socket = client.TargetClient.Socket;
                if (socket == null || !socket.Connected)
                {
                    client.TargetClient = AppShareData.Instance.Clients.Values.FirstOrDefault(c => c.Name == client.TargetClient.Name);
                    if (client.TargetClient != null)
                    {
                        socket = client.TargetClient.Socket;
                    }
                }
            }
            OnRequest?.Invoke(this, new TcpForwardRequestModel
            {
                Msg = new TcpForwardModel
                {
                    RequestId = client.RequestId,
                    Buffer = data,
                    Type = TcpForwardType.REQUEST,
                    TargetPort = client.TargetPort,
                    AliveType = client.AliveType,
                    TargetIp = client.TargetIp
                },
                Socket = socket
            });
        }

        public void StartAll(List<TcpForwardRecordBaseModel> mappings)
        {
            foreach (TcpForwardRecordBaseModel item in mappings)
            {
                Start(new TcpForwardRecordModel
                {
                    AliveType = item.AliveType,
                    SourceIp = item.SourceIp,
                    SourcePort = item.SourcePort,
                    TargetIp = item.TargetIp,
                    TargetName = item.TargetName,
                    TargetPort = item.TargetPort,
                    Listening = false,
                });
            }
        }

        public void Response(TcpForwardModel model)
        {
            if (clients.TryGetValue(model.RequestId, out Socket socket) && socket != null)
            {
                try
                {
                    if (socket.Connected)
                    {
                        int length = socket.Send(model.Buffer, SocketFlags.None);
                    }
                }
                catch (Exception)
                {
                }
                if (model.AliveType == TcpForwardAliveTypes.UNALIVE)
                {
                    socket.SafeClose();
                    _ = clients.TryRemove(model.RequestId, out _);
                }
            }
        }

        public void Fail(TcpForwardModel failModel, string body = "snltty")
        {
            if (clients.TryGetValue(failModel.RequestId, out Socket socket) && socket != null)
            {
                if (failModel.AliveType == TcpForwardAliveTypes.UNALIVE)
                {
                    try
                    {
                        var bodyBytes = Encoding.UTF8.GetBytes(body);
                        _ = socket.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n"));
                        _ = socket.Send(Encoding.UTF8.GetBytes("Content-Type: text/html;charset=utf-8\r\n"));
                        _ = socket.Send(Encoding.UTF8.GetBytes($"Content-Length:{bodyBytes.Length}\r\n"));
                        _ = socket.Send(Encoding.UTF8.GetBytes("\r\n"));
                        _ = socket.Send(bodyBytes);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (failModel.Buffer != null)
                {
                    Logger.Instance.Info(failModel.Buffer.Length.ToString());
                }
                socket.SafeClose();
                _ = clients.TryRemove(failModel.RequestId, out _);
            }
        }

        public void Stop(int sourcePort)
        {
            _ = services.TryRemove(sourcePort, out Socket socket);
            socket.SafeClose();
            OnListeningChange?.Invoke(this, new ListeningChangeModel
            {
                SourcePort = sourcePort,
                Listening = false
            });

            IsStart = services.Count > 0;
        }
        public void StopAll()
        {
            foreach (KeyValuePair<int, Socket> item in services)
            {
                Stop(item.Key);
            }
        }
    }

    public class ListeningChangeModel
    {
        public int SourcePort { get; set; } = 0;
        public bool Listening { get; set; } = false;
    }

    public class TcpForwardRequestModel
    {
        public Socket Socket { get; set; }
        public TcpForwardModel Msg { get; set; }
    }

    public class ClientModel2 : ClientModel
    {
        public ClientInfo TargetClient { get; set; }
        public ManualResetEvent AcceptDone { get; set; }
    }
}
