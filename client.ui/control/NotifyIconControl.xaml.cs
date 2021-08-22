using client.ui.plugins.forward.tcp;
using client.ui.plugins.main.viewModel;
using client.ui.plugins.rdpDesktop;
using client.ui.plugins.register;
using common;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace client.ui.control
{
    /// <summary>
    /// NotifyIcon.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyIconControl : UserControl
    {
        private readonly MainWindowNotifyIconViewModel viewModel;
        private ConnectViewModel RegisterViewModel => RegisterHelper.Instance.viewModel;
        private RdpShareSettingViewModel RdpShareDesktopViewModel => RdpSahreHelper.Instance.ViewModel;
        private TcpForwardViewModel TcpForwardViewModel => TcpForwardHelper.Instance.Viewmodel;

        public NotifyIconControl()
        {
            InitializeComponent();

            viewModel = new MainWindowNotifyIconViewModel();
            DataContext = viewModel;

            UpdateTitle();
            TcpForwardHelper.Instance.OnConnectdChange += (sender, e) =>
            {
                TriggerTcpForwardStatus(e);
                UpdateTitle();
            };
            RegisterEventHandles.Instance.OnRegisterTcpResultHandler += (sender, e) =>
            {
                TriggerRegisterMenuStatus();
                UpdateTitle();
            };
            RdpSahreHelper.Instance.OnConnectdChange += (sender, state) =>
            {
                TriggerRdpShareDesktopStatus(state);
                UpdateTitle();
            };
        }

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.OriginalSource as MenuItem;
            NotifyIconMenusModel model = item.Header as NotifyIconMenusModel;

            switch (model.Id)
            {
                case NotifyIconMenuTypeModel.REGISTER:
                    TriggerRegister();
                    break;
                case NotifyIconMenuTypeModel.TCP_FORWARD:
                    TriggerTcpForward();
                    break;
                case NotifyIconMenuTypeModel.RDP_SHARE_DESKTOP:
                    TriggerRdpShareDesktop();
                    break;
                case NotifyIconMenuTypeModel.EXIT:
                    Application.Current.Shutdown();
                    break;
                default:
                    break;
            }
        }

        private void TriggerTcpForward()
        {
            if (TcpForwardHelper.Instance.Viewmodel.Connected)
            {
                TcpForwardHelper.Instance.StopAll();
            }
            else
            {
                TcpForwardHelper.Instance.StartAll();
            }
        }
        private void TriggerTcpForwardStatus(bool state)
        {
            var register = viewModel.Menus.FirstOrDefault(c => c.Id == NotifyIconMenuTypeModel.TCP_FORWARD);
            if (register != null)
            {
                register.Name = TcpForwardViewModel.Connected ? $"开启TCP转发" : $"关闭TCP转发";
                register.Status = TcpForwardViewModel.Connected;
            }

            Dispatcher.Invoke(() =>
            {
                notifyIcon.ShowBalloonTip("TCP转发",
                   string.Join("\r\n", new string[] {
                       state? "已开启":"已关闭",
                       string.Join(",",TcpForwardHelper.Instance.Mappings.Select(c=>{
                        return "["+(c.AliveType == TcpForwardAliveTypes.ALIVE?"长连接":"短连接")+"][" +(c.Listening?"已开启":"未开启") +$"]:{c.SourceIp}:{c.SourcePort}->{c.TargetName}:{c.TargetPort}";
                       })),
                    })
                    , HandyControl.Data.NotifyIconInfoType.Info);
            });
        }

        private void TriggerRegister()
        {
            RegisterHelper.Instance.Start();
        }
        private void TriggerRegisterMenuStatus()
        {
            var register = viewModel.Menus.FirstOrDefault(c => c.Id == NotifyIconMenuTypeModel.REGISTER);
            if (register != null)
            {
                register.Name = RegisterViewModel.TcpConnected ? "重新注册" : "注册";
                register.Status = RegisterViewModel.TcpConnected;
            }
            Dispatcher.Invoke(() =>
            {
                notifyIcon.ShowBalloonTip("服务注册", "已注册", HandyControl.Data.NotifyIconInfoType.Info);
            });
        }

        private void TriggerRdpShareDesktop()
        {
            if (RdpShareDesktopViewModel.Connected)
            {
                RdpSahreHelper.Instance.Stop();
            }
            else
            {
                RdpSahreHelper.Instance.Start();
            }
        }
        private void TriggerRdpShareDesktopStatus(bool state)
        {
            var share = viewModel.Menus.FirstOrDefault(c => c.Id == NotifyIconMenuTypeModel.RDP_SHARE_DESKTOP);
            if (share != null)
            {
                share.Name = state ? "关闭桌面分享" : "开启桌面分享";
                share.Status = state;
            }

            Dispatcher.Invoke(() =>
            {
                notifyIcon.ShowBalloonTip("桌面分享", state ? "桌面分享已开启" : "桌面分享已关闭", HandyControl.Data.NotifyIconInfoType.Info);
            });
        }

        private void UpdateTitle()
        {
            Dispatcher.Invoke(() =>
            {
                viewModel.Title = string.Join("\r\n", new string[] {
                    $"注册状态："+(RegisterViewModel.TcpConnected?"已注册":"未注册"),

                    $"TCP转发："+(TcpForwardViewModel.Connected?string.Join("\r\n",new string[]{
                       "",
                       string.Join(",",TcpForwardHelper.Instance.Mappings.Select(c=>{
                            return "    ["+(c.AliveType == TcpForwardAliveTypes.ALIVE?"长连接":"短连接")+"][" +(c.Listening?"已开启":"未开启") +$"]:{c.SourceIp}:{c.SourcePort}->{c.TargetName}:{c.TargetPort}";
                       }))
                     }):"未开启"),

                    $"桌面共享："+(RdpShareDesktopViewModel.Connected?"已开启":"未开启"),
                });
                notifyIcon.Visibility = Visibility.Hidden;
                Helper.SetTimeout(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        notifyIcon.Visibility = Visibility.Visible;
                    });
                }, 100);
            });

        }
    }
}
