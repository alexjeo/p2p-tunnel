using client.ui.viewModel;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace client.ui.plugins.register
{
    public class RdpShareSettingViewModel : ViewModelBase, IDataErrorInfo
    {
        private ushort attendeeLimit = 65535;
        [Required(ErrorMessage = "最大连接数必填"),RegularExpression(@"^[0-9]\d*$", ErrorMessage = "请填写有效数字"), Range(1, 65535, ErrorMessage = "最大连接数1-65535")]
        public ushort AttendeeLimit
        {
            get => attendeeLimit;
            set
            {
                attendeeLimit = value;
                RaisePropertyChanged(() => AttendeeLimit);
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private bool connected = AppShareData.Instance.Connected;
        public bool Connected
        {
            get => connected;
            set
            {
                connected = value;
                RaisePropertyChanged(() => Connected);
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
