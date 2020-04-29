using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Chrome_Updater
{
    public partial class Form1 : Form
    {
        public static string[] ring = new string[4] { "Canary", "Dev", "Beta", "Stable" };
        public static string[] arappid = new string[4] { "4EA16AC7-FD5A-47C3-875B-DBF4A2008C20", "8A69D345-D564-463C-AFF1-A69D9E530F96", "8A69D345-D564-463C-AFF1-A69D9E530F96", "8A69D345-D564-463C-AFF1-A69D9E530F96" };
        public static string[] arapVersion = new string[8] { "x86", "x86-dev-statsdef_1", "x86-beta-statsdef_1", "x86-stable-statsdef_1", "x64", "x64-dev-statsdef_1", "x64-beta-statsdef_1", "x64-stable-statsdef_1" };
        public static string[] ring2 = new string[8] { "Canary", "Developer", "Beta", "Stable", "Canary", "Developer", "Beta", "Stable" };
        public static string[] buildversion = new string[8];
        public static string[] architektur = new string[2] { "X86", "X64" };
        public static string[] architektur2 = new string[2] { "x86", "x64" };
        public static string[] instDir = new string[9] { "Chrome Canary x86", "Chrome Dev x86", "Chrome Beta x86", "Chrome Stable x86", "Chrome Canary x64", "Chrome Dev x64", "Chrome Beta x64", "Chrome Stable x64", "Chrome" };
        public static string[] entpDir = new string[9] { "Canary86", "Dev86", "Beta86", "Stable86", "Canary64", "Dev64", "Beta64", "Stable64", "Single" };
        public static string[] icon = new string[4] { "4", "8", "9", "0" };
        WebClient webClient;
        readonly string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        readonly string applicationPath = Application.StartupPath;
        private readonly CultureInfo culture1 = CultureInfo.CurrentUICulture;
        readonly ToolTip toolTip = new ToolTip();
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i <= 3; i++)
            {
                WebRequest request = WebRequest.Create("http://tools.google.com/service/update2");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF-8\"?><request protocol=\"3.0\" updater=\"Omaha\" updaterversion=\"1.3.33.23\" shell_version=\"1.3.33.23\" ismachine=\"0\" sessionid=\"{11111111-1111-1111-1111-111111111111}\" requestid=\"{11111111-1111-1111-1111-111111111111}\"><os platform=\"win\" version=\"6.1\" sp=\"\" arch=\"x64\"/><app appid=\"{" + arappid[i] + "}\" version=\"\" ap=\"" + arapVersion[i] + "\" lang=\"\" brand=\"\" client=\"\" iid=\"{11111111-1111-1111-1111-111111111111}\"><updatecheck/></app></request>");
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("manifest version=")).Split(new char[] { '"' });
                    buildversion[i] = URL[1];
                    buildversion[i + 4] = URL[1];
                }
            }
            label2.Text = buildversion[0];
            label4.Text = buildversion[1];
            label6.Text = buildversion[2];
            label8.Text = buildversion[3];
            Refresh();
            button9.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    button10.Text = "Выход";
                    button9.Text = "Установить все";
                    label10.Text = "Установить все версии x86 и/или x64";
                    checkBox4.Text = "Игнорировать проверку версии";
                    checkBox1.Text = "Разные версии в отдельных папках";
                    checkBox5.Text = "Создать ярлык на рабочем столе";
                    break;
                case "de":
                    button10.Text = "Beenden";
                    button9.Text = "Alle Installieren";
                    label10.Text = "Alle x86 und oder x64 installieren";
                    checkBox4.Text = "Versionkontrolle ignorieren";
                    checkBox1.Text = "Für jede Version einen eigenen Ordner";
                    checkBox5.Text = "Eine Verknüpfung auf dem Desktop erstellen";
                    break;
                default:
                    button10.Text = "Quit";
                    button9.Text = "Install all";
                    label10.Text = "Install all x86 and or x64";
                    checkBox4.Text = "Ignore version check";
                    checkBox1.Text = "Create a Folder for each version";
                    checkBox5.Text = "Create a shortcut on the desktop";
                    break;
            }
            if (IntPtr.Size != 8)
            {
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                checkBox3.Visible = false;
            }
            if (IntPtr.Size == 8)
            {
                if (File.Exists(@"Chrome Canary x64\Chrome.exe") || File.Exists(@"Chrome Dev x64\Chrome.exe") || File.Exists(@"Chrome Beta x64\Chrome.exe") || File.Exists(@"Chrome Stable x64\Chrome.exe"))
                {
                    checkBox3.Enabled = false;
                }
                if (File.Exists(@"Chrome Canary x86\Chrome.exe") || File.Exists(@"Chrome Dev x86\Chrome.exe") || File.Exists(@"Chrome Beta x86\Chrome.exe") || File.Exists(@"Chrome Stable x86\Chrome.exe"))
                {
                    checkBox2.Enabled = false;
                }
                if (File.Exists(@"Chrome Canary x86\Chrome.exe") || File.Exists(@"Chrome Dev x86\Chrome.exe") || File.Exists(@"Chrome Beta x86\Chrome.exe") || File.Exists(@"Chrome Stable x86\Chrome.exe") || File.Exists(@"Chrome Canary x64\Chrome.exe") || File.Exists(@"Chrome Dev x64\Chrome.exe") || File.Exists(@"Chrome Beta x64\Chrome.exe") || File.Exists(@"Chrome Stable x64\Chrome.exe"))
                {
                    checkBox1.Checked = true;
                    CheckButton();
                }
                else if (!checkBox1.Checked)
                {
                    checkBox2.Enabled = false;
                    checkBox3.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);

                    if (File.Exists(@"Chrome\Chrome.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            else if (IntPtr.Size != 8)
            {
                if (File.Exists(@"Chrome Canary x86\Chrome.exe") || File.Exists(@"Chrome Dev x86\Chrome.exe") || File.Exists(@"Chrome Beta x86\Chrome.exe") || File.Exists(@"Chrome Stable x86\Chrome.exe"))
                {
                    checkBox1.Checked = true;
                    checkBox2.Enabled = false;
                    CheckButton();
                }
                else if (!checkBox1.Checked)
                {
                    checkBox2.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);

                    if (File.Exists(@"Chrome\Chrome.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            CheckUpdate();
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.Equals("Chrome"))
                {
                    switch (culture1.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            {
                                MessageBox.Show("Необходимо закрыть Google Chrome перед обновлением.", "Portable Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        case "de":
                            {
                                MessageBox.Show("Bitte schließen Sie den laufenden Google Chrome-Browser, bevor Sie den Browser aktualisieren.", "Portable Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        default:
                            {
                                MessageBox.Show("Please close the running Google Chrome browser before updating the browser.", "Portable Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                    }
                }
            }
            CheckLauncher();
        }
        private async void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(0, 0, 0, 1);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(0, 0, 1);
            }
        }
        private async void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(1, 1, 0, 2);
            }
            if (!checkBox1.Checked)
            {
                await NewMethod1(1, 0, 2);
            }
        }
        private async void Button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(2, 2, 0, 3);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(2, 0, 3);
            }
        }
        private async void Button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(3, 3, 0, 4);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(3, 0, 4);
            }
        }
        private async void Button5_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(0, 4, 1, 5);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(0, 1, 5);
            }
        }
        private async void Button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(1, 5, 1, 6);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(1, 1, 6);
            }
        }
        private async void Button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(2, 6, 1, 7);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(2, 1, 7);
            }
        }
        private async void Button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(3, 7, 1, 8);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(3, 1, 8);
            }
        }
        private async void Button9_Click(object sender, EventArgs e)
        {
            await Testing();
        }
        private async Task Testing()
        {
            if ((!Directory.Exists(@"Chrome Canary x86")) && (!Directory.Exists(@"Chrome Dev x86")) && (!Directory.Exists(@"Chrome Beta x86")) && (!Directory.Exists(@"Chrome Stable x86")))
            {
                if (checkBox2.Checked)
                {
                    await DownloadFile(0, 0, 0, 1);
                    await DownloadFile(1, 1, 0, 2);
                    await DownloadFile(2, 2, 0, 3);
                    await DownloadFile(3, 3, 0, 4);
                    checkBox2.Enabled = false;
                }
            }
            await NewMethod2(0, 0, 0, 1);
            await NewMethod2(1, 1, 0, 2);
            await NewMethod2(2, 2, 0, 3);
            await NewMethod2(3, 3, 0, 4);
            if (IntPtr.Size == 8)
            {
                if ((!Directory.Exists(@"Chrome Canary x64")) && (!Directory.Exists(@"Chrome Dev x64")) && (!Directory.Exists(@"Chrome Beta x64")) && (!Directory.Exists(@"Chrome Stable x64")))
                {
                    if (checkBox3.Checked)
                    {
                        await DownloadFile(0, 4, 1, 5);
                        await DownloadFile(1, 5, 1, 6);
                        await DownloadFile(2, 6, 1, 7);
                        await DownloadFile(3, 7, 1, 8);
                        checkBox3.Enabled = false;
                    }
                }
                await NewMethod2(0, 4, 1, 5);
                await NewMethod2(1, 5, 1, 6);
                await NewMethod2(2, 6, 1, 7);
                await NewMethod2(3, 7, 1, 8);
            }
        }
        public async Task DownloadFile(int a, int b, int c, int d)
        {
            GroupBox progressBox = new GroupBox
            {
                Location = new Point(10, button10.Location.Y + button10.Height + 5),
                Size = new Size(groupBox3.Width, 90),
                BackColor = Color.Lavender,
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(2, 10),
                Size = new Size(progressBox.Width - 4, 25),
                Text = "Chrome " + ring[a] + " " + buildversion[a] + " " + architektur2[c],
                TextAlign = ContentAlignment.BottomCenter
            };
            title.Font = new Font(title.Font.Name, 9.25F, FontStyle.Bold);
            Label downloadLabel = new Label
            {
                AutoSize = false,
                Location = new Point(5, 35),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.BottomLeft
            };
            Label percLabel = new Label
            {
                AutoSize = false,
                Location = new Point(progressBox.Size.Width - 105, 35),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.BottomRight
            };
            ProgressBar progressBarneu = new ProgressBar
            {
                Location = new Point(5, 65),
                Size = new Size(progressBox.Size.Width - 10, 7)
            };
            progressBox.Controls.Add(title);
            progressBox.Controls.Add(downloadLabel);
            progressBox.Controls.Add(percLabel);
            progressBox.Controls.Add(progressBarneu);
            Controls.Add(progressBox);
            List<Task> list = new List<Task>();

            WebRequest request = WebRequest.Create("http://tools.google.com/service/update2");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF-8\"?><request protocol=\"3.0\" updater=\"Omaha\" updaterversion=\"1.3.33.23\" shell_version=\"1.3.33.23\" ismachine=\"0\" sessionid=\"{11111111-1111-1111-1111-111111111111}\" requestid=\"{11111111-1111-1111-1111-111111111111}\"><os platform=\"win\" version=\"6.1\" sp=\"\" arch=\"x64\"/><app appid=\"{" + arappid[a] + "}\" version=\"\" ap=\"" + arapVersion[d - 1] + "\" lang=\"\" brand=\"\" client=\"\" iid=\"{11111111-1111-1111-1111-111111111111}\"><updatecheck/></app></request>");
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                string[] tempURL2 = responseFromServer.Substring(responseFromServer.LastIndexOf("codebase=")).Split(new char[] { '"' });
                string[] tempURL4 = responseFromServer.Substring(responseFromServer.IndexOf("run=")).Split(new char[] { '"' });
                string[] tempURL6 = responseFromServer.Substring(responseFromServer.IndexOf("manifest version=")).Split(new char[] { '"' });
                Uri uri = new Uri(tempURL2[1] + tempURL4[1]);
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLimit = 2;
                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += (o, args) =>
                    {
                        Control[] buttons = Controls.Find("button" + d, true);
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Orange;
                        }
                        progressBarneu.Value = args.ProgressPercentage;
                        downloadLabel.Text = string.Format("{0} MB's / {1} MB's",
                            (args.BytesReceived / 1024d / 1024d).ToString("0.00"),
                            (args.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
                        percLabel.Text = args.ProgressPercentage.ToString() + "%";
                    };
                    webClient.DownloadFileCompleted += (o, args) =>
                    {
                        if (args.Cancelled == true)
                        {
                            MessageBox.Show("Download has been canceled.");
                        }
                        else
                        {
                            switch (culture1.TwoLetterISOLanguageName)
                            {
                                case "ru":
                                    downloadLabel.Text = "Распаковка";
                                    break;
                                case "de":
                                    downloadLabel.Text = "Entpacken";
                                    break;
                                default:
                                    downloadLabel.Text = "Unpacking";
                                    break;
                            }
                            string arguments = " x " + "Chrome_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe" + " -o" + @"Update\" + entpDir[b] + " -y";
                            Process process = new Process();
                            process.StartInfo.FileName = @"Bin\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            process.StartInfo.Arguments = " x " + @"Update\" + entpDir[b] + "\\Chrome.7z -o" + @"Update\" + entpDir[b] + " -y";
                            process.Start();
                            process.WaitForExit();
                            if ((File.Exists(@"Update\" + entpDir[b] + "\\chrome-bin\\Chrome.exe")) && (File.Exists(instDir[b] + "\\updates\\Version.log")))
                            {
                                string[] instVersion = File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' });
                                FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\Chrome.exe");
                                if (checkBox1.Checked)
                                {
                                    if (testm.FileVersion != instVersion[0])
                                    {
                                        if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instDir[b] + "\\" + instVersion[0], true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(b, c, testm, d);
                                    }
                                    else if ((testm.FileVersion == instVersion[0]) && (checkBox4.Checked))
                                    {
                                        if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instDir[d] + "\\" + instVersion[0], true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(b, c, testm, d);
                                    }
                                }
                                else if (!checkBox1.Checked)
                                {
                                    if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                    {
                                        Directory.Delete(instDir[b] + "\\" + instVersion[0], true);
                                    }
                                    Thread.Sleep(2000);
                                    NewMethod4(b, c, testm, d);
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(instDir[b]))
                                {
                                    Directory.CreateDirectory(instDir[b]);
                                }
                                NewMethod4( b, c, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\Chrome.exe"), d);
                            }
                        }
                        if (checkBox5.Checked)
                        {
                            if (!File.Exists(deskDir + "\\" + instDir[b] + ".lnk"))
                            {
                                NewMethod5(a, b);
                            }
                        }
                        else if (File.Exists(deskDir + "\\" + instDir[b] + ".lnk") && (instDir[b] == "Chrome"))
                        {
                            NewMethod5(a, b);
                        }
                        if (!File.Exists(@instDir[b] + " Launcher.exe"))
                        {
                            File.Copy(@"Bin\Launcher\" + instDir[b] + " Launcher.exe", @instDir[b] + " Launcher.exe");
                        }
                        File.Delete("Chrome_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                        switch (culture1.TwoLetterISOLanguageName)
                        {
                            case "ru":
                                downloadLabel.Text = "Распакованный";
                                break;
                            case "de":
                                downloadLabel.Text = "Entpackt";
                                break;
                            default:
                                downloadLabel.Text = "Unpacked";
                                break;
                        }
                        downloadLabel.Text = culture1.Name != "de-DE" ? "Unpacked" : "Entpackt";
                    };
                    try
                    {
                        var task = webClient.DownloadFileTaskAsync(uri, "Chrome_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                        list.Add(task);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            await Task.WhenAll(list);
            await Task.Delay(2000);
            Controls.Remove(progressBox);
        }
        public void CheckButton()
        {
            NewMethod3();
            for (int i = 0; i <= 7; i++)
            {
                if (File.Exists(@instDir[i] + "\\updates\\Version.log"))
                {
                    Control[] buttons = Controls.Find("button" + (i + 1), true);
                    string[] instVersion = File.ReadAllText(@instDir[i] + "\\updates\\Version.log").Split(new char[] { '|' });
                    if (buildversion[i] == instVersion[0])
                    {
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Green;
                        }
                    }
                    else if (buildversion[i] != instVersion[0])
                    {
                        switch (culture1.TwoLetterISOLanguageName)
                        {
                            case "ru":
                                button9.Text = "Обновить все";
                                break;
                            case "de":
                                button9.Text = "Alle Updaten";
                                break;
                            default:
                                button9.Text = "Update all";
                                break;
                        }
                        button9.Enabled = true;
                        button9.BackColor = Color.FromArgb(224, 224, 224);
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        public void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (File.Exists(@"Chrome Canary x64\Chrome.exe") || File.Exists(@"Chrome Dev x64\Chrome.exe") || File.Exists(@"Chrome Beta x64\Chrome.exe") || File.Exists(@"Chrome Stable x64\Chrome.exe"))
                {
                    checkBox3.Enabled = false;
                }
                else
                {
                    checkBox3.Enabled = true;
                }
                if (File.Exists(@"Chrome Canary x86\Chrome.exe") || File.Exists(@"Chrome Dev x86\Chrome.exe") || File.Exists(@"Chrome Beta x86\Chrome.exe") || File.Exists(@"Chrome Stable x86\Chrome.exe"))
                {
                    checkBox2.Enabled = false;
                }
                else
                {
                    checkBox2.Enabled = true;
                }
                if (button9.Enabled)
                {
                    button9.BackColor = Color.FromArgb(224, 224, 224);
                }
                CheckButton();
            }
            if (!checkBox1.Checked)
            {
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
                CheckButton2();
            }
        }
        public void CheckButton2()
        {
            NewMethod3();
            if (File.Exists(@"Chrome\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Chrome\updates\Version.log").Split(new char[] { '|' });
                switch (instVersion[1])
                {
                    case "Canary":
                        NewMethod6(instVersion, 1, 5, 0);
                        break;
                    case "Developer":
                        NewMethod6(instVersion, 2, 6, 1);
                        break;
                    case "Beta":
                        NewMethod6(instVersion, 3, 7, 2);
                        break;
                    case "Stable":
                        NewMethod6(instVersion, 4, 8, 3);
                        break;
                }
            }
        }
        private void Button1_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(0, "x86");
        }
        private void Button2_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(1, "x86");
        }
        private void Button3_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(2, "x86");
        }
        private void Button4_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(3, "x86");
        }
        private void Button5_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(4, "x64");
        }
        private void Button6_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(5, "x64");
        }
        private void Button7_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(6, "x64");
        }
        private void Button8_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(7, "x64");
        }
        public void Message1()
        {
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    MessageBox.Show("Данная версия уже установлена", "Portabel Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                case "de":
                    MessageBox.Show("Die selbe Version ist bereits installiert", "Portabel Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                default:
                    MessageBox.Show("The same version is already installed", "Portabel Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox2.Checked) && (!checkBox3.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox2.Checked) && (!checkBox3.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
                Application.Exit();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Directory.Exists(@"Update"))
            {
                Directory.Delete(@"Update", true);
            }
        }
        private void Button9_EnabledChanged(object sender, EventArgs e)
        {
            if (!button9.Enabled)
            {
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private async Task NewMethod(int a, int b, int c, int d)
        {
            if (File.Exists(@instDir[b] + "\\updates\\Version.log"))
            {
                if (File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, b, c, d);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, b, c, d);
                }
            }
            else
            {
                await DownloadFile(a, b, c, d);
            }
        }
        private async Task NewMethod1(int a, int b, int c)
        {
            if (File.Exists(@"Chrome\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Chrome\updates\Version.log").Split(new char[] { '|' });
                if ((instVersion[0] == buildversion[a]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, 8, b, c);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, 8, b, c);
                }
            }
            else
            {
                await DownloadFile(a, 8, b, c);
            }
        }
        private async Task NewMethod2(int a, int b, int c, int d)
        {
            if (Directory.Exists(instDir[b]))
            {
                if (File.Exists(instDir[b] + "\\updates\\Version.log"))
                {
                    if (File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
                    {
                        await DownloadFile(a, b, c, d);
                    }
                }
            }
        }
        private void NewMethod3()
        {
            for (int i = 1; i <= 8; i++)
            {
                Control[] buttons = Controls.Find("button" + i, true);
                if (buttons.Length > 0)
                {
                    Button button = (Button)buttons[0];
                    button.BackColor = Color.FromArgb(224, 224, 224);
                }
            }
        }
        private void NewMethod4(int b, int c, FileVersionInfo testm, int d)
        {
            Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instDir[b] + "\\" + testm.FileVersion);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\Chrome.exe", instDir[b] + "\\Chrome.exe", true);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\Chrome_proxy.exe", instDir[b] + "\\Chrome_proxy.exe", true);
            if (!Directory.Exists(instDir[b] + "\\updates"))
            {
                Directory.CreateDirectory(instDir[b] + "\\updates");
            }
            File.WriteAllText(instDir[b] + "\\updates\\Version.log", testm.FileVersion + "|" + ring2[d - 1] + "|" + architektur2[c]);
            Directory.Delete(@"Update\" + entpDir[b], true);
            if (checkBox1.Checked)
            {
                CheckButton();
            }
            else if (!checkBox1.Checked)
            {
                CheckButton2();
            }
        }
        private void NewMethod5(int a, int b)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(deskDir + "\\" + instDir[b] + ".lnk");
            link.IconLocation = applicationPath + "\\" + instDir[b] + "\\Chrome.exe" + "," + icon[a];
            link.WorkingDirectory = applicationPath;
            link.TargetPath = applicationPath + "\\" + instDir[b] + " Launcher.exe";
            link.Save();
        }
        private void NewMethod6(string[] instVersion, int a, int b, int c)
        {
            Control[] buttons = Controls.Find("button" + a, true);
            Control[] buttons2 = Controls.Find("button" + b, true);
            if (instVersion[0] == buildversion[c])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Green;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Green;
                    }
                }
            }
            else if (instVersion[0] != buildversion[c])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Red;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Red;
                    }
                }
            }
        }
        private void NewMethod7(int a, string arch)
        {
            Control[] buttons = Controls.Find("button" + (a + 1), true);
            Button button = (Button)buttons[0];
            if (!checkBox1.Checked)
            {
                if (File.Exists(@"Chrome\updates\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(@"Chrome\updates\Version.log").Split(new char[] { '|' }));
                }
            }
            if (checkBox1.Checked)
            {
                if (File.Exists(instDir[a] + "\\updates\\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(instDir[a] + "\\updates\\Version.log").Split(new char[] { '|' }));
                }
            }
        }
        private void NewMethod8(int a, string arch, Button button, string[] instVersion)
        {
            if ((instVersion[1] == ring2[a]) && (instVersion[2] == arch))
            {
                toolTip.SetToolTip(button, instVersion[0]);
                toolTip.IsBalloon = true;
            }
            else
            {
                toolTip.SetToolTip(button, String.Empty);
            }
        }
        private void CheckUpdate()
        {
            GroupBox groupBoxupdate = new GroupBox
            {
                Location = new Point(groupBox3.Location.X, button10.Location.Y + button10.Size.Height + 5),
                Size = new Size(groupBox3.Width, 90),
                BackColor = Color.Aqua
            };
            Label versionLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 30),
                Size = new Size(groupBoxupdate.Width - 4, 25),
            };
            versionLabel.Font = new Font(versionLabel.Font.Name, 10F, FontStyle.Bold);
            Label infoLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 10),
                Size = new Size(groupBoxupdate.Width - 4, 20),
            };
            infoLabel.Font = new Font(infoLabel.Font.Name, 8.75F);
            Label downLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(100, 23),
            };
            Button laterButton = new Button
            {
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            Button updateButton = new Button
            {
                Location = new Point(groupBoxupdate.Width - Width - 10, 60),
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            updateButton.Location = new Point(groupBoxupdate.Width - updateButton.Width - 10, 60);
            laterButton.Location = new Point(updateButton.Location.X - laterButton.Width - 5, 60);
            downLabel.Location = new Point(laterButton.Location.X - downLabel.Width - 20, 60);
            groupBoxupdate.Controls.Add(updateButton);
            groupBoxupdate.Controls.Add(laterButton);
            groupBoxupdate.Controls.Add(downLabel);
            groupBoxupdate.Controls.Add(infoLabel);
            groupBoxupdate.Controls.Add(versionLabel);
            updateButton.Click += new EventHandler(UpdateButton_Click);
            laterButton.Click += new EventHandler(LaterButton_Click);
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    infoLabel.Text = "Доступна новая версия";
                    laterButton.Text = "нет";
                    updateButton.Text = "Да";
                    downLabel.Text = "ОБНОВИТЬ";
                    break;
                case "de":
                    infoLabel.Text = "Eine neue Version ist verfügbar";
                    laterButton.Text = "Nein";
                    updateButton.Text = "Ja";
                    downLabel.Text = "Jetzt Updaten";
                    break;
                default:
                    infoLabel.Text = "A new version is available";
                    laterButton.Text = "No";
                    updateButton.Text = "Yes";
                    downLabel.Text = "Update now";
                    break;
            }
            void LaterButton_Click(object sender, EventArgs e)
            {
                groupBoxupdate.Dispose();
                Controls.Remove(groupBoxupdate);
                groupBox3.Enabled = true;
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorChromeUpd/raw/master/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                   var version = reader.ReadToEnd();
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Portable Chrome Updater.exe");
                    versionLabel.Text = testm.FileVersion + "  >>> " + version;
                    if (version != testm.FileVersion)
                    {
                        Controls.Add(groupBoxupdate);
                        groupBox3.Enabled = false;
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                
            }
            void UpdateButton_Click(object sender, EventArgs e)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request2 = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorChromeUpd/raw/master/Version.txt");
                var response2 = request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    reader.Close();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient myWebClient2 = new WebClient())
                    {
                        myWebClient2.DownloadFile($"https://github.com/UndertakerBen/PorChromeUpd/releases/download/v{version}/Portable.Chrome.Updater.v{version}.7z", @"Portable.Chrome.Updater.v" + version + ".7z");
                    }
                    File.AppendAllText(@"Update.cmd", "@echo off" + "\n" +
                        "timeout /t 1 /nobreak" + "\n" +
                        "\"" + applicationPath + "\\Bin\\7zr.exe\" e \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\" -o\"" + applicationPath + "\" \"Portable Chrome Updater.exe\"" + " -y\n" +
                        "call cmd /c Start /b \"\" " + "\"" + applicationPath + "\\Portable Chrome Updater.exe\"\n" +
                        "del /f /q \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\"\n" +
                        "del /f /q \"" + applicationPath + "\\Update.cmd\" && exit\n" +
                        "exit\n");

                    string arguments = " /c call Update.cmd";
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = arguments;
                    process.Start();
                    Close();
                }
            }
        }
        private void CheckLauncher()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorChromeUpd/raw/master/Launcher/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\Chrome Launcher.exe");
                    if (version != testm.FileVersion)
                    {
                        reader.Close();
                        try
                        {
                            using (WebClient myWebClient2 = new WebClient())
                            {
                                myWebClient2.DownloadFile("https://github.com/UndertakerBen/PorChromeUpd/raw/master/Launcher/Launcher.7z", @"Launcher.7z");
                            }
                            string arguments = " x " + @"Launcher.7z" + " -o" + @"Bin\\Launcher" + " -y";
                            Process process = new Process();
                            process.StartInfo.FileName = @"Bin\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            File.Delete(@"Launcher.7z");
                            foreach (string launcher in instDir)
                            {
                                if (File.Exists(launcher + " Launcher.exe"))
                                {
                                    FileVersionInfo binLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\" + launcher + " Launcher.exe");
                                    FileVersionInfo istLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\" + launcher + " Launcher.exe");
                                    if (Convert.ToDecimal(binLauncher.FileVersion) > Convert.ToDecimal(istLauncher.FileVersion))
                                    {
                                        File.Copy(@"bin\\Launcher\\" + launcher + " Launcher.exe", launcher + " Launcher.exe", true);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
