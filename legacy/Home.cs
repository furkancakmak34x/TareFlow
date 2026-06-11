using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class Home : Form
    {
        bool exit = false;
        public Home()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);
        }

        private void Home_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
        }

        private void btnWeight_Click(object sender, EventArgs e)
        {
            Form frm = new Weight();
            frm.ShowDialog();
        }

        private void btnSecWeight_Click(object sender, EventArgs e)
        {
            if (TableCheck("Weight")) {
                Form frm = new SecWeight();
                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("İkinci tartıma girecek araç bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (TableCheck("SecWeight"))
            {
                Form frm = new Search();
                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("Kayıt listesinde araç bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReceivable_Click(object sender, EventArgs e)
        {
            if (TableCheck("Receivable"))
            {
                Form frm = new Receivable();
                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("Tahsilat listesi boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form frm = new Settings();
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Yapılıyor", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                exit = true;
                Application.Exit();
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exit)
            {
                e.Cancel = true;
                btnExit.PerformClick();
            }
        }
    }
}
