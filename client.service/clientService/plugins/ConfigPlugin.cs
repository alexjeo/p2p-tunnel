using common;

namespace client.service.clientService.plugins
{

    /// <summary>
    /// 客户端配置
    /// </summary>
    public class ConfigPlugin : IClientServicePlugin
    {
        public void Update(ClientServicePluginExcuteWrap arg)
        {
            SettingModel model = Helper.DeJsonSerializer<SettingModel>(arg.Content);

            AppShareData.Instance.ClientName = model.ClientName;
            AppShareData.Instance.GroupId = model.GroupId;
            AppShareData.Instance.AutoReg = model.AutoReg;
            AppShareData.Instance.UseMac = model.UseMac;

            AppShareData.Instance.ServerIp = model.ServerIp;
            AppShareData.Instance.ServerPort = model.ServerPort;
            AppShareData.Instance.ServerTcpPort = model.ServerTcpPort;

            AppShareData.Instance.SaveConfig();
        }
    }

    public class SettingModel
    {
        /// <summary>
        /// 客户端名
        /// </summary>
        public string ClientName { get; set; } = string.Empty;
        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupId { get; set; } = string.Empty;

        /// <summary>
        /// 自动注册
        /// </summary>
        public bool AutoReg { get; set; } = false;
        /// <summary>
        /// 是否上报MAC地址
        /// </summary>
        public bool UseMac { get; set; } = false;

        /// <summary>
        /// NAT服务地址
        /// </summary>
        public string ServerIp { get; set; } = string.Empty;
        /// <summary>
        /// NAT服务UDP端口
        /// </summary>
        public int ServerPort { get; set; } = 0;
        /// <summary>
        /// NAT服务TCP端口
        /// </summary>
        public int ServerTcpPort { get; set; } = 0;
    }
}
