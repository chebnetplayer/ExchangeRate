using System;
using System.Windows;

namespace GameClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += handler;
        }

        static void handler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception) args.ExceptionObject;
            MessageBox.Show(e.Message);
        }
    }
}
