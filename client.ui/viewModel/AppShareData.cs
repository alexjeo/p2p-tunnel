using client.ui.plugins.clients;
using common.extends;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Net;

namespace client.ui.viewModel
{
    public class AppShareData : ViewModelBase
    {
        private static readonly Lazy<AppShareData> lazy = new Lazy<AppShareData>(() => new AppShareData());
        public static AppShareData Instance => lazy.Value;
        private AppShareData()
        {
        }

        //路由网关数，与外网的距离
        private int routeLevel = 0;
        public int RouteLevel
        {
            get => routeLevel;
            set
            {
                routeLevel = value;
                RaisePropertyChanged(() => RouteLevel);
            }
        }

        //外网ip
        private string ip = string.Empty;
        public string Ip
        {
            get => ip;
            set
            {
                ip = value;
                RaisePropertyChanged(() => Ip);
            }
        }

        //客户端名
        private string clientName = Dns.GetHostName().SubStr(0, 20);
        public string ClientName
        {
            get => clientName;
            set
            {
                clientName = value;
                RaisePropertyChanged(() => ClientName);
            }
        }

        //UDP是否已连接
        private bool connected = false;
        public bool Connected
        {
            get => connected;
            set
            {
                connected = value;
                RaisePropertyChanged(() => Connected);
            }
        }
        //TCP是否已连接
        private bool tcpCconnected = false;
        public bool TcpConnected
        {
            get => tcpCconnected;
            set
            {
                tcpCconnected = value;
                RaisePropertyChanged(() => TcpConnected);
            }
        }

        //在线客户端列表
        private ObservableCollection<ClientInfo> clients = new ObservableCollection<ClientInfo>();
        public ObservableCollection<ClientInfo> Clients
        {
            get => clients;
            set
            {
                clients = value;
                RaisePropertyChanged(() => Clients);
            }
        }

        //当前选中客户端
        private ClientInfo currentClientInfo = new ClientInfo();
        public ClientInfo CurrentClientInfo
        {
            get => currentClientInfo;
            set
            {
                currentClientInfo = value;
                RaisePropertyChanged(() => CurrentClientInfo);
            }
        }

        //NAT服务地址
        private string serverIp = "120.79.205.184";
        public string ServerIp
        {
            get => serverIp;
            set
            {
                serverIp = value;
                RaisePropertyChanged(() => ServerIp);
            }
        }
        //NAT服务UDP端口
        private int serverPort = 8099;
        public int ServerPort
        {
            get => serverPort;
            set
            {
                serverPort = value;
                RaisePropertyChanged(() => ServerPort);
            }
        }
        //NAT服务TCP端口
        private int serverTcpPort = 8000;
        public int ServerTcpPort
        {
            get => serverTcpPort;
            set
            {
                serverTcpPort = value;
                RaisePropertyChanged(() => ServerTcpPort);
            }
        }

        //客户端UDP端口
        private int clientPort = 8099;
        public int ClientPort
        {
            get => clientPort;
            set
            {
                clientPort = value;
                RaisePropertyChanged(() => ClientPort);
            }
        }
        //客户端TCP端口
        private int clientTcpPort = 8000;
        public int ClientTcpPort
        {
            get => clientTcpPort;
            set
            {
                clientTcpPort = value;
                RaisePropertyChanged(() => ClientTcpPort);
            }
        }

        //是否正在连接
        private bool isConnecting = false;
        public bool IsConnecting
        {
            get => isConnecting;
            set
            {
                isConnecting = value;
                RaisePropertyChanged(() => IsConnecting);
            }
        }

        //连接ID 身份标识
        private long connectId = 0;
        public long ConnectId
        {
            get => connectId;
            set
            {
                connectId = value;
                RaisePropertyChanged(() => ConnectId);
            }
        }
    }
}
