using client.service.clientService;
using client.service.clientService.plugins;
using client.service.config;
using client.service.serverPlugins.chat;
using client.service.serverPlugins.clients;
using client.service.serverPlugins.forward.tcp;
using client.service.serverPlugins.register;
using common;
using System;
using System.IO;

namespace client.service
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = Helper.DeJsonSerializer<Config>(File.ReadAllText("appsettings.json"));

            //客户端信息
            AppShareData.Instance.ClientName = config.Client.Name;
            AppShareData.Instance.GroupId = config.Client.GroupId;
            AppShareData.Instance.AutoReg = config.Client.AutoReg;

            //服务器信息
            AppShareData.Instance.ServerIp = config.Server.Ip;
            AppShareData.Instance.ServerPort = config.Server.Port;
            AppShareData.Instance.ServerTcpPort = config.Server.TcpPort;

            //客户端websocket
            ClientServer.Instance.Start(config.Websocket.Ip, config.Websocket.Port);

            //p2p聊天
            P2PChatHelper.Instance.Start();
            //端点
            ClientsHelper.Instance.Start();
            //TCP转发
            TcpForwardHelper.Instance.Start();
            //UPNP
            UpnpHelper.Instance.Start();

            //静态web服务
            WebServer.Instance.Start(config.Web.Ip, config.Web.Port, config.Web.Path);

            //自动注册
            if (AppShareData.Instance.AutoReg)
            {
                RegisterHelper.Instance.AutoReg();
            }

            //退出事件
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                RegisterEventHandles.Instance.SendExitMessage();
            };
            Console.CancelKeyPress += (s, e) =>
            {
                RegisterEventHandles.Instance.SendExitMessage();
            };

            //日志输出
            Logger.Instance.OnLogger += (sender, model) =>
            {
                Console.WriteLine($"[{model.Type}][{model.Time:yyyy-MM-dd HH:mm:ss}]:{model.Content}");
            };


            Console.WriteLine("=======================================");
            Console.WriteLine("没什么报红的报黄的，就说明运行成功了");
            Console.WriteLine("=======================================");
            Console.WriteLine($"前端管理地址:http://{config.Web.Ip}:{config.Web.Port}");
            Console.WriteLine($"管理通信地址:ws://{config.Websocket.Ip}:{config.Websocket.Port}");


            Console.ReadLine();
        }
    }
}
