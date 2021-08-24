using rdpViewer;
using System;
using System.Net;
using System.Windows;
using System.Windows.Interop;

namespace rdp.desktop
{
    /// <summary>
    /// RdpShareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RdpShareWindow : Window
    {
        private RdpViewer viewer;

        public RdpShareWindow(string connectString, string password, Action<string> callback = null)
        {
            InitializeComponent();

            Title = $"正在连接桌面共享";

            try
            {
                viewer = new RdpViewer(connectString, Dns.GetHostName(), password, callback);
                wfh.Child = viewer;
            }
            catch (Exception)
            {
                _ = HandyControl.Controls.MessageBox.Error($"桌面共享连接失败");
            }
            wfh.Height = Height;
            ContentRendered += (sender, e) =>
            {
                SetSize();
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (PresentationSource.FromVisual(this) is HwndSource source)
            {
                source.AddHook(new HwndSourceHook(WinProc));
            }
        }

        private double lastHeight = 0;
        private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;
            switch (msg)
            {
                // 处理窗口消息
                case 0x0232:
                    {
                        SetSize();
                        break;
                    }
            }

            return result;
        }

        private void SetSize()
        {
            // 上下拖拉窗口
            if (this.Height != lastHeight)
            {
                this.Width = this.Height * 1.777;
            }
            else // 左右拖拉窗口
            {
                this.Height = (this.Width) / 1.777;
            }

            lastHeight = (int)this.Height;

            wfh.Height = this.Height - 35;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewer != null)
            {
                viewer.Dispose();
            }
        }
    }

}
