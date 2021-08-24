using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace mstsc.manager
{
    public class RemoteDesktopViewModel : ViewModelBase, IDataErrorInfo
    {

        private ObservableCollection<RemoteDesktopInfo> desktops = new ObservableCollection<RemoteDesktopInfo>();
        public ObservableCollection<RemoteDesktopInfo> Desktops
        {
            get => desktops;
            set
            {
                desktops = value;
                RaisePropertyChanged(() => Desktops);
            }
        }

#if DEBUG
        private string ip = "127.0.0.1";
#endif
#if RELEASE
        private string ip = string.Empty;
#endif
        [Required(ErrorMessage = "服务器地址必填")]
        public string Ip
        {
            get => ip;
            set
            {
                ip = value;
                RaisePropertyChanged(() => Ip);
            }
        }

        private int port = 3389;
        [Required(ErrorMessage = "服务器端口必填")]
        public int Port
        {
            get => port;
            set
            {
                port = value;
                RaisePropertyChanged(() => Port);
            }
        }

        private string userName = "administrator";
        [Required(ErrorMessage = "登录名必填")]
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

#if DEBUG
        private string password = "123456";
#endif
#if RELEASE
        private string password = string.Empty;
#endif
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private string desc = "default";
        public string Desc
        {
            get => desc;
            set
            {
                desc = value;
                RaisePropertyChanged(() => Desc);
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

    public class RemoteDesktopInfo
    {
        public string Ip { get; set; } = string.Empty;

        public int Port { get; set; } = 3389;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Desc { get; set; } = string.Empty;
    }

    internal class PasseordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Regex.Replace(value.ToString(), @"[\s\S]*.", "*");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Regex.Replace(value.ToString(), @"[\s\S]*.", "*");
        }
    }
}
