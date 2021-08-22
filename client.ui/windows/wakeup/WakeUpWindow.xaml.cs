using common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace client.ui.windows.wakeup
{
    /// <summary>
    /// WakeUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WakeUpWindow : Window
    {
        WakeUpViewModel viewModel;
        public WakeUpWindow()
        {
            InitializeComponent();

            viewModel = new WakeUpViewModel();
            DataContext = viewModel;

            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            IEnumerable<string> ips = Dns.GetHostAddresses(Dns.GetHostName())
                .Where(c => c.AddressFamily == AddressFamily.InterNetwork)
                .Where(c => c.GetAddressBytes()[3] != 1)
                .Select(c => c.ToString());
            viewModel.Ips = new ObservableCollection<string>(ips);
            if (viewModel.Ips.Count > 0)
            {
                IpsComboBox.SelectedIndex = 0;
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

        private void WakeUpClick(object sender, RoutedEventArgs e)
        {
            WakeUp(viewModel.Mac, viewModel.Ip, 8888);
            _ = HandyControl.Controls.MessageBox.Info("已发送");
        }

        private void WakeUp(string ip, string mac, int port = 8888)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(mac))
            {
                mac = Helper.GetMacAddress(ip);
            }
            if (string.IsNullOrWhiteSpace(mac))
            {
                return;
            }
            Send(mac, ip, port);
        }

        private void WakeUpAllClick(object sender, RoutedEventArgs e)
        {
            string ip = IpsComboBox.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(ip))
            {
                _ = HandyControl.Controls.MessageBox.Error("请选择网段");
                return;
            }

            _ = HandyControl.Controls.MessageBox.Info("已发送");
            _ = Task.Run(() =>
              {
                  byte[] address = IPAddress.Parse(ip).GetAddressBytes();
                  for (byte i = 2; i <= 255; i++)
                  {
                      address[3] = i;
                      WakeUp(address.ToString(), string.Empty);
                  }
              });
        }
    }
}
