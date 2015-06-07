namespace AAStats
{
    partial class UCCharacterBuff
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.bgImg = new Infragistics.Win.Misc.UltraPanel();
            this.lblDuration = new AAStats.UCOutlinedLabel();
            this.bgImg.ClientArea.SuspendLayout();
            this.bgImg.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgImg
            // 
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.bgImg.Appearance = appearance1;
            this.bgImg.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            // 
            // bgImg.ClientArea
            // 
            this.bgImg.ClientArea.Controls.Add(this.lblDuration);
            this.bgImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bgImg.Location = new System.Drawing.Point(0, 0);
            this.bgImg.Name = "bgImg";
            this.bgImg.Size = new System.Drawing.Size(48, 48);
            this.bgImg.TabIndex = 2;
            // 
            // lblDuration
            // 
            this.lblDuration.BackColor = System.Drawing.Color.Transparent;
            this.lblDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDuration.Font = new System.Drawing.Font("Moire ExtraBold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.ForeColor = System.Drawing.Color.White;
            this.lblDuration.Location = new System.Drawing.Point(0, 0);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.OutlineForeColor = System.Drawing.Color.Black;
            this.lblDuration.OutlineWidth = 2F;
            this.lblDuration.Size = new System.Drawing.Size(46, 46);
            this.lblDuration.TabIndex = 0;
            this.lblDuration.Text = "90m";
            // 
            // UCCharacterBuff
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.bgImg);
            this.Name = "UCCharacterBuff";
            this.Size = new System.Drawing.Size(48, 48);
            this.bgImg.ClientArea.ResumeLayout(false);
            this.bgImg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel bgImg;
        private UCOutlinedLabel lblDuration;

    }
}
