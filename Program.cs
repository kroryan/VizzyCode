using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            var form = new MainForm(fileToOpen);
            form.Icon = CreateVizzyIcon();
            Application.Run(form);
        }

        /// <summary>
        /// Generates a simple "V" icon programmatically (no .ico file needed).
        /// Deep-blue background with a bright-green V + lightning bolt.
        /// </summary>
        private static Icon CreateVizzyIcon()
        {
            // Draw at 32x32
            using var bmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            using var g   = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Background – dark blue
            g.Clear(Color.FromArgb(20, 30, 60));

            // Rounded rectangle fill
            using var bgBrush = new SolidBrush(Color.FromArgb(0, 80, 180));
            g.FillRectangle(bgBrush, 0, 0, 32, 32);

            // "V" letter in bright electric green/cyan
            using var font = new Font("Segoe UI", 18f, FontStyle.Bold, GraphicsUnit.Pixel);
            using var brush = new SolidBrush(Color.FromArgb(0, 220, 200));
            var sf = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("V", font, brush, new RectangleF(0, -1, 32, 34), sf);

            // Small lightning bolt accent (top-right)
            using var boltBrush = new SolidBrush(Color.FromArgb(255, 220, 50));
            g.FillPolygon(boltBrush, new PointF[]
            {
                new PointF(24, 2), new PointF(20, 13), new PointF(24, 13),
                new PointF(20, 22), new PointF(28, 10), new PointF(24, 10)
            });

            // Convert bitmap to Icon
            IntPtr hIcon = bmp.GetHicon();
            try   { return Icon.FromHandle(hIcon); }
            catch { return SystemIcons.Application; }
        }
    }
}
