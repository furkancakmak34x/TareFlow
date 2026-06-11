namespace TareFlow
{
    partial class SecWeight
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
            this.components = new System.ComponentModel.Container();
            this.lblDate = new System.Windows.Forms.Label();
            this.tbWeight = new System.Windows.Forms.TextBox();
            this.lbWeight = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.lblVendor = new System.Windows.Forms.Label();
            this.tbVendor = new System.Windows.Forms.TextBox();
            this.lblPlate = new System.Windows.Forms.Label();
            this.tbPlate = new System.Windows.Forms.TextBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.tbCustomer = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lbSecWeight = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tbSecWeight = new System.Windows.Forms.TextBox();
            this.lblSecWeight = new System.Windows.Forms.Label();
            this.tbTotal = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelBanner = new System.Windows.Forms.Panel();
            this.lblBanner = new System.Windows.Forms.Label();
            this.lblUnderline = new System.Windows.Forms.Label();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.spScale = new System.IO.Ports.SerialPort(this.components);
            this.Scheduler = new System.Windows.Forms.Timer(this.components);
            this.tbSecDate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(23, 177);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(85, 20);
            this.lblDate.TabIndex = 48;
            this.lblDate.Text = "Giriş Tarihi:";
            // 
            // tbWeight
            // 
            this.tbWeight.Location = new System.Drawing.Point(134, 454);
            this.tbWeight.Margin = new System.Windows.Forms.Padding(10);
            this.tbWeight.Name = "tbWeight";
            this.tbWeight.Size = new System.Drawing.Size(240, 27);
            this.tbWeight.TabIndex = 47;
            // 
            // lbWeight
            // 
            this.lbWeight.AutoSize = true;
            this.lbWeight.Location = new System.Drawing.Point(23, 457);
            this.lbWeight.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbWeight.Name = "lbWeight";
            this.lbWeight.Size = new System.Drawing.Size(76, 20);
            this.lbWeight.TabIndex = 46;
            this.lbWeight.Text = "İlk Tartım:";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(23, 363);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(48, 20);
            this.lblProduct.TabIndex = 41;
            this.lblProduct.Text = "Ürün:";
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(134, 360);
            this.tbProduct.Margin = new System.Windows.Forms.Padding(10);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(240, 27);
            this.tbProduct.TabIndex = 40;
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(23, 318);
            this.lblVendor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(49, 20);
            this.lblVendor.TabIndex = 39;
            this.lblVendor.Text = "Satıcı:";
            // 
            // tbVendor
            // 
            this.tbVendor.Location = new System.Drawing.Point(134, 313);
            this.tbVendor.Margin = new System.Windows.Forms.Padding(10);
            this.tbVendor.Name = "tbVendor";
            this.tbVendor.Size = new System.Drawing.Size(240, 27);
            this.tbVendor.TabIndex = 38;
            // 
            // lblPlate
            // 
            this.lblPlate.AutoSize = true;
            this.lblPlate.Location = new System.Drawing.Point(23, 128);
            this.lblPlate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPlate.Name = "lblPlate";
            this.lblPlate.Size = new System.Drawing.Size(50, 20);
            this.lblPlate.TabIndex = 37;
            this.lblPlate.Text = "Plaka:";
            // 
            // tbPlate
            // 
            this.tbPlate.Location = new System.Drawing.Point(134, 125);
            this.tbPlate.Margin = new System.Windows.Forms.Padding(10);
            this.tbPlate.Name = "tbPlate";
            this.tbPlate.Size = new System.Drawing.Size(240, 27);
            this.tbPlate.TabIndex = 36;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(23, 271);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(42, 20);
            this.lblCustomer.TabIndex = 35;
            this.lblCustomer.Text = "Alıcı:";
            // 
            // tbCustomer
            // 
            this.tbCustomer.Location = new System.Drawing.Point(134, 266);
            this.tbCustomer.Margin = new System.Windows.Forms.Padding(10);
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.Size = new System.Drawing.Size(240, 27);
            this.tbCustomer.TabIndex = 34;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(517, 642);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 47);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Fiş Bas";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(27, 642);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(90, 47);
            this.btnBack.TabIndex = 32;
            this.btnBack.Text = "Çıkış";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lbSecWeight
            // 
            this.lbSecWeight.FormattingEnabled = true;
            this.lbSecWeight.ItemHeight = 20;
            this.lbSecWeight.Location = new System.Drawing.Point(429, 125);
            this.lbSecWeight.Name = "lbSecWeight";
            this.lbSecWeight.Size = new System.Drawing.Size(178, 404);
            this.lbSecWeight.TabIndex = 50;
            this.lbSecWeight.SelectedIndexChanged += new System.EventHandler(this.lbSecWeight_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(429, 544);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(178, 31);
            this.btnDelete.TabIndex = 51;
            this.btnDelete.Text = "Kaydı Sil";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tbSecWeight
            // 
            this.tbSecWeight.Location = new System.Drawing.Point(134, 501);
            this.tbSecWeight.Margin = new System.Windows.Forms.Padding(10);
            this.tbSecWeight.Name = "tbSecWeight";
            this.tbSecWeight.Size = new System.Drawing.Size(240, 27);
            this.tbSecWeight.TabIndex = 53;
            this.tbSecWeight.TextChanged += new System.EventHandler(this.tbSecWeight_TextChanged);
            this.tbSecWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSecWeight_KeyPress);
            // 
            // lblSecWeight
            // 
            this.lblSecWeight.AutoSize = true;
            this.lblSecWeight.Location = new System.Drawing.Point(23, 504);
            this.lblSecWeight.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSecWeight.Name = "lblSecWeight";
            this.lblSecWeight.Size = new System.Drawing.Size(96, 20);
            this.lblSecWeight.TabIndex = 52;
            this.lblSecWeight.Text = "İkinci Tartım:";
            this.lblSecWeight.DoubleClick += new System.EventHandler(this.lblSecWeight_DoubleClick);
            // 
            // tbTotal
            // 
            this.tbTotal.Location = new System.Drawing.Point(134, 548);
            this.tbTotal.Margin = new System.Windows.Forms.Padding(10);
            this.tbTotal.Name = "tbTotal";
            this.tbTotal.Size = new System.Drawing.Size(240, 27);
            this.tbTotal.TabIndex = 55;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(23, 551);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(38, 20);
            this.lblTotal.TabIndex = 54;
            this.lblTotal.Text = "Net:";
            // 
            // panelBanner
            // 
            this.panelBanner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBanner.Controls.Add(this.lblBanner);
            this.panelBanner.Controls.Add(this.lblUnderline);
            this.panelBanner.Location = new System.Drawing.Point(27, 21);
            this.panelBanner.Name = "panelBanner";
            this.panelBanner.Size = new System.Drawing.Size(580, 61);
            this.panelBanner.TabIndex = 56;
            // 
            // lblBanner
            // 
            this.lblBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblBanner.Location = new System.Drawing.Point(2, 19);
            this.lblBanner.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblBanner.Name = "lblBanner";
            this.lblBanner.Size = new System.Drawing.Size(576, 32);
            this.lblBanner.TabIndex = 40;
            this.lblBanner.Text = "İkinci Tartım İşlemi";
            this.lblBanner.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblUnderline
            // 
            this.lblUnderline.Font = new System.Drawing.Font("Impact", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnderline.Location = new System.Drawing.Point(2, 28);
            this.lblUnderline.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUnderline.Name = "lblUnderline";
            this.lblUnderline.Size = new System.Drawing.Size(576, 33);
            this.lblUnderline.TabIndex = 41;
            this.lblUnderline.Text = "                                                  ";
            this.lblUnderline.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(134, 172);
            this.tbDate.Margin = new System.Windows.Forms.Padding(10);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(240, 27);
            this.tbDate.TabIndex = 57;
            // 
            // Scheduler
            // 
            this.Scheduler.Interval = 50;
            this.Scheduler.Tick += new System.EventHandler(this.Scheduler_Tick);
            // 
            // tbSecDate
            // 
            this.tbSecDate.Location = new System.Drawing.Point(134, 219);
            this.tbSecDate.Margin = new System.Windows.Forms.Padding(10);
            this.tbSecDate.Name = "tbSecDate";
            this.tbSecDate.Size = new System.Drawing.Size(240, 27);
            this.tbSecDate.TabIndex = 59;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 224);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 58;
            this.label1.Text = "Çıkış Tarihi:";
            // 
            // SecWeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 711);
            this.Controls.Add(this.tbSecDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDate);
            this.Controls.Add(this.panelBanner);
            this.Controls.Add(this.tbTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.tbSecWeight);
            this.Controls.Add(this.lblSecWeight);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lbSecWeight);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.tbWeight);
            this.Controls.Add(this.lbWeight);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.tbProduct);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.tbVendor);
            this.Controls.Add(this.lblPlate);
            this.Controls.Add(this.tbPlate);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.tbCustomer);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBack);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(650, 750);
            this.MinimumSize = new System.Drawing.Size(650, 750);
            this.Name = "SecWeight";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İkinci Tartım";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SecWeight_FormClosing);
            this.Load += new System.EventHandler(this.SecWeight_Load);
            this.panelBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox tbWeight;
        private System.Windows.Forms.Label lbWeight;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.TextBox tbVendor;
        private System.Windows.Forms.Label lblPlate;
        private System.Windows.Forms.TextBox tbPlate;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox tbCustomer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ListBox lbSecWeight;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox tbSecWeight;
        private System.Windows.Forms.Label lblSecWeight;
        private System.Windows.Forms.TextBox tbTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panelBanner;
        private System.Windows.Forms.Label lblBanner;
        private System.Windows.Forms.Label lblUnderline;
        private System.Windows.Forms.TextBox tbDate;
        private System.IO.Ports.SerialPort spScale;
        private System.Windows.Forms.Timer Scheduler;
        private System.Windows.Forms.TextBox tbSecDate;
        private System.Windows.Forms.Label label1;
    }
}