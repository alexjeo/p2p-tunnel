using client.ui.viewModel;
using common;
using GalaSoft.MvvmLight;
using Mono.Nat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.main.viewModel
{
    public class UpnpViewModel : ViewModelBase, IDataErrorInfo
    {

        private ObservableCollection<Mapping> mappings = new ObservableCollection<Mapping>();
        public ObservableCollection<Mapping> Mappings
        {
            get => mappings;
            set
            {
                mappings = value;
                RaisePropertyChanged(() => Mappings);
            }
        }

        private ObservableCollection<DeviceModel> devices = new ObservableCollection<DeviceModel>();
        public ObservableCollection<DeviceModel> Devices
        {
            get => devices;
            set
            {
                devices = value;
                RaisePropertyChanged(() => Devices);
            }
        }

        private ObservableCollection<ProtocolModel> protocols = new ObservableCollection<ProtocolModel> {
             new ProtocolModel{ Protocol= Protocol.Tcp,Text="TCP" },
             new ProtocolModel{ Protocol= Protocol.Udp,Text="UDP" }
        };
        public ObservableCollection<ProtocolModel> Protocols
        {
            get => protocols;
            set
            {
                protocols = value;
                RaisePropertyChanged(() => Protocols);
            }
        }




        private int publicPort = 8099;
        [Required(ErrorMessage = "外网端口必填"), Range(1, 65535, ErrorMessage = "端口1-65535")]
        public int PublicPort
        {
            get => publicPort;
            set
            {
                publicPort = value;

                RaisePropertyChanged(() => PublicPort);
            }
        }

        private int privatePort = 8099;
        [Required(ErrorMessage = "内网端口必填"), Range(1, 65535, ErrorMessage = "端口1-65535")]
        public int PrivatePort
        {
            get => privatePort;
            set
            {
                privatePort = value;

                RaisePropertyChanged(() => PrivatePort);
            }
        }

        private Protocol protocol = Protocol.Tcp;
        [Required(ErrorMessage = "请选择协议类型")]
        public Protocol Protocol
        {
            get => protocol;
            set
            {
                protocol = value;

                RaisePropertyChanged(() => Protocol);
            }
        }

        private string description = string.Empty;
        [StringLength(20, MinimumLength = 0, ErrorMessage = "0-20个字符")]
        public string Description
        {
            get => description;
            set
            {
                description = value;

                RaisePropertyChanged(() => Description);
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

    public class DeviceModel
    {
        public INatDevice Device { get; set; } = null;
        public string Text { get; set; } = null;
    }

    public class ProtocolModel
    {
        public Protocol Protocol { get; set; } = Protocol.Tcp;
        public string Text { get; set; } = "TCP";
    }
}
