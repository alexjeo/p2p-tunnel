using client.ui.viewModel;
using common;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.main.viewModel
{
    public class MainWindowNotifyIconViewModel : ViewModelBase
    {
        private ObservableCollection<NotifyIconMenusModel> menus = new ObservableCollection<NotifyIconMenusModel> {
            new NotifyIconMenusModel{
                Id =  NotifyIconMenuTypeModel.REGISTER,
                Name="注册",
                HasStatus = true,
                Status = false
             },
            new NotifyIconMenusModel{
                Id =  NotifyIconMenuTypeModel.TCP_FORWARD,
                Name="TCP无连接转发",
                HasStatus = true,
                Status = false
             },
             new NotifyIconMenusModel{
                Id =  NotifyIconMenuTypeModel.RDP_SHARE_DESKTOP,
                Name="桌面共享",
                HasStatus = true,
                Status = false
             },
             new NotifyIconMenusModel{
                Id =  NotifyIconMenuTypeModel.EXIT,
                Name="退出",
                HasStatus = false,
                Status = false
             },
        };
        public ObservableCollection<NotifyIconMenusModel> Menus
        {
            get => menus;
            set
            {
                menus = value;
                RaisePropertyChanged(() => Menus);
            }
        }

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

    }
    public enum NotifyIconMenuTypeModel
    {
        REGISTER,
        TCP_FORWARD,
        RDP_SHARE_DESKTOP,
        EXIT
    }

    public class NotifyIconMenusModel : ViewModelBase
    {
        public NotifyIconMenuTypeModel Id { get; set; } = NotifyIconMenuTypeModel.EXIT;
        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        public bool HasStatus { get; set; } = false;
        private bool status = false;
        public bool Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public ObservableCollection<NotifyIconMenusModel> menus = new ObservableCollection<NotifyIconMenusModel>();
        public ObservableCollection<NotifyIconMenusModel> Menus
        {
            get => menus;
            set
            {
                menus = value;
                RaisePropertyChanged(() => Menus);
            }
        }
    }
}
