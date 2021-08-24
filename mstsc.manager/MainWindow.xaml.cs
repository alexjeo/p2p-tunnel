using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mstsc.manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<string, bool> desktops = new Dictionary<string, bool>();


        public MainWindow()
        {
            InitializeComponent();

            RemoteDesktopHelper.Instance.OnOpen += OnOpen;
        }
        ~MainWindow()
        {
            desktops.Clear();
        }

        private void OnOpen(object sender, RemoteDesktopInfo e)
        {
            string id = $"wfh{e.Ip.Replace(".", "")}";
            if (desktops.ContainsKey(id)) return;
            desktops.Add(id, true);

            var item = new HandyControl.Controls.TabItem
            {
                Header = e.Desc,

            };
            item.MouseDoubleClick += (sender, e) =>
            {
                HandyControl.Controls.TabItem item = (HandyControl.Controls.TabItem)e.Source;
                WindowsFormsHost wfh = item.Content as WindowsFormsHost;
                RdpControl rdp = wfh.Child as RdpControl;
                rdp.FullScreen = !rdp.FullScreen;
            };
            _ = tab.Items.Add(item);

            _ = Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {

                    WindowsFormsHost wfh = new WindowsFormsHost
                    {
                        Name = id
                    };
                    item.Content = wfh;
                    RdpControl rdp = new RdpControl();
                    ((System.ComponentModel.ISupportInitialize)(rdp)).BeginInit();
                    rdp.Enabled = true;
                    wfh.Child = rdp;
                    ((System.ComponentModel.ISupportInitialize)(rdp)).EndInit();
                    rdp.Server = e.Ip;
                    rdp.UserName = e.UserName;
                    rdp.AdvancedSettings9.ClearTextPassword = e.Password;
                    rdp.ColorDepth = 24;
                    rdp.AdvancedSettings9.SmartSizing = true;
                    rdp.AdvancedSettings9.AuthenticationLevel = 2;
                    rdp.AdvancedSettings9.EnableCredSspSupport = true;
                    rdp.AdvancedSettings9.PinConnectionBar = false;
                    rdp.AdvancedSettings9.ConnectionBarShowMinimizeButton = false;
                    float scal = GetWinScaling();
                    rdp.Width = 1920;
                    rdp.Width = 1080;
                    rdp.DesktopWidth = (int)(1920 * scal);
                    rdp.DesktopHeight = (int)(1080 * scal);
                    rdp.Connect();
                    rdp.OnDisconnected += (sender, e) =>
                    {
                        rdp.Dispose();
                    };
                    rdp.OnFatalError += (sender, e) =>
                    {
                        try
                        {
                            rdp.Disconnect();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        rdp.Dispose();
                    };
                });
            });
        }

        private void TabItemClosing(object sender, EventArgs e)
        {
            HandyControl.Data.CancelRoutedEventArgs arg = (HandyControl.Data.CancelRoutedEventArgs)e;
            HandyControl.Controls.TabItem item = (HandyControl.Controls.TabItem)arg.OriginalSource;
            WindowsFormsHost wfh = item.Content as WindowsFormsHost;
            RdpControl rdp = wfh.Child as RdpControl;
            try
            {
                rdp.Disconnect();
            }
            catch (Exception)
            {
            }
            rdp.Dispose();
            rdp = null;
            item.Content = null;
            try
            {
                _ = desktops.Remove(wfh.Name);
            }
            catch (Exception)
            {
            }
        }

        public float GetWinScaling()
        {
            Graphics currentGraphics = Graphics.FromHwnd(new WindowInteropHelper(this).Handle);
            return currentGraphics.DpiX / 96;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (object _item in tab.Items)
            {
                HandyControl.Controls.TabItem item = (HandyControl.Controls.TabItem)_item;
                if (item.Content is WindowsFormsHost wfh)
                {
                    RdpControl rdp = wfh.Child as RdpControl;
                    rdp.Disconnect();
                    rdp.Dispose();
                }
            }
            desktops.Clear();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            DataContext = null;
            GC.Collect();
        }
    }
}
