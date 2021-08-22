using client.ui.plugins.clients;
using client.ui.plugins.forward;
using System.Windows;
using System.Windows.Controls;

namespace client.ui.plugins.forward.tcp
{
    /// <summary>
    /// TcpForwardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TcpForwardWindow : Window
    {

        public TcpForwardWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DataContext = TcpForwardHelper.Instance.Viewmodel;
        }

        private void StartAllClick(object sender, RoutedEventArgs e)
        {
            TcpForwardHelper.Instance.StartAll();
        }
        private void StopAllClick(object sender, RoutedEventArgs e)
        {
            TcpForwardHelper.Instance.StopAll();
        }

        private void DelClick(object sender, RoutedEventArgs e)
        {
            TcpForwardHelper.Instance.Del(dataGrid.SelectedItem as TcpForwardRecordBaseModel);
            TcpForwardHelper.Instance.Viewmodel.CurrentIndex = -1;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            TcpForwardRecordBaseModel model = dataGrid.SelectedItem as TcpForwardRecordBaseModel;
            if (model.Listening)
            {
                TcpForwardHelper.Instance.Stop(model);
            }
            else
            {
                TcpForwardHelper.Instance.Start(model);
            }
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            string msg = TcpForwardHelper.Instance.Viewmodel.GetErrorMsg();
            if (!string.IsNullOrWhiteSpace(msg))
            {
                HandyControl.Controls.MessageBox.Error(msg);
                return;
            }

            TcpForwardHelper.Instance.Add(new TcpForwardRecordBaseModel
            {
                SourceIp = TcpForwardHelper.Instance.Viewmodel.SourceIp,
                Listening = false,
                SourcePort = TcpForwardHelper.Instance.Viewmodel.SourcePort,
                TargetName = TcpForwardHelper.Instance.Viewmodel.TargetName,
                TargetPort = TcpForwardHelper.Instance.Viewmodel.TargetPort,
                AliveType = TcpForwardHelper.Instance.Viewmodel.AliveType,
                TargetIp = TcpForwardHelper.Instance.Viewmodel.TargetIp,
            }, TcpForwardHelper.Instance.Viewmodel.CurrentIndex);

            TcpForwardHelper.Instance.Viewmodel.CurrentIndex = -1;
        }
        private void CancelEdit(object sender, RoutedEventArgs e)
        {
            TcpForwardHelper.Instance.Viewmodel.CurrentIndex = -1;
        }

        private void dataGridMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TcpForwardHelper.Instance.Viewmodel.CurrentIndex = dataGrid.SelectedIndex;

            var item = TcpForwardHelper.Instance.Viewmodel.Mappings[dataGrid.SelectedIndex];

            TcpForwardHelper.Instance.Viewmodel.SourceIp = item.SourceIp;
            TcpForwardHelper.Instance.Viewmodel.SourcePort = item.SourcePort;
            TcpForwardHelper.Instance.Viewmodel.TargetName = item.TargetName;
            TcpForwardHelper.Instance.Viewmodel.TargetPort = item.TargetPort;
            TcpForwardHelper.Instance.Viewmodel.AliveType = item.AliveType;
            TcpForwardHelper.Instance.Viewmodel.TargetIp = item.TargetIp;
        }
    }
}
