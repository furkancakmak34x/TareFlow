using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TareFlow.Processes;


namespace TareFlow
{
    public partial class Weight : Form
    {
        public Weight()
        {
            InitializeComponent();
            EnableDoubleBuffering(this);

            if (!spScale.IsOpen)
            {
                spScale.Open();
            }

            Scheduler.Start();
        }

        private bool NullCheck()
        {
            foreach (Control control in this.Controls)
            {
                if (control is System.Windows.Forms.TextBox && control.Visible && control.Enabled)
                {
                    System.Windows.Forms.TextBox tb = (TextBox)control;

                    if (string.IsNullOrWhiteSpace(tb.Text))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Weight_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.icon;
            tbDate.ReadOnly = true;
            tbDate.BackColor = SystemColors.Window;
            tbDate.ForeColor = SystemColors.ControlText;
            tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm", new CultureInfo("tr-TR"));
            tbCustomer.Enabled = false;
            tbVendor.Enabled = false;
            tbProduct.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (NullCheck())
            {
                string plate = tbPlate.Text;
                string date = tbDate.Text;
                int weight = Convert.ToInt32(tbWeight.Text);

                string customer = "";
                string vendor = "";
                string product = "";

                if (cbCustomer.Checked) { customer = tbCustomer.Text; }
                if (cbVendor.Checked) { vendor = tbVendor.Text; }
                if (cbProduct.Checked) { product = tbProduct.Text; }

                Confirm frm = new Confirm(plate, date, weight, customer, vendor, product);
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Weight(plate, date, weight, customer, vendor, product);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Kayıt eklenirken hata oluştu: " + ex.Message);
                    }

                    finally
                    {
                        this.Close();
                    }
                }

                else
                {
                    return;
                }
            }

            else
            {
                MessageBox.Show("Boş kısımları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCustomer.Checked)
            {
                tbCustomer.Enabled = true;
            }

            else
            {
                tbCustomer.Enabled = false;
                tbCustomer.Text = "";
            }
        }
        private void cbVendor_CheckedChanged(object sender, EventArgs e)
        {
            if (cbVendor.Checked)
            {
                tbVendor.Enabled = true;
            }

            else
            {
                tbVendor.Enabled = false;
                tbVendor.Text = "";
            }
        }
        private void cbProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProduct.Checked)
            {
                tbProduct.Enabled = true;
            }

            else
            {
                tbProduct.Enabled = false;
                tbProduct.Text = "";
            }
        }
        private void tbPlate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void tbWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void tbCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void tbVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void tbProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Weight_FormClosing(object sender, FormClosingEventArgs e)
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
                        tbWeight.Clear();
                        tbWeight.Text = parsedWeight.ToString();
                    });
                }
            }
        }
    }
}
