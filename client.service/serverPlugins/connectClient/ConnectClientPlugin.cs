using common.extends;
using server.model;
using server.plugin;

namespace client.service.serverPlugins.connectClient
{
    /// <summary>
    /// 具体流程 看MessageTypes里的描述
    /// </summary>
    public class ConnectClientStep1Plugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.P2P_STEP_1;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageConnectClientStep1Model data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep1Model>();
            if (serverType == ServerType.UDP)
            {
                ConnectClientEventHandles.Instance.OnConnectClientStep1Message(new OnConnectClientStep1EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
            else if (serverType == ServerType.TCP)
            {
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep1Message(new OnConnectClientStep1EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }


    public class ConnectClientStep2Plugin : IPlugin
    {
        public  MessageTypes MsgType => MessageTypes.P2P_STEP_2;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageConnectClientStep2Model data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep2Model>();

            if (serverType == ServerType.UDP)
            {
                ConnectClientEventHandles.Instance.OnConnectClientStep2Message(new OnConnectClientStep2EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
            else if (serverType == ServerType.TCP)
            {
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep2Message(new OnConnectClientStep2EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }

    public class ConnectClientStep2RetryPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P_STEP_2_RETRY;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            if (serverType == ServerType.TCP)
            {
                MessageConnectClientStep2RetryModel data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep2RetryModel>();
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep2RetryMessage(new OnConnectClientStep2RetryEventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }

    public class ConnectClientStep2FailPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P_STEP_2_FAIL;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageConnectClientStep2FailModel data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep2FailModel>();

            if (serverType == ServerType.UDP)
            {
                //EventHandlers.Instance.OnConnectClientStep2FailMessage(new OnConnectClientStep2FailEventArg
                //{
                //    Data = data,
                //    Packet = model,
                //});
            }
            else if (serverType == ServerType.TCP)
            {
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep2FailMessage(new OnTcpConnectClientStep2FailEventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }

    public class ConnectClientStep3Plugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.P2P_STEP_3;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageConnectClientStep3Model data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep3Model>();

            if (serverType == ServerType.UDP)
            {
                ConnectClientEventHandles.Instance.OnConnectClientStep3Message(new OnConnectClientStep3EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
            else if (serverType == ServerType.TCP)
            {
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep3Message(new OnConnectClientStep3EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }

    public class ConnectClientStep4Plugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.P2P_STEP_4;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageConnectClientStep4Model data = model.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep4Model>();

            if (serverType == ServerType.UDP)
            {
                ConnectClientEventHandles.Instance.OnConnectClientStep4Message(new OnConnectClientStep4EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
            else if (serverType == ServerType.TCP)
            {
                ConnectClientEventHandles.Instance.OnTcpConnectClientStep4Message(new OnConnectClientStep4EventArg
                {
                    Data = data,
                    Packet = model,
                });
            }
        }
    }
}
