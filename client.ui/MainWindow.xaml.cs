using client.ui.plugins.rdpDesktop;
using client.ui.plugins.register;
using System.Windows;
using Application = System.Windows.Application;

namespace client.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HandyControl.Controls.Growl.SetGrowlParent(MessagePanel, true);

            notifyIcon.notifyIcon.Click += (sender, e) =>
            {
                Show();
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }
            };

            Application.Current.Exit += (object sender, ExitEventArgs e) =>
            {
                RegisterEventHandles.Instance.SendExitMessage();
            };
            Application.Current.SessionEnding += (sender, e) =>
            {
                RegisterEventHandles.Instance.SendExitMessage();
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            WindowState = WindowState.Minimized;
            notifyIcon.notifyIcon.Visibility = Visibility.Visible;
        }
    }

}
