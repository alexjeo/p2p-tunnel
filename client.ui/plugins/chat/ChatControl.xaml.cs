using client.ui.plugins.clients;
using client.ui.viewModel;
using common;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace client.ui.plugins.chat
{
    /// <summary>
    /// ChatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChatControl : UserControl
    {
        private ClientMessageCacheModel currentCache = null;
        private readonly ConcurrentDictionary<long, ClientMessageCacheModel> msgs = new();
        private ClientInfo Client => AppShareData.Instance.CurrentClientInfo;
        private P2PChatViewModel viewModel;
        private readonly ConcurrentDictionary<string, FileStream> files = new();
        private string filePath = "./chat_files";
        private ContextMenu fileContextMenu;

        public ChatControl()
        {
            InitializeComponent();

            viewModel = new P2PChatViewModel();
            DataContext = viewModel;

            ClientsEventHandles.Instance.OnClientRemoveHandler += (sender, id) =>
            {
                _ = msgs.TryRemove(id, out _);
            };
            ClientsEventHandles.Instance.OnCurrentClientChangeHandler += (sender, client) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (currentCache != null)
                    {
                        currentCache.UI = new UIElement[textExchangePanel.Children.Count];
                        textExchangePanel.Children.CopyTo(currentCache.UI, 0);
                    }
                    textExchangePanel.Children.Clear();

                });
                if (!msgs.TryGetValue(client.Id, out ClientMessageCacheModel model))
                {
                    model = new ClientMessageCacheModel
                    {
                        Queue = new ConcurrentQueue<MessageModel>(),
                    };
                    _ = msgs.TryAdd(client.Id, model);
                }
                currentCache = model;
                if (currentCache.UI != null)
                {
                    for (int i = 0; i < currentCache.UI.Length; i++)
                    {
                        textExchangePanel.Children.Add(currentCache.UI[i]);
                    }
                    currentCache.UI = new UIElement[0];
                }
            };

            P2PChatEventHandles.Instance.OnTcpChatTextMessageHandler += OnTcpChatTextMessageHandler;
            P2PChatEventHandles.Instance.OnTcpChatFileMessageHandler += OnTcpChatFileMessageHandler;

            fileContextMenu = FindResource("fileContextMenu") as ContextMenu;
            _ = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (currentCache != null && currentCache.Queue.Count > 0)
                    {
                        MessageModel msg = DequeueMsg();
                        if (msg != null)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                if (textExchangePanel.Children.Count >= 100)
                                {
                                    textExchangePanel.Children.RemoveAt(0);
                                }

                                switch (msg.Type)
                                {
                                    case MessageType.SYSTEM:
                                        _ = textExchangePanel.Children.Add(GetTextExchangeSystem(msg.Content));
                                        break;
                                    case MessageType.SEND:
                                        _ = textExchangePanel.Children.Add(GetTextExchangeSend(msg.Content));
                                        break;
                                    case MessageType.RECEIVE:
                                        _ = textExchangePanel.Children.Add(GetTextExchangeSendReceive(msg.Content));
                                        break;
                                    case MessageType.SEND_FILE:
                                        _ = textExchangePanel.Children.Add(GetTextExchangeSendFile(msg.Content, msg.FilePath));
                                        break;
                                    case MessageType.RECEIVE_FILE:
                                        _ = textExchangePanel.Children.Add(GetTextExchangeSendReceiveFile(msg.Content, msg.FilePath));
                                        break;
                                    default:
                                        break;
                                }
                                textExchangeScrollBar.ScrollToEnd();
                            });
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void OnTcpChatFileMessageHandler(object sender, P2PChatFileReceiveModel e)
        {
            if (e.Data.Length == 0)
            {
                _ = EnqueueMsg(new MessageModel
                {
                    Type = MessageType.RECEIVE_FILE,
                    Content = $"{e.Name} ({Helper.FileSizeFormat(e.Size)})"
                }, e.FromId);
            }
            else
            {
                //保存路径
                string path = Path.Combine(filePath, e.FromId.ToString());
                if (!Directory.Exists(path))
                {
                    _ = Directory.CreateDirectory(path);
                }
                string fullPath = Path.Combine(path, e.Name);
                //因为分包，每次消息只有文件的一部分，需要缓存
                _ = files.TryGetValue(e.Md5, out FileStream fs);
                if (fs == null)
                {
                    //删除旧文件
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                    //界面显示
                    _ = EnqueueMsg(new MessageModel
                    {
                        Type = MessageType.RECEIVE_FILE,
                        Content = $"{e.Name} ({Helper.FileSizeFormat(e.Size)})",
                        FilePath = fullPath
                    }, e.FromId);
                    //创建或者追加
                    fs = new FileStream(fullPath, FileMode.Create & FileMode.Append, FileAccess.Write);
                    _ = files.TryAdd(e.Md5, fs);
                }
                fs.Write(e.Data);

                if (fs.Length >= e.Size)
                {
                    _ = files.TryRemove(e.Md5, out _);
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTcpChatTextMessageHandler(object sender, P2PChatTextReceiveModel e)
        {
            _ = EnqueueMsg(new MessageModel { Type = MessageType.RECEIVE, Content = e.Text }, e.Id);
        }

        /// <summary>
        /// 点击发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendTextButton_Click(object sender, RoutedEventArgs e)
        {
            string text = sendMessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (EnqueueMsg(new MessageModel { Type = MessageType.SEND, Content = text }, Client.Id))
                {
                    if (Client != null && Client.Socket != null)
                    {
                        P2PChatEventHandles.Instance.SendTcpChatTextMessage(new SendTcpChatMessageEventArg<P2PChatTextModel>
                        {
                            Socket = Client.Socket,
                            ToId = Client.Id,

                            Data = new P2PChatTextModel
                            {
                                Text = text
                            }
                        });
                    }
                    sendMessageTextBox.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 服务器消息模板
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private TextExchangeSystem GetTextExchangeSystem(string content)
        {
            TextExchangeSystem receive = new();
            receive.content.Text = content;
            return receive;
        }
        /// <summary>
        /// 回应消息模板
        /// </summary>
        /// <returns></returns>
        private TextExchangeReceive GetTextExchangeSendReceive(string text)
        {
            TextExchangeReceive receive = new();
            receive.time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            receive.content.Text = text;
            return receive;
        }
        /// <summary>
        /// 发送消息模板
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private TextExchangeSend GetTextExchangeSend(string content)
        {
            TextExchangeSend receive = new();
            receive.time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            receive.content.Text = content;
            return receive;
        }
        /// <summary>
        /// 发送消息模板
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private TextExchangeSendFile GetTextExchangeSendFile(string content, string path = "")
        {
            TextExchangeSendFile receive = new();
            receive.time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            receive.content.Text = content;
            receive.border.ContextMenu = fileContextMenu;
            receive.border.FilePath = path;
            return receive;
        }
        private TextExchangeReceiveFile GetTextExchangeSendReceiveFile(string text, string path = "")
        {
            TextExchangeReceiveFile receive = new();
            receive.time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            receive.content.Text = text;
            receive.border.ContextMenu = fileContextMenu;
            receive.border.FilePath = path;
            return receive;
        }


        private void ClearClick(object sender, RoutedEventArgs e)
        {
            textExchangePanel.Children.Clear();
        }

        private bool EnqueueMsg(MessageModel msg, long id)
        {
            if (id == 0)
            {
                return false;
            }

            if (!msgs.TryGetValue(id, out ClientMessageCacheModel model))
            {
                model = new ClientMessageCacheModel
                {
                    Queue = new ConcurrentQueue<MessageModel>(),
                };
                _ = msgs.TryAdd(id, model);
            }
            model.Queue.Enqueue(msg);
            if (id != Client.Id && model.Queue.Count >= 100)
            {
                _ = model.Queue.TryDequeue(out _);
            }

            return true;
        }

        private MessageModel DequeueMsg()
        {
            if (currentCache == null)
            {
                return null;
            }

            _ = currentCache.Queue.TryDequeue(out MessageModel msg);

            return msg;
        }

        private void ChoiceFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            if (dialog.ShowDialog() == true)
            {
                viewModel.Sending = true;
                _ = Task.Run(() =>
                  {
                      FileInfo file = new(dialog.FileName);
                      P2PChatFileModel chatFileModel = new()
                      {
                          Name = Path.GetFileName(dialog.FileName),
                          Md5 = Helper.GetMd5Hash($"${AppShareData.Instance.ConnectId}{dialog.FileName}"),
                          Size = file.Length
                      };
                      if (string.IsNullOrWhiteSpace(chatFileModel.Md5))
                      {
                          _ = EnqueueMsg(new MessageModel
                          {
                              Type = MessageType.SYSTEM,
                              Content = $"{Path.GetFileName(dialog.FileName)} 文件读取失败"
                          }, Client.Id);
                      }
                      else
                      {
                          _ = EnqueueMsg(new MessageModel
                          {
                              Type = MessageType.SEND_FILE,
                              Content = $"{chatFileModel.Name} ({Helper.FileSizeFormat(chatFileModel.Size)})",
                              FilePath = dialog.FileName
                          }, Client.Id);
                          P2PChatEventHandles.Instance.SendTcpChatFileMessage(new SendTcpChatMessageEventArg<P2PChatFileModel>
                          {
                              ToId = Client.Id,
                              Socket = Client.Socket,
                              Data = chatFileModel
                          }, file);
                      }
                      viewModel.Sending = false;
                  });
            }
        }


        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            ContextMenu menu = item.Parent as ContextMenu;
            FileBorder border = menu.PlacementTarget as FileBorder;

            _ = Process.Start("explorer.exe", border.FilePath);
        }
        private void OpenDirClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            ContextMenu menu = item.Parent as ContextMenu;
            FileBorder border = menu.PlacementTarget as FileBorder;

            _ = Process.Start("explorer.exe",Path.GetDirectoryName(border.FilePath));
        }

    }

    public class FileBorder : Border
    {
        public string FilePath { get; set; }
    }

    public class ClientMessageCacheModel
    {
        public ConcurrentQueue<MessageModel> Queue { get; set; } = new ConcurrentQueue<MessageModel>();
        public UIElement[] UI { get; set; } = new UIElement[0];
    }

    public class MessageModel
    {
        public MessageType Type { get; set; } = MessageType.SYSTEM;
        public string Content { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    public enum MessageType
    {
        SYSTEM,
        SEND, RECEIVE,
        SEND_FILE, RECEIVE_FILE
    }
}
