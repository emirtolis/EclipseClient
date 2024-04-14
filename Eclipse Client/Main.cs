using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CmlLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Eclipse_Client
{
    public partial class Main : Form
    {
        public Main()
        {

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        public static string username;
        public static string versiyon;
        public static bool fullscreen;
        private DiscordRPC.EventHandlers handlers;
        private DiscordRPC.RichPresence presence;
        void RPC()
        {
            this.handlers = default(DiscordRPC.EventHandlers);
            DiscordRPC.Initialize("1221202208410964118", ref this.handlers, true, null);
            this.presence.details = "eclipseclient.com";
            this.presence.state = "Home Screen";
            this.presence.largeImageKey = "rpc-image1";
            this.presence.largeImageText = "Open-Source, Free, Fast Minecraft Client";
            this.presence.startTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            DiscordRPC.UpdatePresence(ref this.presence);
        }

        void PlayingRPC()
        {
            this.handlers = default(DiscordRPC.EventHandlers);
            DiscordRPC.Initialize("1221202208410964118", ref this.handlers, true, null);
            this.presence.details = "eclipseclient.com";
            this.presence.state = "In The Game";
            this.presence.largeImageKey = "rpc-image1";
            this.presence.largeImageText = "Open-Source, Free, Fast Minecraft Client";
            this.presence.startTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            DiscordRPC.UpdatePresence(ref this.presence);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DiscordRPC)
            {
                RPC();
            }




            var request = WebRequest.Create("https://minotar.net/helm/" + username + "/100.png");
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                var bmp = new Bitmap(stream);
                guna2CirclePictureBox1.Image = bmp;
            }

            ramCombo.SelectedItem = Convert.ToString(Properties.Settings.Default.Ram);
            verCombo.SelectedItem = Convert.ToString(Properties.Settings.Default.Version);
            chcDiscordRpc.Checked = Properties.Settings.Default.DiscordRPC;
            chcFullScreen.Checked = Properties.Settings.Default.FullScreen;
            resolutionCombo.SelectedItem = Convert.ToString(Properties.Settings.Default.Resolution);

            path();
            lblUsername.Text = "Welcome, " + username;
        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            LoginScreen ls = new LoginScreen();
            ls.Show();
            Hide();
        }

        private void path()
        {
            MinecraftPath path = new MinecraftPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".eclipseclient"));
            var launcher = new CMLauncher(path);
            foreach (var item in launcher.GetAllVersions())
            {
                if (item.MType != MVersionType.OldBeta && item.MType != MVersionType.Snapshot && item.MType != MVersionType.OldAlpha)
                {
                    verCombo.Items.Add(item.Name);
                }
            }
        }

        private void Launch()
        {
            MinecraftPath path = new MinecraftPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".eclipseclient"));
            var launcher = new CMLauncher(path);
            launcher.FileChanged += (e) =>
           {
               StatusLabel.Text = (string.Format("[{0}] {1} - {2}/{3}", e.FileKind.ToString(), e.FileName, e.ProgressedFileCount, e.TotalFileCount));
           };
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            var launchOption = new MLaunchOption
            {
                MaximumRamMb = 4096,
                Session = MSession.GetOfflineSession(username),
                FullScreen = fullscreen



            };
            verCombo.SelectedItem = Properties.Settings.Default.Version;

            string selectedResolution = resolutionCombo.SelectedItem.ToString();
            string[] resolutionParts = selectedResolution.Split('x');
            if (resolutionParts.Length == 2)
            {
                int width, height;
                if (int.TryParse(resolutionParts[0], out width) && int.TryParse(resolutionParts[1], out height))
                {
                    launchOption.ScreenWidth = width;
                    launchOption.ScreenHeight = height;
                }
            }


            var process = launcher.CreateProcess(verCombo.SelectedItem.ToString(), launchOption);
            if (Properties.Settings.Default.DiscordRPC)
            {
                PlayingRPC();
            }
            process.Start();
            Hide();

            while (true)
            {
                Process[] pname = Process.GetProcessesByName("javaw");
                if (pname.Length > 0)
                {

                }
                else
                {
                    StatusLabel.Visible = false;
                    this.Show();
                    if (Properties.Settings.Default.DiscordRPC)
                    {
                        RPC();
                    }
                    playBtn.Enabled = true;
                    verCombo.Enabled = true;
                    resolutionCombo.Enabled = true;
                    ramCombo.Enabled = true;
                    break;
                }

            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        {
            ramCombo.SelectedItem = Properties.Settings.Default.Ram;
            verCombo.SelectedItem = Properties.Settings.Default.Version;
            chcDiscordRpc.Checked = Properties.Settings.Default.DiscordRPC;
            resolutionCombo.SelectedItem = Properties.Settings.Default.Resolution;

            guna2Panel3.Visible = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Panel3.Visible = false;
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chcFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FullScreen = chcFullScreen.Checked;
            Properties.Settings.Default.Save();

            if (chcFullScreen.Checked == true)
            {
                fullscreen = true;
            }
            else
            {
                fullscreen = false;
            }

        }

        private void ramCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Ram = Convert.ToInt16(ramCombo.SelectedItem);
            Properties.Settings.Default.Save();
        }

        private void verCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Version = Convert.ToString(verCombo.SelectedItem);
            Properties.Settings.Default.Save();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            StatusLabel.Visible = true;
            verCombo.Enabled = false;
            resolutionCombo.Enabled = false;
            playBtn.Enabled = false;
            ramCombo.Enabled = false;
            Thread thread = new Thread(() => Launch());
            thread.IsBackground = true;
            thread.Start();
        }

        private void chcDiscordRpc_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.DiscordRPC = chcDiscordRpc.Checked;
            Properties.Settings.Default.Save();
        }

        private void resolutionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Resolution = Convert.ToString(resolutionCombo.SelectedItem);
            Properties.Settings.Default.Save();
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
