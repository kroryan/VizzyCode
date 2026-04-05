using System;
using System.Windows.Forms;

namespace VizzyCode
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            string? fileToOpen = args.Length > 0 ? args[0] : null;
            Application.Run(new MainForm(fileToOpen));
        }
    }
}
