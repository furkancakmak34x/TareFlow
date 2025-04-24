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
    public partial class Confirm: Form
    {
        public Confirm(string plate, string date, int weight, string customer, string vendor, string product)
        {
            this.Icon = Properties.Resources.icon;

            InitializeComponent();
            EnableDoubleBuffering(this);

            lblPlate.Text = "Plaka:  " + plate;
            lblDate.Text = "Tarih:  " + date;
            lblWeight.Text = "İlk Tartım:  " + Convert.ToString(weight) + " KG";
            lblCustomer.Text = "Alıcı:  " + customer;
            lblVendor.Text = "Satıcı:  " + vendor;
            lblProduct.Text = "Ürün:  " + product;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
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
