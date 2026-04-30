using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using SpieleLernApp.Services;
using SpieleLernApp.Views;

namespace SpieleLernApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        DispatcherUnhandledException += App_DispatcherUnhandledException;

        var startupWindow = new StartupWindow();
        MainWindow = startupWindow;
        startupWindow.Show();

        base.OnStartup(e);
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        WriteErrorLog("DispatcherUnhandledException", e.Exception);
        MessageBox.Show(
            "Es ist ein unerwarteter Fehler aufgetreten. Details stehen in app-error.log.",
            "SpieleLernApp",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        e.Handled = true;
    }

    private static void WriteErrorLog(string source, Exception exception)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}");
        logBuilder.AppendLine(exception.ToString());
        logBuilder.AppendLine(new string('-', 60));
        AppStoragePaths.AppendErrorLog(logBuilder.ToString());
    }
}
