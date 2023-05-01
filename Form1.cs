using System;
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
        public static string[] arapVersion = new string[8] { "-statsdef_1", "x86-dev-statsdef_1", "x86-beta-statsdef_1", "x86-stable-statsdef_1", "x64-canary-statsdef_1", "x64-dev-statsdef_1", "x64-beta-statsdef_1", "x64-stable-statsdef_1" };
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
        private readonly string[] CommandLineArgs = Environment.GetCommandLineArgs();
        private readonly ToolTip toolTip = new ToolTip();
        private Label[] labels = null;
        private Button[] buttons = null;
        private static string[] envs = new string[4] { "LAUNCHER_URL", "VERSION_URL", "UPDATE_URL", "DOWNLOAD_REPLACE" };

        // all url
        private static string launcherUrl = "https://raw.githubusercontent.com/UndertakerBen/PorChromeUpd/master/Launcher/Launcher.7z";
        private static string versionUrl = "https://raw.githubusercontent.com/UndertakerBen/PorChromeUpd/master/Version.txt";
        private static string updateUrl = "https://github.com/UndertakerBen/PorChromeUpd/releases/download/v{version}/Portable.Chrome.Updater.v{version}.7z";

        private static string[] chromeDownloadReplace = null;
        private static string chromeUpdateUrl = "https://tools.google.com/service/update2";
        private static readonly string chromeUpdateBody = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<request
    protocol=""3.0""
    updater=""Omaha""
    updaterversion=""1.3.36.152""
    shell_version=""1.3.36.151""
    ismachine=""0""
    sessionid=""{{11111111-1111-1111-1111-111111111111}}""
    installsource=""taggedmi""
    requestid=""{{11111111-1111-1111-1111-111111111111}}""
    dedup=""cr""
    domainjoined=""0"">
    <hw
        physmemory=""16""
        sse=""1""
        sse2=""1""
        sse3=""1""
        ssse3=""1""
        sse41=""1""
        sse42=""1""
        avx=""1""/>
    <os platform=""win"" version=""10.0.22621.1028"" sp="""" arch=""x64""/>
    <app
        appid=""{{{0}}}""
        version=""""
        nextversion=""""
        ap=""{1}""
        lang=""de""
        brand=""""
        client=""""
        installage=""-1""
        installdate=""-1""
        iid=""{{11111111-1111-1111-1111-111111111111}}"">
        <updatecheck/>
        <data name=""install"" index=""empty""/>
    </app>
</request>";
        public Form1()
        {
            InitializeComponent();
            initConfig();
            labels = new Label[4] { label2, label4, label6, label8 };
            buttons = new Button[8] { button1, button5, button2, button6, button3, button7, button4, button8 };
            foreach (Button b in buttons)
            {
                b.Enabled = false;
            }
            // use not block ui
            Task.Run(initCheck);
        }

        private void initConfig()
        {

            var config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var configPath = $"{applicationPath}\\config.ini";
            if (File.Exists(configPath))
            {
                config = ConfigHelper.GetConfig(configPath);
            }
            foreach (string env in envs)
            {
                string v = null;
                if (config.ContainsKey(env))
                {
                    v = config[env];
                }
                /*if (string.IsNullOrEmpty(v))
                {
                    v = Environment.GetEnvironmentVariable($"PCU_{env}");
                }*/
                if (!string.IsNullOrEmpty(v))
                {
                    v = v.Trim();
                    switch (env)
                    {
                        case "LAUNCHER_URL":
                            launcherUrl = v;
                            break;
                        case "VERSION_URL":
                            versionUrl = v;
                            break;
                        case "UPDATE_URL":
                            updateUrl = v;
                            break;
                        case "DOWNLOAD_REPLACE":
                            var arr = v.Split(',');
                            if (arr.Length == 2)
                            {
                                chromeDownloadReplace = arr;
                            }
                            break;
                    }
                }
            }
        }

        private async Task initCheck()
        {
            button9.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            if (IntPtr.Size != 8)
            {
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                checkBox3.Visible = false;
            }
            try
            {
                const string loading = "loading……";
                for (int i = 0; i <= 3; i++)
                {
                    labels[i].Text = loading;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Form1.chromeUpdateUrl);
                    request.Method = "POST";
                    request.UserAgent = "Google Update/1.3.36.152;winhttp";
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] byteArray = Encoding.UTF8.GetBytes(generateBody(arappid[i], arapVersion[i]));
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
                        labels[i].Text = buildversion[i];
                        int index = i * 2;
                        buttons[index].Enabled = true;
                        buttons[index + 1].Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: \n\r" + ex.Message);
            }
            Refresh();
            if (IntPtr.Size == 8)
            {
                if (File.Exists($"{applicationPath}\\Chrome Canary x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Dev x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Beta x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Stable x64\\Chrome.exe"))
                {
                    checkBox3.Enabled = false;
                }
                if (File.Exists($"{applicationPath}\\Chrome Canary x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Dev x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Beta x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Stable x86\\Chrome.exe"))
                {
                    checkBox2.Enabled = false;
                }
                if (File.Exists($"{applicationPath}\\Chrome Canary x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Dev x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Beta x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Stable x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Canary x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Dev x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Beta x64\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Stable x64\\Chrome.exe"))
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

                    if (File.Exists($"{applicationPath}\\Chrome\\Chrome.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            else if (IntPtr.Size != 8)
            {
                if (File.Exists($"{applicationPath}\\Chrome Canary x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Dev x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Beta x86\\Chrome.exe") || File.Exists($"{applicationPath}\\Chrome Stable x86\\Chrome.exe"))
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

                    if (File.Exists($"{applicationPath}\\Chrome\\Chrome.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.Equals("Chrome"))
                {
                    MessageBox.Show(Langfile.Texts("MeassageRunning"), "Portable Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            CheckLauncher();
            await TestCheck();
        }

        private static string generateBody(string appid, string ap)
        {
            return string.Format(chromeUpdateBody, appid, ap);
        }
        private async Task TestCheck()
        {
            await CheckUpdate();
            for (int i = 0; i < CommandLineArgs.GetLength(0); i++)
            {
                if (CommandLineArgs[i].ToLower().Equals("-updateall"))
                {
                    await UpdateAll();
                    await Task.Delay(2000);
                    Application.Exit();
                }
            }
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
            if ((!Directory.Exists($"{applicationPath}\\Chrome Canary x86")) && (!Directory.Exists($"{applicationPath}\\Chrome Dev x86")) && (!Directory.Exists($"{applicationPath}\\Chrome Beta x86")) && (!Directory.Exists($"{applicationPath}\\Chrome Stable x86")))
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
                if ((!Directory.Exists($"{applicationPath}\\Chrome Canary x64")) && (!Directory.Exists($"{applicationPath}\\Chrome Dev x64")) && (!Directory.Exists($"{applicationPath}\\Chrome Beta x64")) && (!Directory.Exists($"{applicationPath}\\Chrome Stable x64")))
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
        private async Task UpdateAll()
        {
            if (Directory.Exists($"{applicationPath}\\Chrome"))
            {
                string[] instVersion = File.ReadAllText($"{applicationPath}\\Chrome\\updates\\Version.log").Split(new char[] { '|' });
                if (instVersion[1] == "Canary" & instVersion[2] == "x86")
                {
                    if (new Version(buildversion[0]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(0, 0, 1);
                    }
                }
                if (instVersion[1] == "Developer" & instVersion[2] == "x86")
                {
                    if (new Version(buildversion[1]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(1, 0, 2);
                    }
                }
                if (instVersion[1] == "Beta" & instVersion[2] == "x86")
                {
                    if (new Version(buildversion[2]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(2, 0, 3);
                    }
                }
                if (instVersion[1] == "Stable" & instVersion[2] == "x86")
                {
                    if (new Version(buildversion[3]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(3, 0, 4);
                    }
                }
                if (instVersion[1] == "Canary" & instVersion[2] == "x64")
                {
                    if (new Version(buildversion[4]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(0, 1, 5);
                    }
                }
                if (instVersion[1] == "Developer" & instVersion[2] == "x64")
                {
                    if (new Version(buildversion[5]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(1, 1, 6);
                    }
                }
                if (instVersion[1] == "Beta" & instVersion[2] == "x64")
                {
                    if (new Version(buildversion[6]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(2, 1, 7);
                    }
                }
                if (instVersion[1] == "Stable" & instVersion[2] == "x64")
                {
                    if (new Version(buildversion[7]) > new Version(instVersion[0]))
                    {
                        await NewMethod1(3, 1, 8);
                    }
                }
            }
            await Testing();
            await Task.WhenAll();
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
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Form1.chromeUpdateUrl);
                request.Method = "POST";
                request.UserAgent = "Google Update/1.3.36.82;winhttp";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes(generateBody(arappid[a], arapVersion[d - 1]));
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
                    string tempURL = tempURL2[1] + tempURL4[1];
                    if (chromeDownloadReplace != null)
                    {
                        tempURL = tempURL.Replace(chromeDownloadReplace[0], chromeDownloadReplace[1]);
                    }
                    Uri uri = new Uri(tempURL);
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
                            if (args.Error != null)
                            {
                                MessageBox.Show("Download has been canceled.\n" + args.Error.Message);
                            }
                            else
                            {
                                downloadLabel.Text = Langfile.Texts("downUnpstart");
                                string arguments = $" x \"{applicationPath}\\Chrome_{architektur[c]}_{buildversion[a]}_{ring[a]}.exe\" -o\"{applicationPath}\\Update\\{entpDir[b]}\" -y";
                                Process process = new Process();
                                process.StartInfo.FileName = $"{applicationPath}\\Bin\\7zr.exe";
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                                process.StartInfo.Arguments = arguments;
                                process.Start();
                                process.WaitForExit();
                                process.StartInfo.Arguments = $" x \"{applicationPath}\\Update\\{entpDir[b]}\\Chrome.7z\" -o\"{applicationPath}\\Update\\{entpDir[b]}\" -y";
                                process.Start();
                                process.WaitForExit();
                                if (File.Exists($"{applicationPath}\\Update\\{entpDir[b]}\\chrome-bin\\Chrome.exe") && File.Exists($"{applicationPath}\\{instDir[b]}\\updates\\Version.log"))
                                {
                                    string[] instVersion = File.ReadAllText($"{applicationPath}\\{instDir[b]}\\updates\\Version.log").Split(new char[] { '|' });
                                    Version testm = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Update\\{entpDir[b]}\\chrome-bin\\Chrome.exe").FileVersion);
                                    if (testm != new Version(instVersion[0]))
                                    {
                                        if (Directory.Exists($"{applicationPath}\\{instDir[b]}\\{instVersion[0]}"))
                                        {
                                            Directory.Delete($"{applicationPath}\\{instDir[b]}\\{instVersion[0]}", true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(b, c, testm, d);
                                    }
                                    else if ((testm == new Version(instVersion[0])) && (checkBox4.Checked))
                                    {
                                        if (Directory.Exists($"{applicationPath}\\{instDir[b]}\\{instVersion[0]}"))
                                        {
                                            Directory.Delete($"{applicationPath}\\{instDir[d]}\\{instVersion[0]}", true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(b, c, testm, d);
                                    }
                                }
                                else
                                {
                                    if (!Directory.Exists($"{applicationPath}\\{instDir[b]}"))
                                    {
                                        Directory.CreateDirectory($"{applicationPath}\\{instDir[b]}");
                                    }
                                    NewMethod4(b, c, new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Update\\{entpDir[b]}\\chrome-bin\\Chrome.exe").FileVersion), d);
                                }
                            }
                            if (!File.Exists($"{applicationPath}\\{instDir[b]} Launcher.exe"))
                            {
                                File.Copy($"{applicationPath}\\Bin\\Launcher\\{instDir[b]} Launcher.exe", $"{applicationPath}\\{instDir[b]} Launcher.exe");
                            }
                            if (checkBox5.Checked)
                            {
                                if (!File.Exists($"{deskDir}\\{instDir[b]}.lnk"))
                                {
                                    NewMethod5(a, b);
                                }
                            }
                            else if (File.Exists($"{deskDir}\\{instDir[b]}.lnk") && ($"{applicationPath}\\{instDir[b]}" == "Chrome"))
                            {
                                NewMethod5(a, b);
                            }
                            File.Delete($"{applicationPath}\\Chrome_{architektur[c]}_{buildversion[a]}_{ring[a]}.exe");
                            downloadLabel.Text = Langfile.Texts("downUnpfine");
                        };
                        try
                        {
                            var task = webClient.DownloadFileTaskAsync(uri, $"{applicationPath}\\Chrome_{architektur[c]}_{buildversion[a]}_{ring[a]}.exe");
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
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n\r" + ex.Message);
                Controls.Remove(progressBox);
            }
        }
        public void CheckButton()
        {
            NewMethod3();
            for (int i = 0; i <= 7; i++)
            {
                if (File.Exists($"{applicationPath}\\{instDir[i]}\\updates\\Version.log"))
                {
                    Control[] buttons = Controls.Find("button" + (i + 1), true);
                    string[] instVersion = File.ReadAllText($"{applicationPath}\\{instDir[i]}\\updates\\Version.log").Split(new char[] { '|' });
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
                        button9.Text = Langfile.Texts("Button9UAll");
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
                checkBox3.Enabled = !File.Exists($"{applicationPath}\\Chrome Canary x64\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Dev x64\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Beta x64\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Stable x64\\Chrome.exe");
                checkBox2.Enabled = !File.Exists($"{applicationPath}\\Chrome Canary x86\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Dev x86\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Beta x86\\Chrome.exe") && !File.Exists($"{applicationPath}\\Chrome Stable x86\\Chrome.exe");
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
            if (File.Exists($"{applicationPath}\\Chrome\\updates\\Version.log"))
            {
                string[] instVersion = File.ReadAllText($"{applicationPath}\\Chrome\\updates\\Version.log").Split(new char[] { '|' });
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
            MessageBox.Show(Langfile.Texts("MeassageVersion"), "Portabel Chrome Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            if (Directory.Exists($"{applicationPath}\\Update"))
            {
                Directory.Delete($"{applicationPath}\\Update", true);
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
            if (File.Exists($"{applicationPath}\\{instDir[b]}\\updates\\Version.log"))
            {
                if (File.ReadAllText($"{applicationPath}\\{instDir[b]}\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
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
            if (File.Exists($"{applicationPath}\\Chrome\\updates\\Version.log"))
            {
                string[] instVersion = File.ReadAllText($"{applicationPath}\\Chrome\\updates\\Version.log").Split(new char[] { '|' });
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
            if (Directory.Exists($"{applicationPath}\\{instDir[b]}"))
            {
                if (File.Exists($"{applicationPath}\\{instDir[b]}\\updates\\Version.log"))
                {
                    if (File.ReadAllText($"{applicationPath}\\{instDir[b]}\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
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
        private void NewMethod4(int b, int c, Version testm, int d)
        {
            Directory.Move($"{applicationPath}\\Update\\{entpDir[b]}\\chrome-bin\\{testm}", $"{applicationPath}\\{instDir[b]}\\{testm}");
            File.Copy($"{applicationPath}\\Update\\{entpDir[b]}\\Chrome-bin\\Chrome.exe", $"{applicationPath}\\{instDir[b]}\\Chrome.exe", true);
            File.Copy($"{applicationPath}\\Update\\{entpDir[b]}\\Chrome-bin\\Chrome_proxy.exe", $"{applicationPath}\\{instDir[b]}\\Chrome_proxy.exe", true);
            if (!Directory.Exists($"{applicationPath}\\{instDir[b]}\\updates"))
            {
                Directory.CreateDirectory($"{applicationPath}\\{instDir[b]}\\updates");
            }
            File.WriteAllText($"{applicationPath}\\{instDir[b]}\\updates\\Version.log", testm + "|" + ring2[d - 1] + "|" + architektur2[c]);
            Directory.Delete($"{applicationPath}\\Update\\{entpDir[b]}", true);
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
            ShortcutHelper.CreateShortcut(
                $"{deskDir}\\{instDir[b]}.lnk",
                $"{applicationPath}{instDir[b]} Launcher.exe",
                workingDirectory: applicationPath,
                icon: $"{applicationPath}{instDir[b]}\\Chrome.exe,{icon[a]}"
            );
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
                if (File.Exists($"{applicationPath}\\Chrome\\updates\\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText($"{applicationPath}\\Chrome\\updates\\Version.log").Split(new char[] { '|' }));
                }
            }
            if (checkBox1.Checked)
            {
                if (File.Exists($"{applicationPath}\\{instDir[a]}\\updates\\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText($"{applicationPath}\\{instDir[a]}\\updates\\Version.log").Split(new char[] { '|' }));
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
        private async Task CheckUpdate()
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
                Size = new Size(50, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            Button updateButton = new Button
            {
                Location = new Point(groupBoxupdate.Width - Width - 10, 60),
                Size = new Size(50, 23),
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
            infoLabel.Text = Langfile.Texts("infoLabel");
            laterButton.Text = Langfile.Texts("laterButton");
            updateButton.Text = Langfile.Texts("updateButton");
            downLabel.Text = Langfile.Texts("downLabel");
            void LaterButton_Click(object sender, EventArgs e)
            {
                groupBoxupdate.Dispose();
                Controls.Remove(groupBoxupdate);
                groupBox3.Enabled = true;
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create(Form1.versionUrl);
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Version version = new Version(reader.ReadToEnd());
                    Version testm = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Portable Chrome Updater.exe").FileVersion);
                    versionLabel.Text = testm + "  >>> " + version;
                    if (version > testm)
                    {
                        for (int i = 0; i < CommandLineArgs.GetLength(0); i++)
                        {
                            if (CommandLineArgs[i].ToLower().Equals("-updateall"))
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                using (WebClient myWebClient2 = new WebClient())
                                {

                                    myWebClient2.DownloadFile(updateUrl.Replace("{version}", version.ToString()), $"{applicationPath}\\Portable.Chrome.Updater.v{version}.7z");
                                }
                                File.AppendAllText($"{applicationPath}\\Update.cmd", "@echo off" + "\r\n" +
                                    "timeout /t 5 /nobreak" + "\r\n" +
                                    "\"" + applicationPath + "\\Bin\\7zr.exe\" e \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\" -o\"" + applicationPath + "\" \"Portable Chrome Updater.exe\"" + " -y\r\n" +
                                    "call cmd /c Start /b \"\" " + "\"" + applicationPath + "\\Portable Chrome Updater.exe\" -UpdateAll\r\n" +
                                    "del /f /q \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\"\r\n" +
                                    "del /f /q \"" + applicationPath + "\\Update.cmd\" && exit\r\n" +
                                    "exit\r\n");
                                
                                string arguments = $" /c call \"{applicationPath}\\Update.cmd\"";
                                Process process = new Process();
                                process.StartInfo.FileName = "cmd.exe";
                                process.StartInfo.Arguments = arguments;
                                process.Start();
                                Close();
                                await Task.Delay(3000);
                            }
                        }
                        Controls.Add(groupBoxupdate);
                        groupBox3.Enabled = false;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update Updater Error\r\n{ex.Message}");
            }
            void UpdateButton_Click(object sender, EventArgs e)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                try
                {
                    var request2 = (WebRequest)HttpWebRequest.Create(Form1.versionUrl);
                    var response2 = request2.GetResponse();
                    using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                    {
                        var version = reader.ReadToEnd();
                        reader.Close();
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        using (WebClient myWebClient2 = new WebClient())
                        {
                            myWebClient2.DownloadFile(updateUrl.Replace("{version}", version.ToString()), $"{applicationPath}\\Portable.Chrome.Updater.v{version}.7z");
                        }
                        File.AppendAllText($"{applicationPath}\\Update.cmd", "@echo off" + "\n" +
                            "timeout /t 2 /nobreak" + "\n" +
                            "\"" + applicationPath + "\\Bin\\7zr.exe\" e \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\" -o\"" + applicationPath + "\" \"Portable Chrome Updater.exe\"" + " -y\n" +
                            "call cmd /c Start /b \"\" " + "\"" + applicationPath + "\\Portable Chrome Updater.exe\"\n" +
                            "del /f /q \"" + applicationPath + "\\Portable.Chrome.Updater.v" + version + ".7z\"\n" +
                            "del /f /q \"" + applicationPath + "\\Update.cmd\" && exit\n" +
                            "exit\n");

                        string arguments = $" /c call \"{applicationPath}\\Update.cmd\"";
                        Process process = new Process();
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = arguments;
                        process.Start();
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:\n\r" + ex.Message);
                }
            }
            await Task.WhenAll();
        }
        private void CheckLauncher()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create(Form1.versionUrl);
                //request.Proxy = ProxyClass.ProxyServer;
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Version version = new Version(reader.ReadToEnd());
                    Version testm = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Bin\\Launcher\\Chrome Launcher.exe").FileVersion);
                    if (version > testm)
                    {
                        reader.Close();
                        try
                        {
                            using (WebClient myWebClient2 = new WebClient())
                            {
                                myWebClient2.DownloadFile(launcherUrl, $"{applicationPath}\\Launcher.7z");
                            }
                            string arguments = $" x \"{applicationPath}\\Launcher.7z\" -o\"{applicationPath}\\Bin\\Launcher\" -y";
                            Process process = new Process();
                            process.StartInfo.FileName = $"{applicationPath}\\Bin\\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            File.Delete($"{applicationPath}\\Launcher.7z");
                            foreach (string launcher in instDir)
                            {
                                if (File.Exists($"{applicationPath}\\{launcher} Launcher.exe"))
                                {
                                    Version binLauncher = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Bin\\Launcher\\{launcher} Launcher.exe").FileVersion);
                                    Version istLauncher = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\{launcher} Launcher.exe").FileVersion);
                                    if (binLauncher > istLauncher)
                                    {
                                        File.Copy($"{applicationPath}\\Bin\\Launcher\\{launcher} Launcher.exe", $"{applicationPath}\\{launcher} Launcher.exe", true);
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
        private void VersinsInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Version updVersion = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Portable Chrome Updater.exe").FileVersion);
            Version launcherVersion = new Version(FileVersionInfo.GetVersionInfo($"{applicationPath}\\Bin\\Launcher\\Chrome Launcher.exe").FileVersion);
            MessageBox.Show("Updater Version - " + updVersion + "\nLauncher Version - " + launcherVersion, "Version Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RegistrierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[8], 0);
        }
        private void RegistrierenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[3], 0);
        }
        private void RegistrierenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[7], 0);
        }

        private void RegistrierenToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[2], 9);
        }
        private void RegistrierenToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[6], 9);
        }
        private void RegistrierenToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[1], 8);
        }
        private void RegistrierenToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[5], 8);
        }
        private void RegistrierenToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[0], 4);
        }
        private void RegistrierenToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instDir[4], 4);
        }
        private void EntfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem1.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem2.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem3.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem4.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem5.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem6.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem7.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem8.Enabled = true;
            Regfile.RegDel();
        }
        private void ExtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                if (Microsoft.Win32.Registry.GetValue("HKEY_Current_User\\Software\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE", default, null) != null)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Clients\\StartMenuInternet\\Google Chrome.PORTABLE", false);
                    switch (key.GetValue(default).ToString())
                    {
                        case "Google Chrome Portable":
                            key.Close();
                            registrierenToolStripMenuItem.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = true;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Stable x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem1.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = true;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Stable x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem2.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = true;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Beta x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem3.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = true;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Beta x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem4.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = true;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Dev x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem5.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = true;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Dev x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem6.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = true;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Canary x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem7.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = true;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                            break;
                        case "Google Chrome Canary x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem8.Enabled = false;
                            chromeAlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                            chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = true;
                            break;
                    }
                }
                else
                {
                    if (Directory.Exists($"{applicationPath}\\Chrome"))
                    {
                        chromeAlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeAlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Stable x86"))
                    {
                        chromeStableX86AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeStableX86AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Stable x64"))
                    {
                        chromeStableX64AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeStableX64AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Beta x86"))
                    {
                        chromeBetax86AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeBetax86AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Beta x64"))
                    {
                        chromeBetaX64AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeBetaX64AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Dev x86"))
                    {
                        chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeDeveloperX86AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Dev x64"))
                    {
                        chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeDeveloperX64AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Canary x86"))
                    {
                        chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeCanaryX86AlsStandardToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists($"{applicationPath}\\Chrome Canary x64"))
                    {
                        chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chromeCanaryX64AlsStandardToolStripMenuItem.Enabled = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
