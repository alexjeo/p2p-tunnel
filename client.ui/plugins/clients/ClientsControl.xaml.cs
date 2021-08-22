using client.ui.extends;
using client.ui.plugins.chat;
using client.ui.plugins.connectClient;
using client.ui.plugins.heart;
using client.ui.plugins.rdpDesktop;
using client.ui.plugins.register;
using client.ui.viewModel;
using common;
using server.model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace client.ui.plugins.clients
{
    /// <summary>
    /// ClientsControl.xaml 的交互逻辑
    /// </summary>
    public partial class ClientsControl : UserControl
    {
        private readonly ClientsViewModel viewModel;
        private readonly ConcurrentDictionary<long, ClientInfo> clients = new();
        private readonly ConcurrentDictionary<long, RdpShareWindow> shareDekstops = new();

        //接收到服务器的客户端列表的次数  第一次的时候 每个客户端链接一下
        private short readClientsTimes = 0;

        public ClientsControl()
        {
            InitializeComponent();

            viewModel = new ClientsViewModel();
            DataContext = viewModel;

            //文本聊天
            P2PChatEventHandles.Instance.OnTcpChatTextMessageHandler += (sender, arg) =>
            {
                if (clients.TryGetValue(arg.Id, out ClientInfo client))
                {
                    if (client.Id != AppShareData.Instance.CurrentClientInfo.Id)
                    {
                        client.MsgCount += 1;
                    }
                    else
                    {
                        client.MsgCount = 0;
                    }
                }
            };
            ClientsEventHandles.Instance.OnCurrentClientChangeHandler += (sender, client) =>
            {
                client.MsgCount = 0;
            };

            //本客户端注册状态
            RegisterEventHandles.Instance.OnSendRegisterTcpStateChangeHandler += OnRegisterTcpStateChangeHandler;
            //收到来自服务器的 在线客户端 数据
            ClientsEventHandles.Instance.OnServerSendClientsHandler += OnServerSendClientsHandler;

            //UDP
            //有人想连接我
            ConnectClientEventHandles.Instance.OnConnectClientStep1Handler += OnConnectClientStep1Handler;
            //有人连接我
            ConnectClientEventHandles.Instance.OnConnectClientStep3Handler += OnConnectClientStep3Handler;

            //TCP
            //有人回应
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep4Handler += OnTcpConnectClientStep4Handler;
            //连接别人失败
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientStep2FailHandler += OnSendTcpConnectClientStep2FailHandler;

            //退出消息
            RegisterEventHandles.Instance.OnSendExitMessageHandler += (sender, e) =>
            {
                foreach (ClientInfo client in clients.Values)
                {
                    _ = RemoveClient(client.Id);
                    if (client.Connecting)
                    {
                        ConnectClientEventHandles.Instance.SendTcpConnectClientStep2StopMessage(client.Id);
                    }
                }
                clients.Clear();
                Dispatcher.Invoke(() =>
                {
                    viewModel.Clients.Clear();
                });
                readClientsTimes = 0;
            };

            //给各个客户端发送心跳包
            _ = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    long time = Helper.GetTimeStamp();

                    foreach (ClientInfo client in clients.Values)
                    {
                        if (client.IsTimeout())
                        {
                            if (client.Connected && !client.Connecting)
                            {
                                SetClientOffline(client.Id);
                            }
                        }
                        else
                        {
                            if (client.Connected)
                            {
                                HeartEventHandles.Instance.SendHeartMessage(client.Address);
                            }
                            if (client.TcpConnected)
                            {
                                HeartEventHandles.Instance.SendTcpHeartMessage(client.Socket);
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(5000);
                }

            }, TaskCreationOptions.LongRunning);

            //收客户端的心跳包
            HeartEventHandles.Instance.OnHeartEventHandler += (sender, e) =>
            {
                if (e.Data.SourceId > 0)
                {
                    _ = clients.TryGetValue(e.Data.SourceId, out ClientInfo cacheClient);
                    if (cacheClient != null)
                    {
                        if (e.Packet.ServerType == ServerType.UDP)
                        {
                            cacheClient.LastTime = Helper.GetTimeStamp();
                        }
                        else if (e.Packet.ServerType == ServerType.TCP)
                        {
                            cacheClient.TcpLastTime = Helper.GetTimeStamp();
                        }
                    }
                }
            };

            _ = Task.Run(() =>
            {
                viewModel.RouteLevel = Helper.GetRouteLevel();
            });

            //new RdpShareWindow(new ClientInfo { Name = "11" }).Show();
        }

        private void OnConnectClientStep1Handler(object sender, OnConnectClientStep1EventArg e)
        {
            if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null)
            {
                cacheClient.Connecting = true;
            }
            _ = Helper.SetTimeout(() =>
              {
                  if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null && !cacheClient.Connected)
                  {
                      cacheClient.Connecting = false;
                  }
              }, 3000);
        }
        private void OnConnectClientStep3Handler(object sender, OnConnectClientStep3EventArg e)
        {
            if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null)
            {
                cacheClient.Connected = true;
                cacheClient.LastTime = Helper.GetTimeStamp();
                cacheClient.Address = e.Packet.SourcePoint;
            }
        }


        private void OnSendTcpConnectClientStep2FailHandler(object sender, OnSendTcpConnectClientStep2FailEventArg e)
        {
            SetClientOffline(e.ToId);
        }

        private void OnTcpConnectClientStep4Handler(object sender, OnConnectClientStep4EventArg e)
        {
            if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null)
            {
                cacheClient.TcpConnected = true;
                cacheClient.Connecting = false;
                cacheClient.TcpLastTime = Helper.GetTimeStamp();
                cacheClient.Socket = e.Packet.TcpSocket;
            }
        }

        private void OnRegisterTcpStateChangeHandler(object sender, RegisterTcpEventArg e)
        {
            if (e.State)
            {
                viewModel.Ip = e.Ip;
            }
        }

        private void OnServerSendClientsHandler(object sender, OnServerSendClientsEventArg e)
        {
            _ = Task.Run(() =>
              {
                  try
                  {
                      ++readClientsTimes;
                      if (e.Data.Clients == null)
                      {
                          return;
                      }

                      //下线了的
                      IEnumerable<long> offlines = clients.Values.Select(c => c.Id).Except(e.Data.Clients.Select(c => c.Id));
                      foreach (long offid in offlines)
                      {
                          if (offid == AppShareData.Instance.ConnectId)
                          {
                              continue;
                          }
                          SetClientOffline(offid);
                          _ = RemoveClient(offid);
                      }
                      //新上线的
                      IEnumerable<long> upLines = e.Data.Clients.Select(c => c.Id).Except(clients.Values.Select(c => c.Id));
                      IEnumerable<MessageClientsClientModel> upLineClients = e.Data.Clients.Where(c => upLines.Contains(c.Id));
                      foreach (MessageClientsClientModel item in upLineClients)
                      {
                          if (item.Id == AppShareData.Instance.ConnectId)
                          {
                              continue;
                          }
                          ClientInfo client = new ClientInfo
                          {
                              Ping = 0,
                              LastTime = 0,
                              TcpLastTime = 0,
                              Connected = false,
                              TcpConnected = false,
                              Connecting = false,
                              Socket = null,
                              Address = IPEndPoint.Parse(item.Address),
                              Id = item.Id,
                              Name = item.Name,
                              Port = item.Port,
                              TcpPort = item.TcpPort
                          };
                          _ = clients.TryAdd(item.Id, client);
                          if (readClientsTimes == 1)
                          {
                              ConnectClient(client);
                          }
                      }
                      Dispatcher.Invoke(() =>
                      {
                          viewModel.Clients = new ObservableCollection<ClientInfo>(clients.Values);
                      });
                  }
                  catch (Exception ex)
                  {
                      Logger.Instance.Error("" + ex);
                  }
              });
        }

        private void ClientsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = sender as ListBox;

            if (box.SelectedItem is ClientInfo client)
            {
                SetCurrentClient(client.Id);
            }
        }

        private void ConnectClient(ClientInfo info)
        {
            if (info.Id == AppShareData.Instance.ConnectId)
            {
                return;
            }

            if (info.Connecting == false && info.Connected == false)
            {
                info.Connecting = true;
                ConnectClientEventHandles.Instance.SendConnectClientMessage(new ConnectParams
                {
                    Id = info.Id,
                    Name = info.Name,
                    Callback = (e) =>
                    {
                        if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null)
                        {
                            cacheClient.Connected = true;
                            cacheClient.LastTime = Helper.GetTimeStamp();
                            cacheClient.Address = e.Packet.SourcePoint;

                            cacheClient.TcpConnecting = true;
                            ConnectClientEventHandles.Instance.SendTcpConnectClientMessage(new ConnectTcpParams
                            {
                                Callback = (e) =>
                                {
                                    if (clients.TryGetValue(e.Data.Id, out ClientInfo cacheClient) && cacheClient != null)
                                    {
                                        cacheClient.TcpConnected = true;
                                        cacheClient.Connecting = false;
                                        cacheClient.TcpConnecting = false;
                                        cacheClient.TcpLastTime = Helper.GetTimeStamp();
                                        cacheClient.Socket = e.Packet.TcpSocket;
                                    }
                                },
                                FailCallback = (e) =>
                                {
                                    SetClientOffline(info.Id);
                                },
                                Id = info.Id,
                                Name = info.Name,
                                Timeout = 300 * 1000
                            });
                        }
                    },
                    FailCallback = (e) =>
                    {
                        SetClientOffline(info.Id);
                    },
                    Timeout = 2000,
                    TryTimes = 5
                });
            }
        }
        private void ConnectClick(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ClientInfo info)
            {
                ConnectClient(info);
            }
        }

        private void DesktopClick(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ClientInfo info)
            {
                _ = shareDekstops.TryGetValue(info.Id, out RdpShareWindow window);
                if (window == null)
                {
                    window = new RdpShareWindow(info);
                    window.Show();
                    window.Closed += (sender, e) =>
                    {
                        window = null;
                        _ = shareDekstops.TryRemove(info.Id, out _);
                        Helper.FlushMemory();
                    };
                    _ = shareDekstops.TryAdd(info.Id, window);
                }
                else
                {
                    _ = window.Focus();
                    _ = window.Activate();
                }

            }
        }

        private void SetClientOffline(long id)
        {
            _ = clients.TryGetValue(id, out ClientInfo client);
            if (client != null)
            {
                client.Connecting = false;
                client.TcpConnecting = false;
                client.Connected = false;
                client.TcpConnected = false;
                client.LastTime = 0;
                client.TcpLastTime = 0;
                if (client.Socket != null)
                {
                    client.Socket.SafeClose();
                }
            }
        }
        private ClientInfo RemoveClient(long id)
        {
            _ = clients.TryRemove(id, out ClientInfo client);
            ClientsEventHandles.Instance.OnClientRemove(id);
            return client;
        }

        private void SetCurrentClient(long id)
        {
            if (clients.TryGetValue(id, out ClientInfo client))
            {
                SetCurrentClient(client);
            }
        }
        private void SetCurrentClient(ClientInfo client)
        {
            if (client != null)
            {
                AppShareData.Instance.CurrentClientInfo = client;
                ClientsEventHandles.Instance.OnCurrentClientChange(client);
            }
        }
    }
}
