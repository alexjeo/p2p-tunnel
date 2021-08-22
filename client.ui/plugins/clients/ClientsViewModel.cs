using client.ui.viewModel;
using common;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.clients
{
    public class ClientsViewModel : ViewModelBase
    {
        private string toolTip = Dns.GetHostName();
        public string ToolTip
        {
            get => toolTip;
            set
            {
                toolTip = value;
                RaisePropertyChanged(() => ToolTip);
            }
        }


        private string ip = AppShareData.Instance.Ip;
        public string Ip
        {
            get => ip;
            set
            {
                AppShareData.Instance.Ip = ip = value;
                RaisePropertyChanged(() => Ip);
                ChangeToolTip();
            }
        }

        private int routeLevel = AppShareData.Instance.RouteLevel;
        public int RouteLevel
        {
            get => routeLevel;
            set
            {
                AppShareData.Instance.RouteLevel = routeLevel = value;
                RaisePropertyChanged(() => RouteLevel);
                ChangeToolTip();
            }
        }


        private void ChangeToolTip()
        {
            ToolTip = string.Join("\r\n", new List<string>
            {
                AppShareData.Instance.ClientName,$"外网IP:{Ip}",$"外网距离:{RouteLevel}个路由",$"本地端口:{AppShareData.Instance.ClientTcpPort}"
            });
        }

        private ObservableCollection<ClientInfo> clients = new ObservableCollection<ClientInfo>();
        public ObservableCollection<ClientInfo> Clients
        {
            get => clients;
            set
            {
                clients = value;
                AppShareData.Instance.Clients = clients;
                RaisePropertyChanged(() => Clients);
            }
        }

    }

    public class ClientInfo : ViewModelBase
    {
        private bool connecting = false;
        public bool Connecting
        {
            get => connecting;
            set
            {
                connecting = value;
                RaisePropertyChanged(() => Connecting);
            }
        }

        private bool tcpConnecting = false;
        public bool TcpConnecting
        {
            get => tcpConnecting;
            set
            {
                tcpConnecting = value;
                RaisePropertyChanged(() => TcpConnecting);
            }
        }

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

        private bool tcpConnected = false;
        public bool TcpConnected
        {
            get => tcpConnected;
            set
            {
                tcpConnected = value;
                RaisePropertyChanged(() => TcpConnected);
            }
        }

        private int ping = 0;
        public int Ping
        {
            get => ping;
            set
            {
                ping = value;
                RaisePropertyChanged(() => Ping);
            }
        }

        private int msgCount = 0;
        public int MsgCount
        {
            get => msgCount;
            set
            {
                msgCount = value;
                ShowBadge = msgCount >= 1;
                RaisePropertyChanged(() => MsgCount);
            }
        }
        private bool showBadge = false;
        public bool ShowBadge
        {
            get => showBadge;
            set
            {
                showBadge = value;
                RaisePropertyChanged(() => ShowBadge);
            }
        }


        public Socket Socket { get; set; } = null;
        public IPEndPoint Address { get; set; } = null;

        public int Port { get; set; } = 0;
        public int TcpPort { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public long Id { get; set; } = 0;

        public long LastTime { get; set; } = 0;
        public long TcpLastTime { get; set; } = 0;

        public bool IsTimeout()
        {
            //Logger.Instance.Error($"{Name}:LastTime->{LastTime},TcpLastTime->{TcpLastTime}");
            long time = Helper.GetTimeStamp();
            return (LastTime > 0 && time - LastTime > 20000) || (TcpLastTime > 0 && time - TcpLastTime > 20000);
        }
    }
}
