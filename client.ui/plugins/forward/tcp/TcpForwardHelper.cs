using client.ui.extends;
using client.ui.plugins.clients;
using client.ui.viewModel;
using common;
using common.extends;
using HandyControl.Controls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client.ui.plugins.forward.tcp
{
    public class TcpForwardHelper
    {
        private static readonly Lazy<TcpForwardHelper> lazy = new Lazy<TcpForwardHelper>(() => new TcpForwardHelper());
        public static TcpForwardHelper Instance => lazy.Value;

        public TcpForwardViewModel Viewmodel { get; private set; }
        public IPAddress IP { get; private set; }
        private readonly string configFileName = "config_tcp_forward.bin";
        public List<TcpForwardRecordBaseModel> Mappings { get; set; } = new List<TcpForwardRecordBaseModel>();
        private readonly ConcurrentDictionary<long, ClientModel> clients = new ConcurrentDictionary<long, ClientModel>();


        private TcpForwardHelper()
        {
            Viewmodel = new TcpForwardViewModel();
            ReadConfig();

            //A来了请求 ，转发到B，
            TcpForwardServer.Instance.OnRequest += OnRequest;
            //B那边发生了错误，无法完成请求
            TcpForwardEventHandles.Instance.OnTcpForwardMessageHandler += OnTcpForwardMessageHandler;

            TcpForwardServer.Instance.OnListeningChange += (sender, model) =>
            {
                TcpForwardRecordBaseModel mapping = Mappings.FirstOrDefault(c => c.SourcePort == model.SourcePort);
                if (mapping != null)
                {
                    mapping.Listening = model.Listening;
                    UpdateMappingList();
                }
                bool lastState = Viewmodel.Connected;
                Viewmodel.Connected = Mappings.Where(c => c.Listening).Count() > 0;

                if (lastState != Viewmodel.Connected)
                {
                    OnConnectdChange?.Invoke(this, Viewmodel.Connected);
                }
            };

        }

        private void OnRequest(object sender, TcpForwardRequestModel arg)
        {
            if (arg.Socket != null)
            {
                TcpForwardEventHandles.Instance.OnSendTcpForwardMessage(new OnSendTcpForwardMessageEventArg
                {
                    Data = arg.Msg,
                    Socket = arg.Socket
                });
            }
            else
            {
                TcpForwardServer.Instance.Fail(arg.Msg, "未选择转发对象，或者未与转发对象建立连接");
            }
        }
        private void OnTcpForwardMessageHandler(object sender, OnTcpForwardMessageEventArg arg)
        {
            OnTcpForwardMessageEventArg _arg = arg;
            switch (_arg.Data.Type)
            {
                //A收到了B的回复
                case TcpForwardType.RESPONSE:
                    TcpForwardServer.Instance.Response(_arg.Data);
                    break;
                //A收到B的错误回复
                case TcpForwardType.FAIL:
                    TcpForwardServer.Instance.Fail(_arg.Data);
                    break;
                //B收到请求
                case TcpForwardType.REQUEST:
                    {
                        try
                        {
                            _ = clients.TryGetValue(_arg.Data.RequestId, out ClientModel client);
                            if (client == null)
                            {
                                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                client = new ClientModel
                                {
                                    RequestId = _arg.Data.RequestId,
                                    TargetSocket = socket,
                                    TargetPort = _arg.Data.TargetPort,
                                    AliveType = _arg.Data.AliveType,
                                    TargetIp = _arg.Data.TargetIp,
                                    SourceSocket = _arg.Packet.TcpSocket
                                };

                                DnsEndPoint dnsEndPoint = new DnsEndPoint(_arg.Data.TargetIp, _arg.Data.TargetPort);
                                socket.Connect(dnsEndPoint);


                                if (_arg.Data.AliveType == TcpForwardAliveTypes.ALIVE)
                                {
                                    _ = clients.TryAdd(_arg.Data.RequestId, client);
                                    BindReceive(client);
                                }
                            }
                            if (client.TargetSocket.Connected)
                            {
                                _ = client.TargetSocket.Send(_arg.Data.Buffer);
                                if (_arg.Data.AliveType == TcpForwardAliveTypes.UNALIVE)
                                {
                                    Receive(client, client.TargetSocket.ReceiveAll());
                                    client.TargetSocket.SafeClose();
                                    _ = clients.TryRemove(_arg.Data.RequestId, out _);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _ = clients.TryRemove(_arg.Data.RequestId, out ClientModel client);
                            TcpForwardEventHandles.Instance.OnSendTcpForwardMessage(new OnSendTcpForwardMessageEventArg
                            {
                                Data = new TcpForwardModel
                                {
                                    RequestId = _arg.Data.RequestId,
                                    Type = TcpForwardType.FAIL,
                                    Buffer = Encoding.UTF8.GetBytes(ex + ""),
                                    AliveType = _arg.Data.AliveType
                                },
                                Socket = _arg.Packet.TcpSocket
                            });
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public event EventHandler<bool> OnConnectdChange;

        public void BindReceive(ClientModel client)
        {
            _ = Task.Factory.StartNew(() =>
              {
                  while (client.TargetSocket.Connected && clients.ContainsKey(client.RequestId))
                  {
                      try
                      {
                          Receive(client, client.TargetSocket.ReceiveAll());
                      }
                      catch (Exception)
                      {
                          _ = clients.TryRemove(client.RequestId, out _);
                          break;
                      }
                  }
              }, TaskCreationOptions.LongRunning);
        }

        private void Receive(ClientModel client, byte[] data)
        {
            TcpForwardEventHandles.Instance.OnSendTcpForwardMessage(new OnSendTcpForwardMessageEventArg
            {
                Data = new TcpForwardModel
                {
                    RequestId = client.RequestId,
                    Buffer = data,
                    Type = TcpForwardType.RESPONSE,
                    TargetPort = client.TargetPort,
                    AliveType = client.AliveType,
                    TargetIp = client.TargetIp
                },
                Socket = client.SourceSocket
            });
        }

        public void Del(TcpForwardRecordBaseModel model)
        {
            Stop(model);
            TcpForwardRecordBaseModel map = Mappings.FirstOrDefault(c => c.SourcePort == model.SourcePort);
            if (map != null)
            {
                _ = Mappings.Remove(map);
            }
            UpdateMappingList();
            SaveConfig();
        }
        public void Add(TcpForwardRecordBaseModel model, int index = -1)
        {
            if (Helper.GetUsedPort().Contains(model.SourcePort))
            {
                _ = MessageBox.Error("源端口已被其它程序占用");
                return;
            }

            TcpForwardRecordBaseModel map = Mappings.FirstOrDefault(c => c.SourcePort == model.SourcePort);
            if (index == -1)
            {
                if (map == null)
                {
                    Mappings.Add(model);
                    UpdateMappingList();
                    SaveConfig();
                }
                else
                {
                    _ = MessageBox.Error("源端口已存在");
                }
            }
            else
            {
                //找到了相同端口，但是下标不一样
                if (map != null && Mappings.IndexOf(map) != index)
                {
                    _ = MessageBox.Error("源端口已存在");
                }
                else
                {
                    TcpForwardRecordBaseModel target = Mappings[index];
                    target.SourceIp = model.SourceIp;
                    target.SourcePort = model.SourcePort;
                    target.TargetName = model.TargetName;
                    target.TargetPort = model.TargetPort;
                    target.AliveType = model.AliveType;
                    target.TargetIp = model.TargetIp;
                    UpdateMappingList();
                    SaveConfig();
                }
            }
        }

        public void Start(TcpForwardRecordBaseModel model)
        {
            try
            {
                TcpForwardRecordModel model2 = new TcpForwardRecordModel
                {
                    AliveType = model.AliveType,
                    SourceIp = model.SourceIp,
                    SourcePort = model.SourcePort,
                    TargetIp = model.TargetIp,
                    TargetName = model.TargetName,
                    TargetPort = model.TargetPort
                };

                model2.Client = AppShareData.Instance.Clients.FirstOrDefault(c => c.Name == model.TargetName);
                if (model2.Client == null)
                {
                    model2.Client = new ClientInfo { Name = model.TargetName };
                }
                TcpForwardServer.Instance.Start(model2);
                SaveConfig();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Error(ex.Message);
            }
        }
        public void StartAll()
        {
            StopAll();
            Mappings.ForEach(c =>
            {
                Start(c);
            });
        }

        public void Stop(TcpForwardRecordBaseModel model)
        {
            TcpForwardServer.Instance.Stop(model.SourcePort);
            SaveConfig();
        }
        public void StopAll()
        {
            Mappings.ForEach(c =>
            {
                TcpForwardServer.Instance.Stop(c.SourcePort);
            });
            SaveConfig();
        }

        private void ReadConfig()
        {
            ConfigFileModel config = configFileName.ProtobufDeserializeFileRead<ConfigFileModel>();
            if (config != null)
            {
                Mappings = config.Mappings;
                Mappings.ForEach(c =>
                {
                    c.Listening = false;
                });
                UpdateMappingList();
            }
        }
        private void SaveConfig()
        {
            try
            {
                _ = new ConfigFileModel
                {
                    Mappings = Mappings
                }.ProtobufSerializeFileSave(configFileName);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Error(ex.Message);
            }
        }

        private void UpdateMappingList()
        {
            Viewmodel.Mappings = new ObservableCollection<TcpForwardRecordBaseModel>(Mappings);
        }
    }

    public class ConfigFileModel : IFileConfig
    {
        public ConfigFileModel() { }

        public List<TcpForwardRecordBaseModel> Mappings { get; set; } = new List<TcpForwardRecordBaseModel>();
    }

    public class ClientModel
    {
        public long RequestId { get; set; }
        public int SourcePort { get; set; } = 0;
        public Socket SourceSocket { get; set; }
        public Socket TargetSocket { get; set; }
        public byte[] BufferSize { get; set; }
        public string TargetIp { get; set; } = string.Empty;
        public int TargetPort { get; set; } = 0;
        public TcpForwardAliveTypes AliveType { get; set; } = TcpForwardAliveTypes.UNALIVE;
    }
}
