using System;
using System.Collections.Concurrent;
using System.IO;

namespace client.service.serverPlugins.chat
{
    public class P2PChatHelper
    {
        private static readonly Lazy<P2PChatHelper> lazy = new(() => new P2PChatHelper());
        public static P2PChatHelper Instance => lazy.Value;

        private readonly ConcurrentDictionary<string, FileStream> files = new();
        private string filePath = "./chat_files";


        private P2PChatHelper()
        {
            P2PChatEventHandles.Instance.OnTcpChatTextMessageHandler += OnTcpChatTextMessageHandler;
            P2PChatEventHandles.Instance.OnTcpChatFileMessageHandler += OnTcpChatFileMessageHandler;
        }

        public void Start()
        {

        }

        private void OnTcpChatFileMessageHandler(object sender, P2PChatFileReceiveModel e)
        {
            if (e.Data.Length == 0)
            {

            }
            else
            {
                string path = Path.Combine(filePath, e.FromId.ToString());
                if (!Directory.Exists(path))
                {
                    _ = Directory.CreateDirectory(path);
                }
                string fullPath = Path.Combine(path, $"{e.Name}");

                _ = files.TryGetValue(e.Md5, out FileStream fs);
                if (fs == null)
                {
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
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
        private void OnTcpChatTextMessageHandler(object sender, P2PChatTextReceiveModel e)
        {

        }
    }
}
