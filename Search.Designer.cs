namespace TareFlow
{
    partial class Search
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
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.lblVendor = new System.Windows.Forms.Label();
            this.tbVendor = new System.Windows.Forms.TextBox();
            this.lblPlate = new System.Windows.Forms.Label();
            this.tbPlate = new System.Windows.Forms.TextBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.tbCustomer = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.lblSecWeight = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.rbPlate = new System.Windows.Forms.RadioButton();
            this.rbDate = new System.Windows.Forms.RadioButton();
            this.rbCustomer = new System.Windows.Forms.RadioButton();
            this.rbVendor = new System.Windows.Forms.RadioButton();
            this.rbProduct = new System.Windows.Forms.RadioButton();
            this.rbTimestamp = new System.Windows.Forms.RadioButton();
            this.lblProduct = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dtStart
            // 
            this.dtStart.Location = new System.Drawing.Point(130, 264);
            this.dtStart.Margin = new System.Windows.Forms.Padding(10);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(100, 27);
            this.dtStart.TabIndex = 65;
            this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(29, 267);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(86, 20);
            this.lblDate.TabIndex = 64;
            this.lblDate.Text = "Seçili Tarih:";
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(113, 217);
            this.tbProduct.Margin = new System.Windows.Forms.Padding(10);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(240, 27);
            this.tbProduct.TabIndex = 56;
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(29, 175);
            this.lblVendor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(49, 20);
            this.lblVendor.TabIndex = 55;
            this.lblVendor.Text = "Satıcı:";
            // 
            // tbVendor
            // 
            this.tbVendor.Location = new System.Drawing.Point(113, 170);
            this.tbVendor.Margin = new System.Windows.Forms.Padding(10);
            this.tbVendor.Name = "tbVendor";
            this.tbVendor.Size = new System.Drawing.Size(240, 27);
            this.tbVendor.TabIndex = 54;
            // 
            // lblPlate
            // 
            this.lblPlate.AutoSize = true;
            this.lblPlate.Location = new System.Drawing.Point(29, 32);
            this.lblPlate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPlate.Name = "lblPlate";
            this.lblPlate.Size = new System.Drawing.Size(50, 20);
            this.lblPlate.TabIndex = 53;
            this.lblPlate.Text = "Plaka:";
            // 
            // tbPlate
            // 
            this.tbPlate.Location = new System.Drawing.Point(113, 29);
            this.tbPlate.Margin = new System.Windows.Forms.Padding(10);
            this.tbPlate.Name = "tbPlate";
            this.tbPlate.Size = new System.Drawing.Size(240, 27);
            this.tbPlate.TabIndex = 52;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(29, 128);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(42, 20);
            this.lblCustomer.TabIndex = 51;
            this.lblCustomer.Text = "Alıcı:";
            // 
            // tbCustomer
            // 
            this.tbCustomer.Location = new System.Drawing.Point(113, 123);
            this.tbCustomer.Margin = new System.Windows.Forms.Padding(10);
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.Size = new System.Drawing.Size(240, 27);
            this.tbCustomer.TabIndex = 50;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(33, 592);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 47);
            this.btnBack.TabIndex = 70;
            this.btnBack.Text = "Geri";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(280, 592);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 47);
            this.btnPrint.TabIndex = 69;
            this.btnPrint.Text = "Yazdır";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Location = new System.Drawing.Point(437, 21);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(720, 619);
            this.dataGridView1.TabIndex = 71;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // dtEnd
            // 
            this.dtEnd.Location = new System.Drawing.Point(253, 264);
            this.dtEnd.Margin = new System.Windows.Forms.Padding(10);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(100, 27);
            this.dtEnd.TabIndex = 74;
            this.dtEnd.ValueChanged += new System.EventHandler(this.dtEnd_ValueChanged);
            // 
            // lblSecWeight
            // 
            this.lblSecWeight.AutoSize = true;
            this.lblSecWeight.Location = new System.Drawing.Point(29, 384);
            this.lblSecWeight.Margin = new System.Windows.Forms.Padding(8);
            this.lblSecWeight.Name = "lblSecWeight";
            this.lblSecWeight.Size = new System.Drawing.Size(72, 20);
            this.lblSecWeight.TabIndex = 75;
            this.lblSecWeight.Text = "2. Tartım:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(29, 420);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(8);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(38, 20);
            this.lblTotal.TabIndex = 76;
            this.lblTotal.Text = "Net:";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(29, 348);
            this.lblWeight.Margin = new System.Windows.Forms.Padding(8);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(70, 20);
            this.lblWeight.TabIndex = 77;
            this.lblWeight.Text = "1. Tartım:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 79;
            this.label1.Text = "Tarih:";
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(113, 76);
            this.tbDate.Margin = new System.Windows.Forms.Padding(10);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(240, 27);
            this.tbDate.TabIndex = 78;
            // 
            // rbPlate
            // 
            this.rbPlate.AutoSize = true;
            this.rbPlate.Location = new System.Drawing.Point(366, 36);
            this.rbPlate.Name = "rbPlate";
            this.rbPlate.Size = new System.Drawing.Size(14, 13);
            this.rbPlate.TabIndex = 80;
            this.rbPlate.TabStop = true;
            this.rbPlate.UseVisualStyleBackColor = true;
            this.rbPlate.CheckedChanged += new System.EventHandler(this.rbPlate_CheckedChanged);
            // 
            // rbDate
            // 
            this.rbDate.AutoSize = true;
            this.rbDate.Location = new System.Drawing.Point(366, 83);
            this.rbDate.Name = "rbDate";
            this.rbDate.Size = new System.Drawing.Size(14, 13);
            this.rbDate.TabIndex = 81;
            this.rbDate.TabStop = true;
            this.rbDate.UseVisualStyleBackColor = true;
            this.rbDate.CheckedChanged += new System.EventHandler(this.rbDate_CheckedChanged);
            // 
            // rbCustomer
            // 
            this.rbCustomer.AutoSize = true;
            this.rbCustomer.Location = new System.Drawing.Point(366, 132);
            this.rbCustomer.Name = "rbCustomer";
            this.rbCustomer.Size = new System.Drawing.Size(14, 13);
            this.rbCustomer.TabIndex = 82;
            this.rbCustomer.TabStop = true;
            this.rbCustomer.UseVisualStyleBackColor = true;
            this.rbCustomer.CheckedChanged += new System.EventHandler(this.rbCustomer_CheckedChanged);
            // 
            // rbVendor
            // 
            this.rbVendor.AutoSize = true;
            this.rbVendor.Location = new System.Drawing.Point(366, 179);
            this.rbVendor.Name = "rbVendor";
            this.rbVendor.Size = new System.Drawing.Size(14, 13);
            this.rbVendor.TabIndex = 83;
            this.rbVendor.TabStop = true;
            this.rbVendor.UseVisualStyleBackColor = true;
            this.rbVendor.CheckedChanged += new System.EventHandler(this.rbVendor_CheckedChanged);
            // 
            // rbProduct
            // 
            this.rbProduct.AutoSize = true;
            this.rbProduct.Location = new System.Drawing.Point(366, 224);
            this.rbProduct.Name = "rbProduct";
            this.rbProduct.Size = new System.Drawing.Size(14, 13);
            this.rbProduct.TabIndex = 84;
            this.rbProduct.TabStop = true;
            this.rbProduct.UseVisualStyleBackColor = true;
            this.rbProduct.CheckedChanged += new System.EventHandler(this.rbProduct_CheckedChanged);
            // 
            // rbTimestamp
            // 
            this.rbTimestamp.AutoSize = true;
            this.rbTimestamp.Location = new System.Drawing.Point(366, 271);
            this.rbTimestamp.Name = "rbTimestamp";
            this.rbTimestamp.Size = new System.Drawing.Size(14, 13);
            this.rbTimestamp.TabIndex = 85;
            this.rbTimestamp.TabStop = true;
            this.rbTimestamp.UseVisualStyleBackColor = true;
            this.rbTimestamp.CheckedChanged += new System.EventHandler(this.rbTimestamp_CheckedChanged);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(29, 220);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(48, 20);
            this.lblProduct.TabIndex = 57;
            this.lblProduct.Text = "Ürün:";
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(157, 592);
            this.btnReload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(100, 47);
            this.btnReload.TabIndex = 86;
            this.btnReload.Text = "Yenile";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // Search
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.rbTimestamp);
            this.Controls.Add(this.rbProduct);
            this.Controls.Add(this.rbVendor);
            this.Controls.Add(this.rbCustomer);
            this.Controls.Add(this.rbDate);
            this.Controls.Add(this.rbPlate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDate);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblSecWeight);
            this.Controls.Add(this.dtEnd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dtStart);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.tbProduct);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.tbVendor);
            this.Controls.Add(this.lblPlate);
            this.Controls.Add(this.tbPlate);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.tbCustomer);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 700);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "Search";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kayıt Arama";
            this.Load += new System.EventHandler(this.Search_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.TextBox tbVendor;
        private System.Windows.Forms.Label lblPlate;
        private System.Windows.Forms.TextBox tbPlate;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox tbCustomer;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Label lblSecWeight;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.RadioButton rbPlate;
        private System.Windows.Forms.RadioButton rbDate;
        private System.Windows.Forms.RadioButton rbCustomer;
        private System.Windows.Forms.RadioButton rbVendor;
        private System.Windows.Forms.RadioButton rbProduct;
        private System.Windows.Forms.RadioButton rbTimestamp;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Button btnReload;
    }
}