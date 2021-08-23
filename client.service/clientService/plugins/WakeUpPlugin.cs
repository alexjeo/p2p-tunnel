using client.service.serverPlugins.clients;
using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client.service.clientService.plugins
{
    public class WakeUpPlugin : IClientServicePlugin
    {
        public void WakeUp(ClientServicePluginExcuteWrap arg)
        {
            WakeUpModel model = Helper.DeJsonSerializer<WakeUpModel>(arg.Content);

            if (AppShareData.Instance.Clients.TryGetValue(model.ID, out ClientInfo client))
            {
                if (client != null)
                {
                    Send(client.Mac, client.Address.Address.ToString(), client.Address.Port);
                }
            }
        }


        private void Send(string mac, string ip, int port)
        {
            using UdpClient udp = new();

            byte[] packet = new byte[6 + (16 * 6)];
            for (int i = 0; i < 6; i++)
            {
                packet[i] = 0xFF;
            }

            byte[] macs = mac.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    packet[6 + (i * 6) + j] = macs[j];
                }
            }
            IPEndPoint endpoint = new(IPAddress.Parse(ip), port);
            _ = udp.Send(packet, packet.Length, endpoint);
        }
    }

    public class WakeUpModel
    {
        public long ID { get; set; } = 0;
    }


}
