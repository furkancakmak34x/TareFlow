using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class Search : Form
    {
        private readonly LPTPrinter LPT = new LPTPrinter();
        public Search()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);
        }

        private void LoadTable()
        {
            dataGridView1.DataSource = ListSecWeight();

            DataGridViewRow row = dataGridView1.Rows[0];

            tbPlate.Text = row.Cells["Plate"].Value.ToString();
            tbDate.Text = row.Cells["Date"].Value.ToString();
            tbCustomer.Text = row.Cells["Customer"].Value.ToString();
            tbVendor.Text = row.Cells["Vendor"].Value.ToString();
            tbProduct.Text = row.Cells["Product"].Value.ToString();
            lblWeight.Text = "İlk Tartım: " + row.Cells["Weight"].Value.ToString() + " KG";
            lblSecWeight.Text = "İkinci Tartım: " + row.Cells["SecWeight"].Value.ToString() + " KG";
            lblTotal.Text = "Net: " + row.Cells["Total"].Value.ToString() + " KG";
        }

        private void Search_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;

            LoadTable();

            if (dtStart.Value >= dtEnd.Value)
            {
                dtStart.Value = dtEnd.Value.AddDays(-1);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                tbPlate.Text = row.Cells["Plate"].Value.ToString();
                tbDate.Text = row.Cells["Date"].Value.ToString();
                tbCustomer.Text = row.Cells["Customer"].Value.ToString();
                tbVendor.Text = row.Cells["Vendor"].Value.ToString();
                tbProduct.Text = row.Cells["Product"].Value.ToString();
                lblWeight.Text = "İlk Tartım: " + row.Cells["Weight"].Value.ToString() + " KG";
                lblSecWeight.Text = "İkinci Tartım: " + row.Cells["SecWeight"].Value.ToString() + " KG";
                lblTotal.Text = "Net: " + row.Cells["Total"].Value.ToString() + " KG";
            }
        }

        private void rbPlate_CheckedChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM SecWeight WHERE @plate IS NULL OR Plate LIKE @plate ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@plate", $"%{tbPlate.Text}%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void rbDate_CheckedChanged(object sender, EventArgs e)
        {
            string date = DateTime.Parse(tbDate.Text).ToString("yyyy-MM-dd");
            string query = "SELECT * FROM SecWeight WHERE Date LIKE @date ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@date", $"{date}%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void rbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM SecWeight WHERE @customer IS NULL OR Customer LIKE @customer ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@customer", $"%{tbCustomer.Text}%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void rbVendor_CheckedChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM SecWeight WHERE @vendor IS NULL OR Vendor LIKE @vendor ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@vendor", $"%{tbVendor.Text}%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void rbProduct_CheckedChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM SecWeight WHERE @product IS NULL OR Product LIKE @product ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@product", $"%{tbProduct.Text}%");
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void rbTimestamp_CheckedChanged(object sender, EventArgs e)
        {
            string start = dtStart.Value.ToString("yyyy-MM-dd");
            string end = dtEnd.Value.ToString("yyyy-MM-dd");

            string query = "SELECT * FROM SecWeight WHERE Date BETWEEN @start AND @end ORDER BY Date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {


            if (dtEnd.Value <= dtStart.Value)
            {
                dtEnd.Value = dtStart.Value.AddDays(1);
            }

            if (rbTimestamp.Checked)
            {
                rbTimestamp_CheckedChanged(this, EventArgs.Empty);
            }

        }


        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dtStart.Value >= dtEnd.Value)
            {
                dtStart.Value = dtEnd.Value.AddDays(-1);
            }


            if (rbTimestamp.Checked)
            {
                rbTimestamp_CheckedChanged(this, EventArgs.Empty);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is RadioButton rb)
                    rb.Checked = false;
            }

            LoadTable();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DialogResult question = MessageBox.Show("Seçili sorguyu yazdırmak istiyor musunuz?", "İşlem Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            DataGridViewRow row = dataGridView1.Rows[0];

            if (question == DialogResult.Yes)
            {
                LPT.Print(row.Cells["Plate"].Value.ToString(),
                          row.Cells["Date"].Value.ToString(),
                          row.Cells["SecDate"].Value.ToString(),
                          row.Cells["Customer"].Value.ToString(),
                          row.Cells["Vendor"].Value.ToString(),
                          row.Cells["Product"].Value.ToString(),
                          row.Cells["Weight"].Value.ToString(),
                          row.Cells["SecWeight"].Value.ToString(),
                          row.Cells["Total"].Value.ToString());                      

                MessageBox.Show("Yazdırma işlemi başarılı.", "Yazdır", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                return;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
