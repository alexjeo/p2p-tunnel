using client.ui.plugins.clients;
using client.ui.viewModel;
using common;
using rdpViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace client.ui.plugins.rdpDesktop
{
    /// <summary>
    /// RdpShareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RdpShareWindow : Window
    {
        private string ClientName => AppShareData.Instance.ClientName;
        private RdpViewer viewer;

        public RdpShareWindow(ClientInfo client)
        {
            InitializeComponent();

            Title = $"正在连接[{client.Name}]的桌面共享";

            RdpShareEventHandles.Instance.GetRdpShareInfo(client.Socket, (result) =>
            {
                if (string.IsNullOrWhiteSpace(result.ConnectString))
                {
                    HandyControl.Controls.MessageBox.Error("远程客户端未开启桌面共享!");
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        Title = $"[{client.Name}]的桌面共享";
                        try
                        {
                            //result.ConnectString = result.ConnectString.Replace("</T></C></E>", "<L P=\"39382\" N=\"127.0.0.1\"/></T></C></E>");
                            Logger.Instance.Info($"收到服务端连接串：{result.ConnectString}");
                            viewer = new RdpViewer(result.ConnectString, ClientName, result.Passeord, (connectString) =>
                            {
                                //Logger.Instance.Info($"发送客户端连接串：{connectString}");
                                RdpShareEventHandles.Instance.SendClientConnectString(client.Socket, connectString);
                            });
                            wfh.Child = viewer;
                        }
                        catch (Exception)
                        {
                            _ = HandyControl.Controls.MessageBox.Error($"[{client.Name}]的桌面共享连接失败");
                        }
                    });
                }
            }, (fail) =>
            {
                _ = HandyControl.Controls.MessageBox.Error(fail.Msg);
            });
            wfh.Height = this.Height;
            ContentRendered += (sender, e) =>
            {
                SetSize();
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (HwndSource.FromVisual(this) is HwndSource source)
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
                this.Width = this.Height * 1.777 + leftColumn.ActualWidth;
            }
            else // 左右拖拉窗口
            {
                this.Height = (this.Width - leftColumn.ActualWidth) / 1.777;
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
