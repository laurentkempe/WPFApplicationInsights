using System.Windows;
using System.Windows.Threading;
using Serilog;

namespace WPFApplicationInsights
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if DEBUG
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("logs\\WPFApplicationInsights.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
#else
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ApplicationInsights("TODO")
                .CreateLogger();
#endif


            Exit += OnApp_Exit;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private static void OnApp_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "DispatcherUnhandledException");
            Log.CloseAndFlush();
        }
    }
}