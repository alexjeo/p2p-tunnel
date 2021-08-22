using System;
using System.Windows;
using System.Windows.Controls;
using MessageBox = HandyControl.Controls.MessageBox;

namespace client.ui.plugins.remoteDesktop
{
    /// <summary>
    /// HomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class HomeControl : UserControl
    {
        private RemoteDesktopViewModel viewModel;

        public HomeControl()
        {
            

            InitializeComponent();

            viewModel = RemoteDesktopHelper.Instance.viewModel;

            DataContext = viewModel;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            string errmsg = viewModel.GetErrorMsg();
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                _ = MessageBox.Error(errmsg);
                return;
            }
            RemoteDesktopHelper.Instance.Add(new RemoteDesktopInfo
            {
                Desc = viewModel.Desc,
                Ip = viewModel.Ip,
                Password = viewModel.Password,
                Port = viewModel.Port,
                UserName = viewModel.UserName,
            });
        }

        private void DelClick(object sender, RoutedEventArgs e)
        {
            RemoteDesktopHelper.Instance.Del(dataGrid.SelectedIndex);
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            RemoteDesktopHelper.Instance.Open(viewModel.Desktops[dataGrid.SelectedIndex]);
    }
    }
}
