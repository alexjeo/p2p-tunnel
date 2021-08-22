using client.ui.viewModel;
using common;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.chat
{
    public class P2PChatViewModel : ViewModelBase
    {
        private bool sending = false;
        public bool Sending
        {
            get => sending;
            set
            {
                sending = value;
                RaisePropertyChanged(() => Sending);
            }
        }
    }
}
