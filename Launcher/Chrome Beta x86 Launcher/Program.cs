using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Chrome_Beta_x86_Launcher
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
            if (File.Exists(@"Chrome Beta x86\Chrome.exe"))
            {
                var sb = new System.Text.StringBuilder();
                string[] CommandLineArgs = Environment.GetCommandLineArgs();
                for (int i = 1; i < CommandLineArgs.Length; i++)
                {
                    if (CommandLineArgs[i].Contains("="))
                    {
                        string[] test = CommandLineArgs[i].Split(new char[] { '=' }, 2);
                        sb.Append(" " + test[0] + "=\"" + test[1] + "\"");
                    }
                    else
                    {
                        sb.Append(" " + CommandLineArgs[i]);
                    }
                }
                if (!File.Exists(@"Chrome Beta x86\Profile.txt"))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                    String Arguments = File.ReadAllText(@"Chrome Beta x86\Profile.txt") + sb.ToString();
                    _ = Process.Start(@"Chrome Beta x86\Chrome.exe", Arguments);
                }
                else
                {
                    String Arguments = File.ReadAllText(@"Chrome Beta x86\Profile.txt") + sb.ToString();
                    _ = Process.Start(@"Chrome Beta x86\Chrome.exe", Arguments);
                }
            }
            else if (culture1.TwoLetterISOLanguageName == "de")
            {
                _ = MessageBox.Show("Chrome ist nicht installiert", "Chrome Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (culture1.TwoLetterISOLanguageName == "ru")
            {
                _ = MessageBox.Show("Google Chrome Portable не найден", "Chrome Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                _ = MessageBox.Show("Chrome is not installed", "Chrome Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
