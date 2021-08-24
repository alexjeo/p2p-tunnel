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
                if (callback != null)
                {
                    string viewerConnString = axRDPViewer.StartReverseConnectListener(connectString, userName, password);
                    callback.Invoke(viewerConnString);
                }
                else
                {
                    axRDPViewer.Connect(connectString, userName, password);
                    axRDPViewer.OnConnectionFailed += (sender, e) =>
                    {
                        _ = MessageBox.Show("连接失败");
                    };
                }
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
