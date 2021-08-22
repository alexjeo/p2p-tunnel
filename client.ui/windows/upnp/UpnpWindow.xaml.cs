using client.ui.plugins.main.viewModel;
using Mono.Nat;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using MessageBox = HandyControl.Controls.MessageBox;

namespace client.ui.windows.upnp
{
    /// <summary>
    /// UpnpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpnpWindow : Window
    {
        readonly UpnpViewModel viewModel = null;
        private INatDevice currentDevcice = null;
        public UpnpWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            viewModel = new UpnpViewModel();
            DataContext = viewModel;
            ProtocolComboBox.SelectedItem = viewModel.Protocols[0];

            NatUtility.DeviceFound += (object sender, DeviceEventArgs e) =>
            {
                INatDevice device = e.Device;
                if (device.NatProtocol == NatProtocol.Upnp)
                {
                    Dispatcher.Invoke(() =>
                    {
                        viewModel.Devices.Add(new DeviceModel
                        {
                            Device = device,
                            Text = $"外网:{device.GetExternalIPAsync().Result},内网:{device.DeviceEndpoint}"
                        });
                    });
                }
            };
            NatUtility.StartDiscovery();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NatUtility.StopDiscovery();
        }

        private void Device_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeviceModel item = (DeviceModel)((ComboBox)sender).SelectedItem;
            currentDevcice = item.Device;
            UpdateMappings();

        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentDevcice.DeletePortMap(viewModel.Mappings[dataGrid.SelectedIndex]);
                UpdateMappings();
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message);
            }
        }

        private void UpdateMappings()
        {
            try
            {
                viewModel.Mappings = new ObservableCollection<Mapping>(currentDevcice.GetAllMappings());
            }
            catch (Exception ex)
            {
                _ = MessageBox.Error(ex.Message);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string errmsg = viewModel.GetErrorMsg();
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                _ = MessageBox.Error(errmsg);
                return;
            }
            if(currentDevcice == null)
            {
                _ = MessageBox.Error("请选择一个设备！");
                return;
            }
            try
            {
                _ = currentDevcice.CreatePortMap(new Mapping(viewModel.Protocol, viewModel.PrivatePort, viewModel.PublicPort, 0, viewModel.Description));
                UpdateMappings();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Error(ex.Message);
            }
        }

        private void ProtocolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProtocolModel item = (ProtocolModel)((ComboBox)sender).SelectedItem;
            viewModel.Protocol = item.Protocol;
        }
    }
}
