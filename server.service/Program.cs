using common;
using common.cache;
using server.model;
using server.service.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace server.test
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = Helper.DeJsonSerializer<Config>(File.ReadAllText("appsettings.json"));

            UDPServer.Instance.Start(config.Udp);
            TCPServer.Instance.Start(config.Tcp);
            Helper.SetInterval(() =>
            {
                try
                {
                    List<RegisterCacheModel> clients = ClientRegisterCache.Instance.GetAll();
                    foreach (RegisterCacheModel client in clients)
                    {
                        IEnumerable<MessageClientsClientModel> clientResults = clients
                        .Where(c => c.GroupId == client.GroupId)
                        .Select(c => new MessageClientsClientModel
                        {
                            Address = c.Address.Address.ToString(),
                            Id = c.Id,
                            Name = c.Name,
                            Port = c.Address.Port,
                            TcpPort = c.TcpPort
                        });

                        UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = client.Address,
                            Data = new MessageClientsModel
                            {
                                Clients = clientResults
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    Logger.Instance.Info($"发送广播客户端消息错误!{e.Message}");
                }

            }, 1000);

            Logger.Instance.OnLogger += (sender, model) =>
            {
                Console.WriteLine($"[{model.Type}][{model.Time:yyyy-MM-dd HH:mm:ss}]:{model.Content}");
            };

            _ = Console.ReadLine();
        }
    }
}
