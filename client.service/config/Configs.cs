using System.Net;

namespace client.service.config
{
    public class Config
    {
        public WebsocketConfig Websocket { get; set; } = new WebsocketConfig();
        public ClientConfig Client { get; set; } = new ClientConfig();
        public ServerConfig Server { get; set; } = new ServerConfig();
        public WebConfig Web { get; set; } = new WebConfig();
    }

    public class WebConfig
    {
        public string Ip { get; set; } = IPAddress.Any.ToString();
        public int Port { get; set; } = 8098;
        public string Path { get; set; } = "./web";

    }

    public class WebsocketConfig
    {
        public string Ip { get; set; } = IPAddress.Any.ToString();
        public int Port { get; set; } = 8098;
    }

    public class ClientConfig
    {
        public string GroupId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool AutoReg { get; set; } = false;
    }

    public class ServerConfig
    {
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; } = 8099;
        public int TcpPort { get; set; } = 8000;
    }

}
