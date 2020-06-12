using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Chrome_Updater
{
    public partial class Regfile
    {
        public static void RegCreate(string applicationPath, string instDir, int icon)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\ChromeHTML.PORTABLE");
            key.SetValue(default, "Chrome HTML Document");
            key.SetValue("AppUserModelId", "Chrome.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\ChromeHTML.PORTABLE\\Application");
            key.SetValue("AppUserModelId", "Chrome.PORTABLE");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\Chrome.exe," + icon);
            key.SetValue("ApplicationName", "Google " + instDir + @" Portable");
            key.SetValue("ApplicationDescription", Langfile.Texts("AppDescriptShort"));
            key.SetValue("ApplicationCompany", "Google LLC");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\ChromeHTML.PORTABLE\\DefaultIcon");
            key.SetValue(default, applicationPath + @"\" + instDir + @"\Chrome.exe," + icon);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\ChromeHTML.PORTABLE\\shell\\open\\command");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" ""%1""");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\RegisteredApplications");
            key.SetValue("Google Chrome.PORTABLE", @"Software\Clients\StartMenuInternet\Google Chrome.PORTABLE\Capabilities");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE");
            key.SetValue(default, "Google " + instDir + @" Portable");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities");
            key.SetValue("ApplicationDescription", Langfile.Texts("AppDescriptFull"));
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\Chrome.exe," + icon);
            key.SetValue("ApplicationName", "Google " + instDir + @" Portable");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\FileAssociations");
            key.SetValue(".htm", "ChromeHTML.PORTABLE");
            key.SetValue(".html", "ChromeHTML.PORTABLE");
            key.SetValue(".shtml", "ChromeHTML.PORTABLE");
            key.SetValue(".svg", "ChromeHTML.PORTABLE");
            key.SetValue(".xht", "ChromeHTML.PORTABLE");
            key.SetValue(".xhtml", "ChromeHTML.PORTABLE");
            key.SetValue(".webp", "ChromeHTML.PORTABLE");
            key.SetValue(".pdf", "ChromeHTML.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\Startmenu");
            key.SetValue("StartMenuInternet", "Google Chrome.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\URLAssociations");
            key.SetValue("ftp", "ChromeHTML.PORTABLE");
            key.SetValue("http", "ChromeHTML.PORTABLE");
            key.SetValue("https", "ChromeHTML.PORTABLE");
            key.SetValue("irc", "ChromeHTML.PORTABLE");
            key.SetValue("mailto", "ChromeHTML.PORTABLE");
            key.SetValue("mms", "ChromeHTML.PORTABLE");
            key.SetValue("news", "ChromeHTML.PORTABLE");
            key.SetValue("nntp", "ChromeHTML.PORTABLE");
            key.SetValue("read", "ChromeHTML.PORTABLE");
            key.SetValue("sms", "ChromeHTML.PORTABLE");
            key.SetValue("smsto", "ChromeHTML.PORTABLE");
            key.SetValue("tel", "ChromeHTML.PORTABLE");
            key.SetValue("urn", "ChromeHTML.PORTABLE");
            key.SetValue("webcal", "ChromeHTML.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\DefaultIcon");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\Chrome.exe," + icon);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\InstallInfo");
            key.SetValue("ReinstallCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --make-default-browser");
            key.SetValue("HideIconsCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --hide-icons");
            key.SetValue("ShowIconsCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --show-icons");
            key.SetValue("IconsVisible", 1, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\shell\\open\\command");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" ""%1""");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.xhtml\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.xht\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.webp\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.svg\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.shtml\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.pdf\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.html\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.htm\\OpenWithProgids");
            key.SetValue("ChromeHTML.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" ""%1""");
            key.SetValue("Path", applicationPath);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts");
            key.SetValue("ChromeHTML.PORTABLE_http", 0, Microsoft.Win32.RegistryValueKind.DWord);
			key.SetValue("ChromeHTML.PORTABLE_https", 0, Microsoft.Win32.RegistryValueKind.DWord);
			key.SetValue("ChromeHTML.PORTABLE_.htm", 0, Microsoft.Win32.RegistryValueKind.DWord);
			key.SetValue("ChromeHTML.PORTABLE_.html", 0, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\https\\UserChoice");
            key.SetValue("ProgId", "ChromeHTML.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice");
            key.SetValue("ProgId", "ChromeHTML.PORTABLE");
            key.Close();
            try
            {
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
                if (key.GetValue("ProductName").ToString().Contains("Windows 10"))
                {
                    key.Close();
                    Process process = new Process();
                    process.StartInfo.FileName = "ms-settings:defaultapps";
                    process.Start();
                }
                else
                {
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void RegDel()
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.pdf\\UserChoice", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\FileAssociations", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\shell\\open\\command", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\shell\\open", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\DefaultIcon", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\InstallInfo", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\Startmenu", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE\\Capabilities\\URLAssociations", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable\\shell\\open\\command", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable\\shell\\open", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable\\shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable\\DefaultIcon", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable\\Application", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\ChromeHTML.Portable", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\Chrome.exe", false);
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\RegisteredApplications", true);
                key.DeleteValue("Google Chrome.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.xhtml\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.xht\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.webp\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.svg\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.shtml\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.pdf\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.html\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.htm\\OpenWithProgids", true);
                key.DeleteValue("ChromeHTML.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts", true);
                key.DeleteValue("ChromeHTML.PORTABLE_http", false);
				key.DeleteValue("ChromeHTML.PORTABLE_https", false);
				key.DeleteValue("ChromeHTML.PORTABLE_.htm", false);
				key.DeleteValue("ChromeHTML.PORTABLE_.html", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\https\\UserChoice", true);
                key.DeleteValue("Hash", false);
                key.DeleteValue("ProgId", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice", true);                
                key.DeleteValue("Hash", false);
                key.DeleteValue("ProgId", false);
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
