using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Data.SqlClient;
using static TareFlow.Processes;

namespace TareFlow
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);
        }

        private void ImportCsvToDatabase(string filePath)
        {
            Connect();
            string[] lines = File.ReadAllLines(filePath);

            try
            {
                string query = "INSERT INTO SecWeight (Plate, Date, SecDate, Customer, Vendor, Product, Weight, SecWeight, Total) " +
                               "VALUES (@Plate, @Date, @SecDate, @Customer, @Vendor, @Product, @Weight, @SecWeight, @Total)";

                using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
                {
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        string plate = parts[1].Trim();
                        DateTime date = DateTime.Parse(parts[2].Trim() + " " + parts[3].Trim());
                        DateTime secDate = DateTime.Parse(parts[4].Trim() + " " + parts[5].Trim());
                        string customer = parts[6].Trim();
                        string vendor = parts[7].Trim();
                        string product = parts[8].Trim();
                        decimal weight = decimal.TryParse(parts[13].Trim(), out var w) ? w : 0;
                        decimal secWeight = decimal.TryParse(parts[14].Trim(), out var sw) ? sw : 0;
                        decimal total = decimal.TryParse(parts[15].Trim(), out var t) ? t : 0;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Plate", plate);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@SecDate", secDate);
                        cmd.Parameters.AddWithValue("@Customer", customer);
                        cmd.Parameters.AddWithValue("@Vendor", vendor);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Weight", weight);
                        cmd.Parameters.AddWithValue("@SecWeight", secWeight);
                        cmd.Parameters.AddWithValue("@Total", total);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                MessageBox.Show("Kayıtları aktarma işlemi başarılı.", "CSV Aktarma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Disconnect();
            }
        }


        private void Settings_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Form frm = new About();
            frm.ShowDialog();
        }

        private void btnTruncate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv";
            openFileDialog.Title = "CSV Dosyasını Seçin";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ImportCsvToDatabase(filePath);
            }
        }
    }
}
