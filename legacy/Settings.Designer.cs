namespace TareFlow
{
    partial class Settings
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnLang = new System.Windows.Forms.Button();
            this.btnTruncate = new System.Windows.Forms.Button();
            this.btnLTP = new System.Windows.Forms.Button();
            this.btnCOM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(35, 299);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(314, 47);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "Anasayfaya Dön";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(35, 244);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(314, 47);
            this.btnAbout.TabIndex = 14;
            this.btnAbout.Text = "Hakkında";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnLang
            // 
            this.btnLang.Location = new System.Drawing.Point(35, 24);
            this.btnLang.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLang.Name = "btnLang";
            this.btnLang.Size = new System.Drawing.Size(314, 47);
            this.btnLang.TabIndex = 13;
            this.btnLang.Text = "Dil Seçenekleri";
            this.btnLang.UseVisualStyleBackColor = true;
            // 
            // btnTruncate
            // 
            this.btnTruncate.Location = new System.Drawing.Point(35, 189);
            this.btnTruncate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.Size = new System.Drawing.Size(314, 47);
            this.btnTruncate.TabIndex = 12;
            this.btnTruncate.Text = "CSV Aktarma";
            this.btnTruncate.UseVisualStyleBackColor = true;
            this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
            // 
            // btnLTP
            // 
            this.btnLTP.Location = new System.Drawing.Point(35, 134);
            this.btnLTP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLTP.Name = "btnLTP";
            this.btnLTP.Size = new System.Drawing.Size(314, 47);
            this.btnLTP.TabIndex = 11;
            this.btnLTP.Text = "LTP Port Ayarları";
            this.btnLTP.UseVisualStyleBackColor = true;
            // 
            // btnCOM
            // 
            this.btnCOM.Location = new System.Drawing.Point(35, 79);
            this.btnCOM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCOM.Name = "btnCOM";
            this.btnCOM.Size = new System.Drawing.Size(314, 47);
            this.btnCOM.TabIndex = 10;
            this.btnCOM.Text = "COM Port Ayarları";
            this.btnCOM.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 371);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnLang);
            this.Controls.Add(this.btnTruncate);
            this.Controls.Add(this.btnLTP);
            this.Controls.Add(this.btnCOM);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 410);
            this.MinimumSize = new System.Drawing.Size(400, 410);
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(32, 20, 32, 20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ayarlar";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnLang;
        private System.Windows.Forms.Button btnTruncate;
        private System.Windows.Forms.Button btnLTP;
        private System.Windows.Forms.Button btnCOM;
    }
}