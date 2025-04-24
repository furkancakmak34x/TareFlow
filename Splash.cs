using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class Splash : Form
    {
        private Timer timer;
        private Image splash;  
        public Splash()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);

            splash = Properties.Resources.tareflow_wide;
            this.TopMost = true;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.Size = splash.Size;
            this.BackgroundImage = splash;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            timer = new Timer();
            timer.Interval = 2500;
            timer.Tick += Timer_Tick;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Hide();
            Form frm = new Home();
            frm.Show();
        }
    }
}
