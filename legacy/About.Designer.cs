namespace TareFlow
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.pbGithub = new System.Windows.Forms.PictureBox();
            this.pbMail = new System.Windows.Forms.PictureBox();
            this.pbTelegram = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGithub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTelegram)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::TareFlow.Properties.Resources.tareflow_wide;
            this.pbLogo.Location = new System.Drawing.Point(50, 50);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(25);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(534, 101);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblVersion.Location = new System.Drawing.Point(50, 191);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(25);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(534, 306);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = resources.GetString("lblVersion.Text");
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(192, 593);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(250, 40);
            this.btnBack.TabIndex = 29;
            this.btnBack.Text = "Geri";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pbGithub
            // 
            this.pbGithub.Image = ((System.Drawing.Image)(resources.GetObject("pbGithub.Image")));
            this.pbGithub.Location = new System.Drawing.Point(191, 505);
            this.pbGithub.Margin = new System.Windows.Forms.Padding(25);
            this.pbGithub.Name = "pbGithub";
            this.pbGithub.Size = new System.Drawing.Size(50, 50);
            this.pbGithub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbGithub.TabIndex = 30;
            this.pbGithub.TabStop = false;
            this.pbGithub.Click += new System.EventHandler(this.pbGithub_Click);
            // 
            // pbMail
            // 
            this.pbMail.Image = global::TareFlow.Properties.Resources.mail;
            this.pbMail.Location = new System.Drawing.Point(391, 505);
            this.pbMail.Margin = new System.Windows.Forms.Padding(25);
            this.pbMail.Name = "pbMail";
            this.pbMail.Size = new System.Drawing.Size(50, 50);
            this.pbMail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMail.TabIndex = 32;
            this.pbMail.TabStop = false;
            this.pbMail.Click += new System.EventHandler(this.pbMail_Click);
            // 
            // pbTelegram
            // 
            this.pbTelegram.Image = global::TareFlow.Properties.Resources.telegram;
            this.pbTelegram.Location = new System.Drawing.Point(291, 505);
            this.pbTelegram.Margin = new System.Windows.Forms.Padding(25);
            this.pbTelegram.Name = "pbTelegram";
            this.pbTelegram.Size = new System.Drawing.Size(50, 50);
            this.pbTelegram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTelegram.TabIndex = 31;
            this.pbTelegram.TabStop = false;
            this.pbTelegram.Click += new System.EventHandler(this.pbTelegram_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 661);
            this.Controls.Add(this.pbGithub);
            this.Controls.Add(this.pbMail);
            this.Controls.Add(this.pbTelegram);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pbLogo);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(650, 700);
            this.MinimumSize = new System.Drawing.Size(650, 700);
            this.Name = "About";
            this.Padding = new System.Windows.Forms.Padding(25);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hakkında";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGithub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTelegram)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.PictureBox pbGithub;
        private System.Windows.Forms.PictureBox pbMail;
        private System.Windows.Forms.PictureBox pbTelegram;
    }
}