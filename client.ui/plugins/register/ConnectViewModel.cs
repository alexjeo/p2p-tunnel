using client.ui.validation;
using client.ui.viewModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;

namespace client.ui.plugins.register
{
    public class ConnectViewModel : ViewModelBase, IDataErrorInfo
    {
        private string serverIp = AppShareData.Instance.ServerIp;
        [Required(ErrorMessage = "服务器地址必填")]
        //[CustomValidation(typeof(TextBoxValidationExtendRules),"Required")]
        public string ServerIp
        {
            get => serverIp;
            set
            {
                AppShareData.Instance.ServerIp = serverIp = value;
                RaisePropertyChanged(() => ServerIp);
            }
        }

        private int serverPort = AppShareData.Instance.ServerPort;
        [Required(ErrorMessage = "服务器端口必填")]
        public int ServerPort
        {
            get => serverPort;
            set
            {
                AppShareData.Instance.ServerPort = serverPort = value;
                RaisePropertyChanged(() => ServerPort);
            }
        }

        private int serverTcpPort = AppShareData.Instance.ServerTcpPort;
        [Required(ErrorMessage = "服务器Tcp端口必填")]
        public int ServerTcpPort
        {
            get => serverTcpPort;
            set
            {
                AppShareData.Instance.ServerTcpPort = serverTcpPort = value;
                RaisePropertyChanged(() => ServerTcpPort);
            }
        }


        private int clientPort = AppShareData.Instance.ClientPort;
        [Required(ErrorMessage = "本地端口必填")]
        public int ClientPort
        {
            get => clientPort;
            set
            {
                AppShareData.Instance.ClientPort = clientPort = value;
                RaisePropertyChanged(() => ClientPort);
            }
        }

        private int clientTcpPort = AppShareData.Instance.ClientTcpPort;
        [Required(ErrorMessage = "本地Tcp端口必填")]
        public int ClientTcpPort
        {
            get => clientTcpPort;
            set
            {
                AppShareData.Instance.ClientTcpPort = clientTcpPort = value;
                RaisePropertyChanged(() => ClientTcpPort);
            }
        }

        private bool isPortGeting = false;
        public bool IsPortGeting
        {
            get => isPortGeting;
            set
            {
                isPortGeting = value;
                RaisePropertyChanged(() => IsPortGeting);
            }
        }

        private string clientName = AppShareData.Instance.ClientName;
        [Required(ErrorMessage = "显示名称必填")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "名字1-20个字符")]
        public string ClientName
        {
            get => clientName;
            set
            {
                AppShareData.Instance.ClientName = clientName = value;
                RaisePropertyChanged(() => ClientName);
            }
        }

        private bool isConnecting = AppShareData.Instance.IsConnecting;
        public bool IsConnecting
        {
            get => isConnecting;
            set
            {
                AppShareData.Instance.IsConnecting = isConnecting = value;
                RaisePropertyChanged(() => IsConnecting);
            }
        }

        private long connectId = AppShareData.Instance.ConnectId;
        public long ConnectId
        {
            get => connectId;
            set
            {
                AppShareData.Instance.ConnectId = connectId = value;
                if (value == 0)
                {
                    ConnectIdStr = string.Empty;
                }
                else
                {
                    ConnectIdStr = value.ToString();
                }
                RaisePropertyChanged(() => ConnectId);
            }
        }

        private string connectIdStr = string.Empty;
        public string ConnectIdStr
        {
            get => connectIdStr;
            set
            {
                connectIdStr = value;
                RaisePropertyChanged(() => ConnectIdStr);
            }
        }

        private bool connected = AppShareData.Instance.Connected;
        public bool Connected
        {
            get => connected;
            set
            {
                AppShareData.Instance.Connected = connected = value;
                RaisePropertyChanged(() => Connected);
            }
        }

        private bool tcpConnected = AppShareData.Instance.TcpConnected;
        public bool TcpConnected
        {
            get => tcpConnected;
            set
            {
                AppShareData.Instance.TcpConnected = tcpConnected = value;
                RaisePropertyChanged(() => TcpConnected);
            }
        }

        private string groupId = string.Empty;
        public string GroupId
        {
            get => groupId;
            set
            {
                groupId = value;
                RaisePropertyChanged(() => GroupId);
            }
        }


        public string Error { get; set; } = string.Empty;

        public string this[string columnName]
        {
            get
            {
                ValidationContext vc = new ValidationContext(this, null, null)
                {
                    MemberName = columnName
                };

                List<ValidationResult> res = new List<ValidationResult>();

                bool result = Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this, null), vc, res);

                Error = res.Count > 0 ? string.Join(Environment.NewLine, res.Select(c => c.ErrorMessage).ToArray()) : string.Empty;

                return Error;
            }
        }
        public string GetErrorMsg()
        {
            var names = GetType().GetProperties().Where(c => c.GetCustomAttributes(typeof(ValidationAttribute), false).Length > 0).Select(c => c.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                string msg = this[names[i]];
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    return msg;
                }
            }
            return string.Empty;
        }

    }
}
