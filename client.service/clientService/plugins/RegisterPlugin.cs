using client.service.serverPlugins.register;
using common;
using System.Threading.Tasks;

namespace client.service.clientService.plugins
{
    public class RegisterPlugin : IClientServicePlugin
    {
        public void Start(ClientServicePluginExcuteWrap arg)
        {
            bool flag = false;
            RegisterHelper.Instance.Start((msg) =>
            {
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    arg.SetResultCode(-1, msg);
                }
                flag = true;
            });
            while (!flag)
            {
                System.Threading.Thread.Sleep(1);
            }
        }

        public void Stop(ClientServicePluginExcuteWrap arg)
        {
            RegisterEventHandles.Instance.SendExitMessage();
        }

        public object Info(ClientServicePluginExcuteWrap arg)
        {
            return new
            {
                AppShareData.Instance.ClientName,
                AppShareData.Instance.ClientPort,
                AppShareData.Instance.ClientTcpPort,
                AppShareData.Instance.ClientTcpPort2,
                AppShareData.Instance.AutoReg,
                AppShareData.Instance.UseMac,
                AppShareData.Instance.Mac,
                AppShareData.Instance.Connected,
                AppShareData.Instance.ConnectId,
                AppShareData.Instance.GroupId,
                AppShareData.Instance.Ip,
                AppShareData.Instance.IsConnecting,
                AppShareData.Instance.RouteLevel,
                AppShareData.Instance.ServerIp,
                AppShareData.Instance.ServerPort,
                AppShareData.Instance.ServerTcpPort,
                AppShareData.Instance.TcpConnected,
            };
        }
    }
}
