using server.packet;
using System.Net;
using System.Net.Sockets;

namespace server.model
{
    public class PluginExcuteModel
    {
        /// <summary>
        /// 来源地址
        /// </summary>
        public IPEndPoint SourcePoint { get; set; }

        /// <summary>
        /// 发送数据
        /// </summary>
        public IPacket Packet { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public ServerType ServerType { get; set; }

        /// <summary>
        /// 消息对象
        /// </summary>
        public Socket TcpSocket { get; set; }
    }
}
