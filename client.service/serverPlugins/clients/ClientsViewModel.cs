using common;
using System.Net;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace client.service.serverPlugins.clients
{

    public class ClientInfo
    {
        private bool connecting = false;
        public bool Connecting
        {
            get => connecting;
            set
            {
                connecting = value;
            }
        }

        private bool tcpConnecting = false;
        public bool TcpConnecting
        {
            get => tcpConnecting;
            set
            {
                tcpConnecting = value;
            }
        }

        private bool connected = false;
        public bool Connected
        {
            get => connected;
            set
            {
                connected = value;
            }
        }

        private bool tcpConnected = false;
        public bool TcpConnected
        {
            get => tcpConnected;
            set
            {
                tcpConnected = value;
            }
        }

        [JsonIgnore]
        public Socket Socket { get; set; } = null;
        [JsonIgnore]
        public IPEndPoint Address { get; set; } = null;

        public int Port { get; set; } = 0;
        public int TcpPort { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;

        public long Id { get; set; } = 0;

        public long LastTime { get; set; } = 0;
        public long TcpLastTime { get; set; } = 0;

        public bool IsTimeout()
        {
            return (LastTime > 0 && Helper.GetTimeStamp() - LastTime > 20000);
        }

        public bool IsTcpTimeout()
        {
            return (TcpLastTime > 0 && Helper.GetTimeStamp() - TcpLastTime > 20000);
        }
    }
}
