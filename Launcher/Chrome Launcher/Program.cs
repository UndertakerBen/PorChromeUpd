using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Chrome_Launcher
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo culture1 = CultureInfo.CurrentUICulture;
            if (File.Exists(@"Chrome\Chrome.exe"))
            {
                if (!File.Exists(@"Chrome\Profile.txt"))
                {
                    if (culture1.Name == "de-DE")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                        String Arguments = File.ReadAllText(@"Chrome\Profile.txt");
                        _ = Process.Start(@"Chrome\Chrome.exe", Arguments);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form2());
                        String Arguments = File.ReadAllText(@"Chrome\Profile.txt");
                        _ = Process.Start(@"Chrome\Chrome.exe", Arguments);
                    }
                    }
                else
                {
                    String Arguments = File.ReadAllText(@"Chrome\Profile.txt");
                    _ = Process.Start(@"Chrome\Chrome.exe", Arguments);
                }
            }
            else if (culture1.Name == "de-DE")
            {
                string message = "Chrome ist nicht installiert";
                _ = MessageBox.Show(message, "Chrome Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string message = "Chrome is not installed";
                _ = MessageBox.Show(message, "Chrome Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
