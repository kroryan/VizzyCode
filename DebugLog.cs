using System;
using System.IO;
using System.Text;

namespace VizzyCode
{
    internal static class DebugLog
    {
        private static readonly object Sync = new();
        private static string SettingsDir => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VizzyCode");
        private static string LogPath => Path.Combine(SettingsDir, "debug.log");

        public static void Write(string message)
        {
            try
            {
                Directory.CreateDirectory(SettingsDir);
                lock (Sync)
                {
                    File.AppendAllText(
                        LogPath,
                        $"[{DateTime.Now:O}] {message}{Environment.NewLine}",
                        new UTF8Encoding(false));
                }
            }
            catch
            {
            }
        }
    }
}
