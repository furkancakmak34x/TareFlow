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
    public partial class SecConfirm : Form
    {
        public bool paid { get; set; }
        public bool print { get; set; }
        public bool type { get; set; }
        public SecConfirm(string plate, string date, string secdate, string customer, string vendor, string product, int weight, int secweight, int total)
        {
            this.Icon = Properties.Resources.icon;

            InitializeComponent();
            EnableDoubleBuffering(this);

            lblPlate.Text = "Plaka:  " + plate;
            lblDate.Text = "Giriş Tarihi:  " + date;
            lblSecDate.Text = "Çıkış Tarihi  :" + secdate;
            lblCustomer.Text = "Alıcı:  " + customer;
            lblVendor.Text = "Satıcı:  " + vendor;
            lblProduct.Text = "Ürün:  " + product;
            lblWeight.Text = "İlk Tartım:  " + Convert.ToString(weight) + " KG";
            lblSecWeight.Text = "İkinci Tartım:  " + Convert.ToString(secweight) + " KG";
            lblTotal.Text = "Net:  " + Convert.ToString(total) + " KG";
        }
        private void SecConfirm_Load(object sender, EventArgs e)
        {
            rbPaid.Checked = true;
            rbPrint.Checked = true;
            rbVan.Checked = true;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (rbPaid.Checked) { paid = true; } else { paid = false; }
            if (rbPrint.Checked) { print = true; } else { print = false; }
            if (rbVan.Checked) { type = true; } else { type = false; }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
