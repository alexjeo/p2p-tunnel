using server.model;

namespace server.plugin
{
    public interface IPlugin
    {
        public MessageTypes MsgType { get; }

        void Excute(PluginExcuteModel data, ServerType serverType);
    }
}
