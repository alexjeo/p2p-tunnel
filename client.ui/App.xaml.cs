using server.models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;
namespace client.ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //TaskScheduler.UnobservedTaskException += (sender, e) =>
            //{
            //    _ = MessageBox.Error(e.Exception.Message);
            //};
            //DispatcherUnhandledException += (sender, e) =>
            //{
            //    _ = MessageBox.Error(e.Exception.Message);
            //};
            //AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            //{
            //    try
            //    {
            //        if (e.ExceptionObject is Exception exception)
            //        {
            //            _ = MessageBox.Error(exception.Message);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _ = MessageBox.Error(ex.Message);
            //    }
            //};

            //AppDomain.CurrentDomain.DomainUnload += (sender, e) =>
            //{
            //    _ = MessageBox.Error("被关闭");
            //};

            base.OnStartup(e);
        }
    }
}
