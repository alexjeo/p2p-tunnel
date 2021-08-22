using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace client.ui.windows.wakeup
{
    public class WakeUpViewModel : ViewModelBase
    {
        private ObservableCollection<string> ips = new ObservableCollection<string>();
        public ObservableCollection<string> Ips
        {
            get => ips;
            set
            {
                ips = value;
                RaisePropertyChanged(() => Ips);
            }
        }


        private string ip = string.Empty;
        public string Ip
        {
            get => ip;
            set
            {
                ip = value;

                RaisePropertyChanged(() => Ip);
            }
        }

        private string mac = string.Empty;
        public string Mac
        {
            get => mac;
            set
            {
                mac = value;

                RaisePropertyChanged(() => Mac);
            }
        }

        private int port = 8099;
        public int Port
        {
            get => port;
            set
            {
                port = value;

                RaisePropertyChanged(() => Port);
            }
        }

    }
}
