namespace TareFlow
{
    partial class Home
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
            this.btnSecWeight = new System.Windows.Forms.Button();
            this.btnWeight = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReceivable = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSecWeight
            // 
            this.btnSecWeight.Location = new System.Drawing.Point(35, 255);
            this.btnSecWeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSecWeight.Name = "btnSecWeight";
            this.btnSecWeight.Size = new System.Drawing.Size(314, 47);
            this.btnSecWeight.TabIndex = 4;
            this.btnSecWeight.Text = "İkinci Tartım";
            this.btnSecWeight.UseVisualStyleBackColor = true;
            this.btnSecWeight.Click += new System.EventHandler(this.btnSecWeight_Click);
            // 
            // btnWeight
            // 
            this.btnWeight.Location = new System.Drawing.Point(35, 200);
            this.btnWeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWeight.Name = "btnWeight";
            this.btnWeight.Size = new System.Drawing.Size(314, 47);
            this.btnWeight.TabIndex = 3;
            this.btnWeight.Text = "İlk Tartım";
            this.btnWeight.UseVisualStyleBackColor = true;
            this.btnWeight.Click += new System.EventHandler(this.btnWeight_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(35, 310);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(314, 47);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Kayıt Arama";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReceivable
            // 
            this.btnReceivable.Location = new System.Drawing.Point(35, 365);
            this.btnReceivable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReceivable.Name = "btnReceivable";
            this.btnReceivable.Size = new System.Drawing.Size(314, 47);
            this.btnReceivable.TabIndex = 7;
            this.btnReceivable.Text = "Tahsilat Defteri";
            this.btnReceivable.UseVisualStyleBackColor = true;
            this.btnReceivable.Click += new System.EventHandler(this.btnReceivable_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(35, 420);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(314, 47);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(35, 475);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(314, 47);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "Çıkış";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::TareFlow.Properties.Resources.tareflow_wide;
            this.pbLogo.Location = new System.Drawing.Point(35, 68);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(314, 46);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 10;
            this.pbLogo.TabStop = false;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnReceivable);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSecWeight);
            this.Controls.Add(this.btnWeight);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 600);
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "Home";
            this.Padding = new System.Windows.Forms.Padding(32, 20, 32, 20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TareFlow - Araç Tartım Platformu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Home_FormClosing);
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSecWeight;
        private System.Windows.Forms.Button btnWeight;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReceivable;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox pbLogo;
    }
}