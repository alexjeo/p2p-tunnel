using client.ui.plugins.register;
using common;
using common.extends;
using ProtoBuf;
using RDPCOMAPILib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace client.ui.plugins.rdpDesktop
{
    public class RdpSahreHelper
    {
        private static readonly Lazy<RdpSahreHelper> lazy = new Lazy<RdpSahreHelper>(() => new RdpSahreHelper());
        public static RdpSahreHelper Instance => lazy.Value;

        private RDPSession rdpSession;
        public string ConnectString { get; private set; } = string.Empty;
        private readonly string configFileName = "config_rdpshare.bin";

        public RdpShareSettingViewModel ViewModel { get; private set; }

        public event EventHandler<bool> OnConnectdChange;

        private bool connecting = false;

        private RdpSahreHelper()
        {
            ViewModel = new RdpShareSettingViewModel();
            ReadConfig();
        }

        public void Start()
        {
            if (ViewModel.Connected)
            {
                Stop();
            }
            else
            {
                if (connecting)
                {
                    return;
                }

                _ = Task.Run(() =>
                  {
                      try
                      {
                          rdpSession = new RDPSession();
                          rdpSession.Open();
                          rdpSession.OnAttendeeConnected += (e) =>
                          {
                              //Logger.Instance.Info("收到连接");
                              IRDPSRAPIAttendee att = e as IRDPSRAPIAttendee;
                              att.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
                          };

                          IRDPSRAPIInvitation invitation = rdpSession.Invitations.CreateInvitation(null, "default", ViewModel.Password, ViewModel.AttendeeLimit);

                          ConnectString = invitation.ConnectionString;

                          ViewModel.Connected = true;
                          OnConnectdChange?.Invoke(this, true);
                          SaveConfig();
                      }
                      catch (Exception ex)
                      {
                          Logger.Instance.Error("开启远程桌面失败:" + ex);
                          connecting = false;
                      }
                  });
            }
        }

        public void Stop()
        {
            if (rdpSession != null)
            {
                rdpSession.Close();
            }
            connecting = false;
            ViewModel.Connected = false;
            OnConnectdChange?.Invoke(this, false);
            ConnectString = string.Empty;
        }

        public void ConnectClient(string connectString)
        {
            if (rdpSession != null)
            {
                try
                {
                    //Logger.Instance.Info($"收到客户端连接串：{connectString}");
                    rdpSession.ConnectToClient(connectString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ReadConfig()
        {
            ConfigFileModel config = configFileName.ProtobufDeserializeFileRead<ConfigFileModel>();
            if (config != null)
            {
                ViewModel.AttendeeLimit = config.AttendeeLimit;
                ViewModel.Password = config.Password;
            }
        }

        private void SaveConfig()
        {
            _ = new ConfigFileModel
            {
                AttendeeLimit = ViewModel.AttendeeLimit,
                Password = ViewModel.Password,
            }.ProtobufSerializeFileSave(configFileName);
        }


    }

    public class ConfigFileModel : IFileConfig
    {
        public ConfigFileModel() { }
        public ushort AttendeeLimit { get; set; }
        public string Password { get; set; }
    }
}
