using client.ui.viewModel;
using common;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.main.viewModel
{
    public class MainWindowMenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenusModel> menus = new ObservableCollection<MenusModel> {
             new MenusModel{
                Id =  MenuTypeModel.REGISTER,
                Name="注册",
                HasStatus = true,
                Status = false
             },
             new MenusModel{
                Id =  MenuTypeModel.UPNP,
                Name="UPNP"
             },
             new MenusModel{
                Id =  MenuTypeModel.TCP_FORWARD,
                Name="TCP转发",
                HasStatus = true,
                Status = false
             },
             new MenusModel{
                Id =  MenuTypeModel.REMOTE_DESKTOP,
                Name="远程桌面",
                HasStatus = false,
                Status = false
             },
             new MenusModel{
                Id =  MenuTypeModel.SHARE_DESKTOP,
                Name="桌面共享",
                HasStatus = true,
                Status = false
             },
             new MenusModel{
                Id =  MenuTypeModel.WAKE_UP,
                Name="远程唤醒",
                HasStatus = false,
                Status = false
             },
        };
        public ObservableCollection<MenusModel> Menus
        {
            get => menus;
            set
            {
                menus = value;
                RaisePropertyChanged(() => Menus);
            }
        }
    }
    public enum MenuTypeModel
    {
        UPNP,
        TCP_FORWARD,
        REGISTER, REMOTE_DESKTOP, SHARE_DESKTOP, WAKE_UP
    }

    public class MenusModel : ViewModelBase
    {
        public MenuTypeModel Id { get; set; } = MenuTypeModel.UPNP;
        public string Name { get; set; } = string.Empty;
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

        public List<MenusModel> Children { get; set; } = new List<MenusModel>();
    }
}
