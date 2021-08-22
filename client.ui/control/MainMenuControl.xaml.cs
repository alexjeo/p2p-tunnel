using client.ui.plugins.forward.tcp;
using client.ui.plugins.main.viewModel;
using client.ui.plugins.rdpDesktop;
using client.ui.plugins.register;
using client.ui.plugins.remoteDesktop;
using client.ui.windows.upnp;
using client.ui.windows.wakeup;
using common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace client.ui.control
{
    /// <summary>
    /// MainMenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
        private readonly MainWindowMenuViewModel viewModel = null;
        readonly List<MenusModel> menus = null;
        readonly Dictionary<MenuTypeModel, Type> windowTypes = new Dictionary<MenuTypeModel, Type> {
            {MenuTypeModel.UPNP,typeof(UpnpWindow) },
            {MenuTypeModel.TCP_FORWARD,typeof(TcpForwardWindow) },
            {MenuTypeModel.REGISTER,typeof(RegisterWindow) },
            {MenuTypeModel.REMOTE_DESKTOP,typeof(RemoteDesktopWindow) },
            {MenuTypeModel.SHARE_DESKTOP,typeof(RdpShareSettingWindow) },
            {MenuTypeModel.WAKE_UP,typeof(WakeUpWindow) },

        };
        readonly Dictionary<MenuTypeModel, Window> windowCache = new Dictionary<MenuTypeModel, Window>();

        public MainMenuControl()
        {
            InitializeComponent();

            viewModel = new MainWindowMenuViewModel();
            DataContext = viewModel;
            menus = viewModel.Menus.ToList();

            TcpForwardHelper.Instance.OnConnectdChange += (sender, e) =>
            {
                MenusModel menu = menus.FirstOrDefault(c => c.Id == MenuTypeModel.TCP_FORWARD);
                if (menu != null)
                {
                    menu.Status = e;
                }
                viewModel.Menus = new ObservableCollection<MenusModel>(menus);
            };

            RegisterHelper.Instance.OnRegisterChange += (sender, e) =>
            {
                MenusModel menu = menus.FirstOrDefault(c => c.Id == MenuTypeModel.REGISTER);
                if (menu != null)
                {
                    menu.Status = e;
                }
                viewModel.Menus = new ObservableCollection<MenusModel>(menus);
            };
            RdpSahreHelper.Instance.OnConnectdChange += (sender, e) =>
            {
                MenusModel menu = menus.FirstOrDefault(c => c.Id == MenuTypeModel.SHARE_DESKTOP);
                if (menu != null)
                {
                    menu.Status = e;
                }
                viewModel.Menus = new ObservableCollection<MenusModel>(menus);
            };
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.OriginalSource as MenuItem;
            MenusModel model = item.Header as MenusModel;

            if (!windowCache.ContainsKey(model.Id))
            {
                Window window = Activator.CreateInstance(windowTypes[model.Id]) as Window;
                window.Show();
                window.Owner = Window.GetWindow(this);
                window.Closed += (sender, e) =>
                {
                    window = null;
                    _ = windowCache.Remove(model.Id);
                    Helper.FlushMemory();
                };
                windowCache.Add(model.Id, window);
            }
            else
            {
                Window window = windowCache[model.Id];
                _ = window.Focus();
                _ = window.Activate();
            }
        }


    }
}
