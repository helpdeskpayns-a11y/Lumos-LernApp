using System;
using System.IO;

namespace SpieleLernApp.Services;

public static class AppStoragePaths
{
    public const string AppFolderName = "Lumos-LernApp";

    public static string RootDirectory => EnsureDirectory(
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            AppFolderName));

    public static string DataDirectory => EnsureDirectory(Path.Combine(RootDirectory, "Data"));

    public static string ErrorLogPath => Path.Combine(RootDirectory, "app-error.log");

    public static void AppendErrorLog(string content)
    {
        try
        {
            File.AppendAllText(ErrorLogPath, content);
        }
        catch
        {
            string fallbackDirectory = EnsureDirectory(Path.Combine(Path.GetTempPath(), AppFolderName));
            string fallbackPath = Path.Combine(fallbackDirectory, "app-error.log");
            File.AppendAllText(fallbackPath, content);
        }
    }

    private static string EnsureDirectory(string path)
    {
        Directory.CreateDirectory(path);
        return path;
    }
}
