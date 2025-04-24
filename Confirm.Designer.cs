namespace TareFlow
{
    partial class Confirm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblConfirm = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblPlate = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblConfirm
            // 
            this.lblConfirm.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblConfirm.Location = new System.Drawing.Point(33, 300);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(418, 53);
            this.lblConfirm.TabIndex = 35;
            this.lblConfirm.Text = "Yukarıda yer alan bilgiler doğrultusunda kayıt ekleme işlemi yapılacaktır. İşlemi" +
    " onaylıyor musunuz?";
            this.lblConfirm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblProduct
            // 
            this.lblProduct.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProduct.Location = new System.Drawing.Point(33, 181);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(8);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(220, 21);
            this.lblProduct.TabIndex = 34;
            this.lblProduct.Text = "Ürün:";
            // 
            // lblVendor
            // 
            this.lblVendor.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblVendor.Location = new System.Drawing.Point(33, 144);
            this.lblVendor.Margin = new System.Windows.Forms.Padding(8);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(220, 21);
            this.lblVendor.TabIndex = 33;
            this.lblVendor.Text = "Satıcı:";
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCustomer.Location = new System.Drawing.Point(33, 107);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(8);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(220, 21);
            this.lblCustomer.TabIndex = 32;
            this.lblCustomer.Text = "Alıcı:";
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDate.Location = new System.Drawing.Point(33, 70);
            this.lblDate.Margin = new System.Windows.Forms.Padding(8);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(220, 21);
            this.lblDate.TabIndex = 31;
            this.lblDate.Text = "Tarih:";
            // 
            // lblWeight
            // 
            this.lblWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblWeight.Location = new System.Drawing.Point(256, 181);
            this.lblWeight.Margin = new System.Windows.Forms.Padding(8);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(220, 21);
            this.lblWeight.TabIndex = 30;
            this.lblWeight.Text = "İlk Tartım:";
            // 
            // lblPlate
            // 
            this.lblPlate.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPlate.Location = new System.Drawing.Point(33, 33);
            this.lblPlate.Margin = new System.Windows.Forms.Padding(8);
            this.lblPlate.Name = "lblPlate";
            this.lblPlate.Size = new System.Drawing.Size(220, 21);
            this.lblPlate.TabIndex = 29;
            this.lblPlate.Text = "Plaka:";
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(356, 393);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(100, 40);
            this.btnYes.TabIndex = 28;
            this.btnYes.Text = "Evet";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(28, 393);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(100, 40);
            this.btnNo.TabIndex = 27;
            this.btnNo.Text = "Hayır";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblPlate);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "Confirm";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İlk Tartım";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblPlate;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
    }
}