using client.ui.plugins.clients;
using client.ui.viewModel;
using GalaSoft.MvvmLight;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Data;

namespace client.ui.plugins.forward.tcp
{
    public class TcpForwardViewModel : ViewModelBase
    {
        private ObservableCollection<TcpForwardRecordBaseModel> mappings = new ObservableCollection<TcpForwardRecordBaseModel>();
        public ObservableCollection<TcpForwardRecordBaseModel> Mappings
        {
            get => mappings;
            set
            {
                mappings = value;
                RaisePropertyChanged(() => Mappings);
            }
        }

        private int currentIndex = -1;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                RaisePropertyChanged(() => CurrentIndex);
            }
        }


        private bool connected = false;
        public bool Connected
        {
            get => connected;
            set
            {
                connected = value;
                RaisePropertyChanged(() => Connected);
            }
        }


        private string sourceIp = "0.0.0.0";
        [Required(ErrorMessage = "源IP必填")]
        public string SourceIp
        {
            get => sourceIp;
            set
            {
                sourceIp = value;
                RaisePropertyChanged(() => SourceIp);
            }
        }

        private int sourcePort = 8080;
        [Required(ErrorMessage = "源端口必填")]
        public int SourcePort
        {
            get => sourcePort;
            set
            {
                sourcePort = value;
                RaisePropertyChanged(() => SourcePort);
            }
        }

        private string targetName = string.Empty;
        //[Required(ErrorMessage = "目标对象必选")]
        public string TargetName
        {
            get => targetName;
            set
            {
                targetName = value;
                RaisePropertyChanged(() => TargetName);
            }
        }


        private string targetIp = "127.0.0.1";
        [Required(ErrorMessage = "目标地址必填")]
        public string TargetIp
        {
            get => targetIp;
            set
            {
                targetIp = value;
                RaisePropertyChanged(() => TargetIp);
            }
        }

        private int targetPort = 8080;
        [Required(ErrorMessage = "目标端口必填")]
        public int TargetPort
        {
            get => targetPort;
            set
            {
                targetPort = value;
                RaisePropertyChanged(() => TargetPort);
            }
        }

        private TcpForwardAliveTypes aliveType = TcpForwardAliveTypes.UNALIVE;
        public TcpForwardAliveTypes AliveType
        {
            get => aliveType;
            set
            {
                aliveType = value;
                RaisePropertyChanged(() => AliveType);
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

    public class TcpForwardRecordBaseModel
    {
        public string SourceIp { get; set; } = "0.0.0.0";
        public int SourcePort { get; set; } = 8080;
        public string TargetName { get; set; } = string.Empty;
        public string TargetIp { get; set; } = "127.0.0.1";
        public int TargetPort { get; set; } = 8080;
        public bool Listening { get; set; } = false;
        public TcpForwardAliveTypes AliveType { get; set; } = TcpForwardAliveTypes.UNALIVE;
    }

    public class TcpForwardRecordModel : TcpForwardRecordBaseModel
    {
        public ClientInfo Client { get; set; }
    }


    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum TcpForwardAliveTypes : int
    {
        //长连接
        ALIVE,
        //短连接
        UNALIVE
    }


    public class TcpForwardTypeBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TcpForwardAliveTypes s = (TcpForwardAliveTypes)value;
            return s == (TcpForwardAliveTypes)int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (!isChecked)
            {
                return null;
            }
            return (TcpForwardAliveTypes)int.Parse(parameter.ToString());
        }
    }
}
