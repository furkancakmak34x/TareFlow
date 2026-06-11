namespace TareFlow
{
    partial class Weight
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.tbCustomer = new System.Windows.Forms.TextBox();
            this.lblPlate = new System.Windows.Forms.Label();
            this.tbPlate = new System.Windows.Forms.TextBox();
            this.lblVendor = new System.Windows.Forms.Label();
            this.tbVendor = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.cbCustomer = new System.Windows.Forms.CheckBox();
            this.cbVendor = new System.Windows.Forms.CheckBox();
            this.cbProduct = new System.Windows.Forms.CheckBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.tbWeight = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.panelBanner = new System.Windows.Forms.Panel();
            this.lblBanner = new System.Windows.Forms.Label();
            this.lblUnderline = new System.Windows.Forms.Label();
            this.spScale = new System.IO.Ports.SerialPort(this.components);
            this.Scheduler = new System.Windows.Forms.Timer(this.components);
            this.panelBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(27, 492);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(90, 47);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Geri";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(267, 492);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 47);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Kayıt";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(29, 254);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(42, 20);
            this.lblCustomer.TabIndex = 11;
            this.lblCustomer.Text = "Alıcı:";
            // 
            // tbCustomer
            // 
            this.tbCustomer.Location = new System.Drawing.Point(100, 251);
            this.tbCustomer.Margin = new System.Windows.Forms.Padding(10);
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.Size = new System.Drawing.Size(235, 27);
            this.tbCustomer.TabIndex = 10;
            this.tbCustomer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCustomer_KeyPress);
            // 
            // lblPlate
            // 
            this.lblPlate.AutoSize = true;
            this.lblPlate.Location = new System.Drawing.Point(29, 110);
            this.lblPlate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPlate.Name = "lblPlate";
            this.lblPlate.Size = new System.Drawing.Size(50, 20);
            this.lblPlate.TabIndex = 14;
            this.lblPlate.Text = "Plaka:";
            // 
            // tbPlate
            // 
            this.tbPlate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbPlate.Location = new System.Drawing.Point(100, 107);
            this.tbPlate.Margin = new System.Windows.Forms.Padding(10);
            this.tbPlate.Name = "tbPlate";
            this.tbPlate.Size = new System.Drawing.Size(235, 27);
            this.tbPlate.TabIndex = 13;
            this.tbPlate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPlate_KeyPress);
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(29, 301);
            this.lblVendor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(49, 20);
            this.lblVendor.TabIndex = 17;
            this.lblVendor.Text = "Satıcı:";
            // 
            // tbVendor
            // 
            this.tbVendor.Location = new System.Drawing.Point(100, 298);
            this.tbVendor.Margin = new System.Windows.Forms.Padding(10);
            this.tbVendor.Name = "tbVendor";
            this.tbVendor.Size = new System.Drawing.Size(235, 27);
            this.tbVendor.TabIndex = 16;
            this.tbVendor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbVendor_KeyPress);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(29, 348);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(48, 20);
            this.lblProduct.TabIndex = 19;
            this.lblProduct.Text = "Ürün:";
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(100, 345);
            this.tbProduct.Margin = new System.Windows.Forms.Padding(10);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(235, 27);
            this.tbProduct.TabIndex = 18;
            this.tbProduct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbProduct_KeyPress);
            // 
            // cbCustomer
            // 
            this.cbCustomer.AutoSize = true;
            this.cbCustomer.Location = new System.Drawing.Point(348, 258);
            this.cbCustomer.Name = "cbCustomer";
            this.cbCustomer.Size = new System.Drawing.Size(15, 14);
            this.cbCustomer.TabIndex = 21;
            this.cbCustomer.UseVisualStyleBackColor = true;
            this.cbCustomer.CheckedChanged += new System.EventHandler(this.cbCustomer_CheckedChanged);
            // 
            // cbVendor
            // 
            this.cbVendor.AutoSize = true;
            this.cbVendor.Location = new System.Drawing.Point(348, 305);
            this.cbVendor.Name = "cbVendor";
            this.cbVendor.Size = new System.Drawing.Size(15, 14);
            this.cbVendor.TabIndex = 22;
            this.cbVendor.UseVisualStyleBackColor = true;
            this.cbVendor.CheckedChanged += new System.EventHandler(this.cbVendor_CheckedChanged);
            // 
            // cbProduct
            // 
            this.cbProduct.AutoSize = true;
            this.cbProduct.Location = new System.Drawing.Point(348, 352);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(15, 14);
            this.cbProduct.TabIndex = 23;
            this.cbProduct.UseVisualStyleBackColor = true;
            this.cbProduct.CheckedChanged += new System.EventHandler(this.cbProduct_CheckedChanged);
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(29, 157);
            this.lblWeight.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(56, 20);
            this.lblWeight.TabIndex = 26;
            this.lblWeight.Text = "Tartım:";
            // 
            // tbWeight
            // 
            this.tbWeight.Location = new System.Drawing.Point(100, 154);
            this.tbWeight.Margin = new System.Windows.Forms.Padding(10);
            this.tbWeight.MaxLength = 5;
            this.tbWeight.Name = "tbWeight";
            this.tbWeight.Size = new System.Drawing.Size(235, 27);
            this.tbWeight.TabIndex = 27;
            this.tbWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbWeight_KeyPress);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(29, 204);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(47, 20);
            this.lblDate.TabIndex = 29;
            this.lblDate.Text = "Tarih:";
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(100, 201);
            this.tbDate.Margin = new System.Windows.Forms.Padding(10);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(235, 27);
            this.tbDate.TabIndex = 36;
            // 
            // panelBanner
            // 
            this.panelBanner.AutoSize = true;
            this.panelBanner.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBanner.Controls.Add(this.lblBanner);
            this.panelBanner.Controls.Add(this.lblUnderline);
            this.panelBanner.Location = new System.Drawing.Point(27, 15);
            this.panelBanner.Name = "panelBanner";
            this.panelBanner.Size = new System.Drawing.Size(333, 61);
            this.panelBanner.TabIndex = 37;
            // 
            // lblBanner
            // 
            this.lblBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblBanner.Location = new System.Drawing.Point(2, 19);
            this.lblBanner.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblBanner.Name = "lblBanner";
            this.lblBanner.Size = new System.Drawing.Size(326, 32);
            this.lblBanner.TabIndex = 40;
            this.lblBanner.Text = "Tartım İşlemi";
            this.lblBanner.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblUnderline
            // 
            this.lblUnderline.Font = new System.Drawing.Font("Impact", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnderline.Location = new System.Drawing.Point(2, 28);
            this.lblUnderline.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUnderline.Name = "lblUnderline";
            this.lblUnderline.Size = new System.Drawing.Size(326, 33);
            this.lblUnderline.TabIndex = 41;
            this.lblUnderline.Text = "                                          ";
            this.lblUnderline.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // spScale
            // 
            this.spScale.DiscardNull = true;
            // 
            // Scheduler
            // 
            this.Scheduler.Interval = 50;
            this.Scheduler.Tick += new System.EventHandler(this.Scheduler_Tick);
            // 
            // Weight
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.panelBanner);
            this.Controls.Add(this.tbDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.tbWeight);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.cbProduct);
            this.Controls.Add(this.cbVendor);
            this.Controls.Add(this.cbCustomer);
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
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 600);
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "Weight";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İlk Tartım";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Weight_FormClosing);
            this.Load += new System.EventHandler(this.Weight_Load);
            this.panelBanner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox tbCustomer;
        private System.Windows.Forms.Label lblPlate;
        private System.Windows.Forms.TextBox tbPlate;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.TextBox tbVendor;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.CheckBox cbCustomer;
        private System.Windows.Forms.CheckBox cbVendor;
        private System.Windows.Forms.CheckBox cbProduct;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.TextBox tbWeight;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.Panel panelBanner;
        private System.Windows.Forms.Label lblBanner;
        private System.Windows.Forms.Label lblUnderline;
        public System.IO.Ports.SerialPort spScale;
        private System.Windows.Forms.Timer Scheduler;
    }
}