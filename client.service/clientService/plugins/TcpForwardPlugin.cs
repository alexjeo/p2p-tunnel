using client.service.serverPlugins.forward.tcp;
using common;
using System.Collections.Generic;

namespace client.service.clientService.plugins
{
    public class TcpForwardPlugin : IClientServicePlugin
    {
        public void Add(ClientServicePluginExcuteWrap arg)
        {
            ForwardSettingModel model = Helper.DeJsonSerializer<ForwardSettingModel>(arg.Content);

            TcpForwardRecordBaseModel fmodel = Helper.DeJsonSerializer<TcpForwardRecordBaseModel>(model.Content);
            string errmsg = TcpForwardHelper.Instance.Add(new TcpForwardRecordBaseModel
            {
                AliveType = fmodel.AliveType,
                Listening = false,
                SourceIp = fmodel.SourceIp,
                SourcePort = fmodel.SourcePort,
                TargetIp = fmodel.TargetIp,
                TargetName = fmodel.TargetName,
                TargetPort = fmodel.TargetPort
            }, model.ID);
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                arg.SetResultCode(-1, errmsg);
            }

        }

        public void Del(ClientServicePluginExcuteWrap arg)
        {
            ForwardSettingModel model = Helper.DeJsonSerializer<ForwardSettingModel>(arg.Content);
            string errmsg = TcpForwardHelper.Instance.Del(model.ID);
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                arg.SetResultCode(-1, errmsg);
            }
        }

        public List<TcpForwardRecordBaseModel> List(ClientServicePluginExcuteWrap arg)
        {
            return TcpForwardHelper.Instance.Mappings;
        }

        public void Start(ClientServicePluginExcuteWrap arg)
        {
            ForwardSettingModel model = Helper.DeJsonSerializer<ForwardSettingModel>(arg.Content);
            string errmsg = TcpForwardHelper.Instance.Start(model.ID);
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                arg.SetResultCode(-1, errmsg);
            }
        }

        public void Stop(ClientServicePluginExcuteWrap arg)
        {
            ForwardSettingModel model = Helper.DeJsonSerializer<ForwardSettingModel>(arg.Content);
            TcpForwardHelper.Instance.Stop(model.ID);
        }
    }

    public class ForwardSettingModel
    {
        public int ID { get; set; } = 0;
        public string Content { get; set; } = string.Empty;
    }
}
