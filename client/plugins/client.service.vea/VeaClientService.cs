﻿using client.service.ui.api.clientServer;
using common.libs.extends;
using System.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace client.service.vea
{
    public class VeaClientService : IClientService
    {
        private readonly Config config;
        private readonly VeaTransfer virtualEthernetAdapterTransfer;
        private IVeaSocks5ServerHandler veaSocks5ServerHandler;

        public VeaClientService(Config config, VeaTransfer virtualEthernetAdapterTransfer, IVeaSocks5ServerHandler veaSocks5ServerHandler)
        {
            this.config = config;
            this.virtualEthernetAdapterTransfer = virtualEthernetAdapterTransfer;
            this.veaSocks5ServerHandler = veaSocks5ServerHandler;
        }

        public Config Get(ClientServiceParamsInfo arg)
        {
            return config;
        }
        public void Set(ClientServiceParamsInfo arg)
        {
            var conf = arg.Content.DeJson<Config>();

            config.Enable = conf.Enable;
            config.ProxyAll = conf.ProxyAll;
            config.TargetName = conf.TargetName;
            config.IP = conf.IP;
            config.LanIP = conf.LanIP;
            config.TunnelType = conf.TunnelType;
            config.SocksPort = conf.SocksPort;
            config.BufferSize = conf.BufferSize;
            config.ConnectEnable = conf.ConnectEnable;
            config.LanConnectEnable = conf.LanConnectEnable;

            veaSocks5ServerHandler.UpdateConfig();

            try
            {
                virtualEthernetAdapterTransfer.Run();
            }
            catch (Exception ex)
            {
                arg.SetCode(ClientServiceResponseCodes.Error, ex.Message);
            }

            config.SaveConfig().Wait();
        }

        public Dictionary<ulong, IPAddressCacheInfo> Update(ClientServiceParamsInfo arg)
        {
            return virtualEthernetAdapterTransfer.IPList.ToDictionary(c => c.Value.Client.Id, d => d.Value);
        }
    }
}