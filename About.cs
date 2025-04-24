using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
        }

        private void pbMail_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://mail.google.com/mail/?view=cm&fs=1&to=furkancakmak34x@gmail.com") { UseShellExecute = true });
        }

        private void pbGithub_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.github.com/furkancakmak34x") { UseShellExecute = true });
        }

        private void pbTelegram_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.telegram.me/furkancakmak34x") { UseShellExecute = true });
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
