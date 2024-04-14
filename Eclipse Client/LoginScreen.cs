using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Eclipse_Client
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
            Init_Data();
        }
        private DiscordRPC.EventHandlers handlers;
        private DiscordRPC.RichPresence presence;
        void RPC()
        {
            this.handlers = default(DiscordRPC.EventHandlers);
            DiscordRPC.Initialize("1221202208410964118", ref this.handlers, true, null);
            this.presence.details = "eclipseclient.com";
            this.presence.state = "Login Screen";
            this.presence.largeImageKey = "rpc-image1";
            this.presence.largeImageText = "Open-Source, Free, Fast Minecraft Client";
            this.presence.startTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            DiscordRPC.UpdatePresence(ref this.presence);
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            loginBtn.Enabled = txtUsername.Text.Length >= 2;

        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            if (string.IsNullOrEmpty(username))
            {
                label2.Visible = true;
            }
            else
            {
                Main.username = username;
                Save_Data();
                Main m = new Main();
                m.Show();
                Hide();
            }
        }

        private void Init_Data()
        {
            if (Properties.Settings.Default.Username != string.Empty)
            {
                if (Properties.Settings.Default.RememberMe == true)
                {
                    txtUsername.Text = Properties.Settings.Default.Username;
                    chcRememberMe.Checked = true;
                }
                else
                {
                    txtUsername.Text = Properties.Settings.Default.Username;
                }
            }
        }

        private void Save_Data()
        {
            if (chcRememberMe.Checked)
            {
                Properties.Settings.Default.Username = txtUsername.Text.Trim();
                Properties.Settings.Default.RememberMe = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.RememberMe = false;
                Properties.Settings.Default.Save();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DiscordRPC)
            {
                RPC();
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) || bannedCharacters(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool bannedCharacters(char c)
        {
            string characters = "çÇğĞıİöÖşŞüÜ";
            return characters.Contains(c);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Process.Start("https://eclipseclient.com/discord");
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Process.Start("https://instagram.com/eclipseclient");
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Process.Start("https://eclipseclient.com/github");
        }
    }
}
