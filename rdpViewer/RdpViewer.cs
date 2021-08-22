using System;
using System.Windows.Forms;

namespace rdpViewer
{
    public partial class RdpViewer : Form
    {
        public RdpViewer(string connectString, string userName, string password, Action<string> callback)
        {
            InitializeComponent();
            TopLevel = false;
            AutoSize = true;
            axRDPViewer.SmartSizing = true;
            try
            {
                //string viewerConnString = axRDPViewer.StartReverseConnectListener(connectString, userName, password);
                axRDPViewer.Connect(connectString, userName, password);
                //callback(viewerConnString);
                axRDPViewer.OnConnectionFailed += (sender, e) =>
                {
                    _ = MessageBox.Show("连接失败");
                };
                //axRDPViewer.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            WindowState = FormWindowState.Maximized;
            SetVisibleCore(true);

        }

        public void Disponse()
        {
            axRDPViewer.Dispose();
        }

        public void SetSize(int width, int height)
        {
            axRDPViewer.Width = width;
            axRDPViewer.Height = height;
        }
    }
}
