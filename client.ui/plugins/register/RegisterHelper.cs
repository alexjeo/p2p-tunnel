using client.ui.events;
using client.ui.plugins.heart;
using common;
using common.extends;
using ProtoBuf;
using server;
using server.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MessageBox = HandyControl.Controls.MessageBox;

namespace client.ui.plugins.register
{
    public class RegisterHelper
    {
        private static readonly Lazy<RegisterHelper> lazy = new(() => new RegisterHelper());
        public static RegisterHelper Instance => lazy.Value;

        public readonly ConnectViewModel viewModel = null;
        private long lastTime = 0;
        private long lastTcpTime = 0;
        private readonly int heartInterval = 5000;
        private readonly string configFileName = "config_connect.bin";
        public event EventHandler<bool> OnRegisterChange;

        private RegisterHelper()
        {
            viewModel = new ConnectViewModel();
            ReadConfig();

            //退出消息
            RegisterEventHandles.Instance.OnSendExitMessageHandler += (sender, e) =>
            {
                viewModel.IsConnecting = false;
                viewModel.Connected = false;
                viewModel.TcpConnected = false;
                //viewModel.ConnectId = 0;
                ResetLastTime();
                OnRegisterChange?.Invoke(this, false);
            };

            //给服务器发送心跳包
            _ = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (IsTimeout())
                    {
                        RegisterEventHandles.Instance.SendExitMessage();
                    }
                    if (viewModel.Connected && viewModel.TcpConnected)
                    {
                        HeartEventHandles.Instance.SendHeartMessage();
                        HeartEventHandles.Instance.SendTcpHeartMessage();
                    }
                    Thread.Sleep(heartInterval);
                }
            }, TaskCreationOptions.LongRunning);

            //收服务器的心跳包
            HeartEventHandles.Instance.OnHeartEventHandler += (sender, e) =>
            {
                if (e.Data.SourceId == -1)
                {
                    if (e.Packet.ServerType == ServerType.UDP)
                    {
                        lastTime = Helper.GetTimeStamp();
                    }
                    else if (e.Packet.ServerType == ServerType.TCP)
                    {
                        lastTcpTime = Helper.GetTimeStamp();
                    }
                }
            };
        }

        public void Start()
        {
            string errmsg = viewModel.GetErrorMsg();
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                _ = MessageBox.Error(errmsg);
                return;
            }

            //不管三七二十一，先停止一波
            bool connecting = viewModel.IsConnecting;
            RegisterEventHandles.Instance.SendExitMessage();
            if (connecting)
            {

            }
            else
            {
                _ = Task.Run(() =>
                {
                    try
                    {
                        viewModel.IsConnecting = true;
                        ResetLastTime();

                        viewModel.ClientPort = Helper.GetRandomPort();
                        viewModel.ClientTcpPort = Helper.GetRandomPort(new List<int> { viewModel.ClientPort });
                        TCPServer.Instance.Start(viewModel.ClientTcpPort);

                        //TCP 开始监听
                        Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        serverSocket.Bind(new IPEndPoint(IPAddress.Any, viewModel.ClientTcpPort));
                        EventHandlers.Instance.TcpServer = serverSocket;
                        serverSocket.Connect(new IPEndPoint(IPAddress.Parse(viewModel.ServerIp), viewModel.ServerTcpPort));
                        TCPServer.Instance.BindReceive(serverSocket);

                        //UDP 开始监听
                        UDPServer.Instance.Start(viewModel.ClientPort);
                        EventHandlers.Instance.UdpServer = new IPEndPoint(IPAddress.Parse(viewModel.ServerIp), viewModel.ServerPort);
                        RegisterEventHandles.Instance.SendRegisterMessage(new RegisterParams
                        {
                            ClientName = viewModel.ClientName,
                            GroupId = viewModel.GroupId,
                            LocalIps = IPEndPoint.Parse(serverSocket.LocalEndPoint.ToString()).Address.ToString(),
                            Timeout = 5 * 1000,
                            Callback = (result) =>
                            {
                                viewModel.ConnectId = result.Id;
                                viewModel.IsConnecting = false;
                                viewModel.Connected = true;
                                viewModel.TcpConnected = true;
                                viewModel.GroupId = result.GroupId;
                                OnRegisterChange?.Invoke(this, true);
                                SaveConfig();
                            },
                            FailCallback = (fail) =>
                            {
                                _ = MessageBox.Error($"注册失败:{fail.Msg}");
                                viewModel.IsConnecting = false;
                                OnRegisterChange?.Invoke(this, false);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Error(ex.Message);
                        _ = MessageBox.Error($"连接失败:{ex.Message}");
                        viewModel.IsConnecting = false;
                        RegisterEventHandles.Instance.SendExitMessage();
                    }
                });
            }
        }

        private void ResetLastTime()
        {
            lastTime = 0;
            lastTcpTime = 0;
        }
        private bool IsTimeout()
        {
            long time = Helper.GetTimeStamp();
            return (lastTime > 0 && time - lastTime > 20000) || (lastTcpTime > 0 && time - lastTcpTime > 20000);
        }

        private void ReadConfig()
        {
            ConfigFileModel config = configFileName.ProtobufDeserializeFileRead<ConfigFileModel>();
            if (config != null)
            {
                viewModel.ServerIp = config.ServerIp;
                viewModel.ServerPort = config.ServerPort;
                viewModel.ServerTcpPort = config.ServerTcpPort;
                viewModel.ClientName = config.ClientName;
                viewModel.GroupId = config.GroupId;
            }
        }
        private void SaveConfig()
        {
            _ = new ConfigFileModel
            {
                ServerIp = viewModel.ServerIp,
                ServerPort = viewModel.ServerPort,
                ServerTcpPort = viewModel.ServerTcpPort,
                ClientName = viewModel.ClientName,
                GroupId = viewModel.GroupId
            }.ProtobufSerializeFileSave(configFileName);
        }
    }

    public class ConfigFileModel : IFileConfig
    {
        public ConfigFileModel() { }

        public string ServerIp { get; set; } = string.Empty;
        public int ServerPort { get; set; } = 0;
        public int ServerTcpPort { get; set; } = 0;
        public string ClientName { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
    }
}
