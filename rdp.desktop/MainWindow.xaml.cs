using System.Windows;

namespace rdp.desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RdpShareWindow viewWindow;

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = RdpSahreHelper.Instance.ViewModel;
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            RdpSahreHelper.Instance.Start();
        }

        private void ConnectServerClick(object sender, RoutedEventArgs e)
        {
            if (viewWindow != null) return;
            if (string.IsNullOrWhiteSpace(RdpSahreHelper.Instance.ViewModel.ServerConnectString))
            {
                HandyControl.Controls.MessageBox.Error("请填写服务连接串");
                return;
            }

            viewWindow = new RdpShareWindow(RdpSahreHelper.Instance.ViewModel.ServerConnectString, RdpSahreHelper.Instance.ViewModel.ServerPassword);
            viewWindow.Closed += (s, e) =>
            {
                viewWindow = null;
            };
            viewWindow.Show();
        }

        private void GetClientConnectStringClick(object sender, RoutedEventArgs e)
        {
            if (viewWindow != null) return;
            if (string.IsNullOrWhiteSpace(RdpSahreHelper.Instance.ViewModel.ServerConnectString))
            {
                HandyControl.Controls.MessageBox.Error("请填写服务连接串");
                return;
            }

            viewWindow = new RdpShareWindow(RdpSahreHelper.Instance.ViewModel.ServerConnectString, RdpSahreHelper.Instance.ViewModel.ServerPassword, (connectString) =>
            {
                RdpSahreHelper.Instance.ViewModel.ClientConnectString = connectString;
            });
            viewWindow.Closed += (s,e) =>
            {
                viewWindow = null;
            };

            viewWindow.Show();
        }

        private void ConnectClientClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RdpSahreHelper.Instance.ViewModel.ClientConnectString))
            {
                HandyControl.Controls.MessageBox.Error("请填写客户端连接串");
                return;
            }
            RdpSahreHelper.Instance.ConnectClient(RdpSahreHelper.Instance.ViewModel.ClientConnectString);
        }

    }
}
