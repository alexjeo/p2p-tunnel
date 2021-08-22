using client.ui.plugins.register;
using System.Windows;

namespace client.ui.plugins.register
{
    /// <summary>
    /// ConnectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            DataContext = RegisterHelper.Instance.viewModel;
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterHelper.Instance.Start();
        }
    }
}
