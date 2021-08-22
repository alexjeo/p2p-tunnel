//using RDPCOMAPILib;
using common.extends;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace client.ui.plugins.remoteDesktop
{
    public class RemoteDesktopHelper
    {
        private static readonly Lazy<RemoteDesktopHelper> lazy = new Lazy<RemoteDesktopHelper>(() => new RemoteDesktopHelper());
        public static RemoteDesktopHelper Instance
        {
            get { return lazy.Value; }
            private set { }

        }

        public RemoteDesktopViewModel viewModel = null;
        private readonly string configFileName = "config_remove_desktop.bin";
        public event EventHandler<RemoteDesktopInfo> OnOpen;

        public RemoteDesktopHelper()
        {
            viewModel = new RemoteDesktopViewModel();
            ReadConfig();
        }


        public void Add(RemoteDesktopInfo model)
        {
            viewModel.Desktops.Add(model);
            SaveConfig();
        }
        public void Del(int index)
        {
            try
            {
                viewModel.Desktops.RemoveAt(index);
                SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message);
            }
        }

        public void Open(RemoteDesktopInfo model)
        {
            OnOpen?.Invoke(this, model);
        }

        private void ReadConfig()
        {
            ConfigFileModel config = configFileName.ProtobufDeserializeFileRead<ConfigFileModel>();
            if (config != null)
            {
                viewModel.Desktops = new ObservableCollection<RemoteDesktopInfo>(config.Desktops);
            }
        }

        private void SaveConfig()
        {
            new ConfigFileModel
            {
                Desktops = viewModel.Desktops.ToList()
            }.ProtobufSerializeFileSave(configFileName);
        }

        public void Disponse()
        {
            Instance = null;
            viewModel.Desktops.Clear();
            viewModel.Cleanup();
            viewModel = null;
        }
    }

    public class ConfigFileModel : IFileConfig
    {
        public ConfigFileModel() { }

        public List<RemoteDesktopInfo> Desktops { get; set; } = new List<RemoteDesktopInfo>();
    }
}
