using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client.ui.plugins.rdpDesktop
{
    /// <summary>
    /// RdpShareSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RdpShareSettingWindow : Window
    {
        public RdpShareSettingWindow()
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
    }
}
