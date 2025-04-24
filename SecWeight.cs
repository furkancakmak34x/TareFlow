using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;
using System.Drawing.Text;
using System.Globalization;

namespace TareFlow
{
    public partial class SecWeight : Form
    {
        public SecWeight()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);

            if (!spScale.IsOpen)
            {
                spScale.Open();
            }

            Scheduler.Start();
        }


        private void SecWeightCheck()
        {
            Connect();
            SQLiteCommand cmd = new SQLiteCommand("SELECT EXISTS (SELECT 1 FROM Weight)", _conn);
            bool check = Convert.ToInt32(cmd.ExecuteScalar()) == 1;

            if (check)
            {
                Disconnect();
                return;
            }

            else
            {
                Disconnect();
                MessageBox.Show("İkinci tartım listesi boş. Anasayfaya yönlendiriliyorsunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void AddScaleFee(string plate, string date, bool paid, bool type) 
        {
           
            if (paid == false)
            {
                Connect();
                int fee = 0;
                
                if (type == true)
                {
                    string query = "SELECT WeightFee FROM Fee WHERE Type = 'Van'";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
                    {
                        fee = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                else if (type == false)
                {
                    string query = "SELECT WeightFee FROM Fee WHERE Type = 'Truck'";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, _conn))
                    {
                        fee = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                string sorgu = "INSERT INTO Receivable (Plate, Date, Fee) VALUES (@plate, @date, @fee)";
                SQLiteCommand command = new SQLiteCommand(sorgu, _conn);
                command.Parameters.AddWithValue("@plate", plate);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@fee", fee);
                command.ExecuteNonQuery();
                Disconnect();
            }

            else if (paid == true)
            {
                return;
            }
        }

        private void PrintQuestion(bool print)
        {
            if (print == true)
            {
                LPTPrinter LPT = new LPTPrinter();

                LPT.Print(  tbPlate.Text,
                            tbDate.Text,
                            tbSecDate.Text,
                            tbCustomer.Text,
                            tbVendor.Text,
                            tbProduct.Text,
                            tbWeight.Text,
                            tbSecWeight.Text,
                            tbTotal.Text);
            }

            else if (print == false)
            {
                return;
            }
        }
        private void SecWeight_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
            tbSecDate.ReadOnly = true;
            tbSecDate.BackColor = SystemColors.Window;
            tbSecDate.ForeColor = SystemColors.ControlText;
            tbSecDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
            List<Database> list = ListWeight();
            lbSecWeight.DisplayMember = "Plate";
            lbSecWeight.ValueMember = "Plate";
            lbSecWeight.DataSource = list;

            foreach (Control control in this.Controls)
            {
                if (control is TextBox tb && tb.Name != "tbCustomer" && tb.Name != "tbProduct" && tb.Name != "tbVendor")
                {
                    tb.ReadOnly = true;
                    tb.BackColor = SystemColors.Window;
                    tb.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void lbSecWeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSecWeight.Text = "";
            tbTotal.Text = "";

            if (lbSecWeight.SelectedItem is Database item)
            {
                tbPlate.Text = item.Plate;
                tbDate.Text = item.Date;
                tbCustomer.Text = item.Customer;
                tbVendor.Text = item.Vendor;
                tbProduct.Text = item.Product;
                tbWeight.Text = item.Weight;
            }
        }

        private void tbSecWeight_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbWeight.Text, out int first) && int.TryParse(tbSecWeight.Text, out int second))
            {
                tbTotal.Text = Math.Abs(first - second).ToString();
            }
            else
            {
                tbTotal.Text = "";
            }
        }

        private void tbSecWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSecWeight.Text))
            {
                MessageBox.Show("Boş kısımları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                string plate = tbPlate.Text;
                string date = tbDate.Text;
                string secdate = tbSecDate.Text;
                string customer = tbCustomer.Text;
                string vendor = tbVendor.Text;
                string product = tbProduct.Text;
                int weight = Convert.ToInt32(tbWeight.Text);
                int secweight = Convert.ToInt32(tbSecWeight.Text);
                int total = Convert.ToInt32(tbTotal.Text);

                SecConfirm frm = new SecConfirm(plate, date, secdate, customer, vendor, product, weight, secweight, total);
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    bool paid = frm.paid;
                    bool print = frm.print;
                    bool type = frm.type;

                    try
                    {
                        SecWeight(plate, date, secdate, customer, vendor, product, weight, secweight, total);
                        DeleteWeight(plate, date);
                        AddScaleFee(plate, date, paid, type);
                        PrintQuestion(print);
                        SecWeight_Load(this, EventArgs.Empty);
                        lbSecWeight_SelectedIndexChanged(this, EventArgs.Empty);
                        SecWeightCheck();                      

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Kayıt eklenirken hata oluştu: " + ex.Message);
                    }
                }

                else
                {
                    return;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seçili kaydı silmek istediğinizden emin misiniz?", "Kayıt Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                string plate = tbPlate.Text;
                string date = tbDate.Text;
                DeleteWeight(plate, date);
                SecWeight_Load(this, EventArgs.Empty);
                lbSecWeight_SelectedIndexChanged(this, EventArgs.Empty);
                SecWeightCheck();
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
        private void SecWeight_FormClosing(object sender, FormClosingEventArgs e)
        {
            Scheduler.Stop();

            if (spScale.IsOpen)
            {
                spScale.Close();
            }
        }

        private void Scheduler_Tick(object sender, EventArgs e)
        {
            spScale.DiscardInBuffer();
            System.Threading.Thread.Sleep(100);
            int bytesToRead = spScale.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            spScale.Read(buffer, 0, bytesToRead);
            string data = Encoding.ASCII.GetString(buffer);
            string[] parts = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                string weight = parts[2].Trim();

                if (int.TryParse(weight, out int parsedWeight))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        tbSecWeight.Clear();
                        tbSecWeight.Text = parsedWeight.ToString();
                    });
                }
            }
        }

        private void lblSecWeight_DoubleClick(object sender, EventArgs e)
        {
            Scheduler.Stop();
            tbSecWeight.ReadOnly = false;
        }
    }
}
