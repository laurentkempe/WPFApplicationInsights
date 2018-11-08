using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
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
                .WriteTo.ApplicationInsightsEvents(ConfigurationManager.AppSettings["ApplicationInsightsInstrumentationKey"])
                .CreateLogger();
#endif

            Exit += OnApp_Exit;

            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogAndFlushUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogAndFlushUnhandledException(Exception exception, string source)
        {
            LogUnhandledException(exception, source);
            Log.CloseAndFlush();
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            Log.Error(exception, source);
        }

        private static void OnApp_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Log.CloseAndFlush();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}