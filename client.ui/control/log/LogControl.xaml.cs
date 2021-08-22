using client.ui.plugins.connectClient;
using common;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace client.ui.control.log
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        public LogControl()
        {
            InitializeComponent();


            //给服务器发送请求连接消息
            ConnectClientEventHandles.Instance.OnSendConnectClientMessageHandler += SendConnectClientMessageHandler;
            //收到连接请求
            ConnectClientEventHandles.Instance.OnConnectClientStep1Handler += OnConnectClientStep1Handler;
            //我已准备好
            ConnectClientEventHandles.Instance.OnConnectClientStep1ResultHandler += OnConnectClientStep1ResultHandler;
            //对方已准备好
            ConnectClientEventHandles.Instance.OnConnectClientStep2Handler += OnConnectClientStep2Handler;
            //连接对方
            ConnectClientEventHandles.Instance.OnSendConnectClientStep3Handler += OnSendConnectClientStep3Handler;
            //收到对方连接
            ConnectClientEventHandles.Instance.OnConnectClientStep3Handler += OnConnectClientStep3Handler;
            //回应对方
            ConnectClientEventHandles.Instance.OnSendConnectClientStep4Handler += OnSendConnectClientStep4Handler;
            //收到对方回应
            ConnectClientEventHandles.Instance.OnConnectClientStep4Handler += OnConnectClientStep4Handler;


            //给服务器发送请求连接消息
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientMessageHandler += SendTcpConnectClientMessageHandler;
            //收到B的信息 准备一下
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep1Handler += OnTcpConnectClientStep1Handler;
            //A已准备
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep1ResultHandler += OnTcpConnectClientStep1ResultHandler;
            //A已准备好 连接他
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep2Handler += OnTcpConnectClientStep2Handler;
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientStep2RetryHandler += OnSendTcpConnectClientStep2RetryHandler;
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep2RetryHandler += OnTcpConnectClientStep2RetryHandler; ;

            //链接失败
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep2FailHandler += OnTcpConnectClientStep2FailHandler;
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientStep2FailHandler += OnSendTcpConnectClientStep2FailHandler; ;
            //连接A
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientStep3Handler += OnSendTcpConnectClientStep3Handler;
            //收到B的连接
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep3Handler += OnTcpConnectClientStep3Handler;
            //回应B
            ConnectClientEventHandles.Instance.OnSendTcpConnectClientStep4Handler += OnSendTcpConnectClientStep4Handler;
            //收到A的回应
            ConnectClientEventHandles.Instance.OnTcpConnectClientStep4Handler += OnTcpConnectClientStep4Handler;


            _ = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (queue.Count > 0)
                    {
                        _ = queue.TryDequeue(out string msg);
                        if (msg != null)
                        {
                            try
                            {

                                Dispatcher.Invoke(() =>
                                {
                                    if (logsPanel.Children.Count > 100)
                                    {
                                        logsPanel.Children.RemoveAt(0);
                                    }
#if DEBUG
                                    Debug.WriteLine(msg);
#endif
#if RELEASE
                                LogInfoControl item = new LogInfoControl();
                                item.content.Text = msg;
                                _ = logsPanel.Children.Add(item);
                                textExchangeScrollBar.ScrollToEnd();
#endif
                                });
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }, TaskCreationOptions.LongRunning);

            Logger.Instance.OnLogger += (sender, model) =>
            {
                queue.Enqueue($"[{model.Type}][{model.Time.ToString("yyyy-MM-dd HH:mm:ss")}]:{model.Content}");
            };
        }

        private void ClearClick(object sender, System.Windows.RoutedEventArgs e)
        {
            logsPanel.Children.Clear();
        }

        #region UDP
        private void SendConnectClientMessageHandler(object sender, SendConnectClientEventArg e)
        {
            Logger.Instance.Info($"自己:向服务器要求连接[{e.Id}]【{e.Name}】");
        }

        private void OnConnectClientStep1Handler(object sender, OnConnectClientStep1EventArg e)
        {
            Logger.Instance.Info($"服务器:[{e.Data.Id}]【{e.Data.Name}】要连接你");
        }
        private void OnConnectClientStep1ResultHandler(object sender, OnConnectClientStep1ResultEventArg e)
        {
            Logger.Instance.Info($"自己:我已准备好连接");
        }

        private void OnConnectClientStep2Handler(object sender, OnConnectClientStep2EventArg e)
        {
            Logger.Instance.Info($"服务器:[{e.Data.Id}]【{e.Data.Name}】已准备好");
        }

        private void OnSendConnectClientStep3Handler(object sender, SendConnectClientStep3EventArg e)
        {
            Logger.Instance.Info($"自己:连接[{e.Id}]");
        }
        private void OnConnectClientStep3Handler(object sender, OnConnectClientStep3EventArg e)
        {
            Logger.Instance.Info($"{e.Data.Id}:我连接你了");
        }

        private void OnSendConnectClientStep4Handler(object sender, SendConnectClientStep4EventArg e)
        {
            Logger.Instance.Info($"自己:回应[{e.Id}]");
        }
        private void OnConnectClientStep4Handler(object sender, OnConnectClientStep4EventArg e)
        {
            Logger.Instance.Info($"{e.Data.Id}:我回应你了");
        }
        #endregion

        #region TCP

        private void SendTcpConnectClientMessageHandler(object sender, SendConnectClientEventArg e)
        {
            Logger.Instance.Info($"TCP:自己:向服务器要求连接[{e.Id}]【{e.Name}】");
        }
        private void OnTcpConnectClientStep1Handler(object sender, OnConnectClientStep1EventArg e)
        {
            Logger.Instance.Info($"TCP:自己:已找到[{e.Data.Id}]【{e.Data.Name}】:{e.Data.TcpPort}");
        }
        private void OnTcpConnectClientStep1ResultHandler(object sender, OnConnectClientStep1ResultEventArg e)
        {
            Logger.Instance.Info($"TCP:自己:我已准备好连接");
        }

        private void OnTcpConnectClientStep2Handler(object sender, OnConnectClientStep2EventArg e)
        {
            Logger.Instance.Info($"TCP:服务器:[{e.Data.Id}]【{e.Data.Name}】:{e.Data.TcpPort} 已准备好");
        }

        private void OnTcpConnectClientStep2RetryHandler(object sender, OnConnectClientStep2RetryEventArg e)
        {
            Logger.Instance.Info($"TCP:服务器:[{e.Data.Id}] 链接失败，正在尝试");
        }
        private void OnSendTcpConnectClientStep2RetryHandler(object sender, long e)
        {
            Logger.Instance.Info($"TCP:自己:[{e}] 链接失败，正在尝试");
        }


        private void OnSendTcpConnectClientStep2FailHandler(object sender, OnSendTcpConnectClientStep2FailEventArg e)
        {
            Logger.Instance.Info($"TCP:自己:[{e.ToId}] 链接失败，可以再试一次");
        }
        private void OnTcpConnectClientStep2FailHandler(object sender, OnTcpConnectClientStep2FailEventArg e)
        {
            Logger.Instance.Info($"TCP:服务器:[{e.Data.Id}] 链接失败，可以再试一次");
        }

        private void OnSendTcpConnectClientStep3Handler(object sender, SendTcpConnectClientStep3EventArg e)
        {
            Logger.Instance.Info($"TCP:自己:连接[{e.Id}]");
        }
        private void OnTcpConnectClientStep3Handler(object sender, OnConnectClientStep3EventArg e)
        {
            Logger.Instance.Info($"TCP:{e.Data.Id}:我连接你了");
        }

        private void OnSendTcpConnectClientStep4Handler(object sender, SendTcpConnectClientStep4EventArg e)
        {
            Logger.Instance.Info($"TCP:自己:回应[{e.Id}]");
        }
        private void OnTcpConnectClientStep4Handler(object sender, OnConnectClientStep4EventArg e)
        {
            Logger.Instance.Info($"TCP:{e.Data.Id}:我回应你了");
        }
        #endregion
    }
}
