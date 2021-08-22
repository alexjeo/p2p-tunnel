using client.service.serverPlugins.clients;
using common;
using System.Collections.Generic;
using System.Linq;

namespace client.service.clientService.plugins
{
    public class ClientsPlugin : IClientServicePlugin
    {
        public List<ClientInfo> List(ClientServicePluginExcuteWrap arg)
        {
            return AppShareData.Instance.Clients.Values.ToList();
        }

        public void Connect(ClientServicePluginExcuteWrap arg)
        {
            ConnectModel model = Helper.DeJsonSerializer<ConnectModel>(arg.Content);
            ClientsHelper.Instance.ConnectClient(model.ID);
        }
    }

    public class ConnectModel
    {
        public long ID { get; set; } = 0;
    }
}
