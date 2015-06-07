namespace AAStats
{
    partial class UCBuffsList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.gBuffs = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tmrUpdater = new System.Windows.Forms.Timer(this.components);
            this.lblCharName = new AAStats.UCOutlinedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gBuffs)).BeginInit();
            this.SuspendLayout();
            // 
            // gBuffs
            // 
            this.gBuffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBuffs.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.gBuffs.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.gBuffs.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gBuffs.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.gBuffs.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gBuffs.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.gBuffs.DisplayLayout.MaxColScrollRegions = 1;
            this.gBuffs.DisplayLayout.MaxRowScrollRegions = 1;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            appearance7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gBuffs.DisplayLayout.Override.ActiveCellAppearance = appearance7;
            appearance10.BackColor = System.Drawing.SystemColors.Highlight;
            appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gBuffs.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.gBuffs.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gBuffs.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            this.gBuffs.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gBuffs.DisplayLayout.Override.CellAppearance = appearance8;
            this.gBuffs.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gBuffs.DisplayLayout.Override.CellPadding = 0;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.gBuffs.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            appearance5.TextHAlignAsString = "Left";
            this.gBuffs.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.gBuffs.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gBuffs.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.gBuffs.DisplayLayout.Override.RowAppearance = appearance11;
            this.gBuffs.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gBuffs.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.gBuffs.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gBuffs.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gBuffs.Location = new System.Drawing.Point(7, 26);
            this.gBuffs.Name = "gBuffs";
            this.gBuffs.Size = new System.Drawing.Size(201, 249);
            this.gBuffs.TabIndex = 2;
            this.gBuffs.Text = "ultraGrid1";
            this.gBuffs.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gBuffs_InitializeLayout);
            this.gBuffs.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.gBuffs_InitializeRow);
            // 
            // tmrUpdater
            // 
            this.tmrUpdater.Enabled = true;
            this.tmrUpdater.Tick += new System.EventHandler(this.tmrUpdater_Tick);
            // 
            // lblCharName
            // 
            this.lblCharName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCharName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCharName.ForeColor = System.Drawing.Color.White;
            this.lblCharName.Location = new System.Drawing.Point(0, 0);
            this.lblCharName.Name = "lblCharName";
            this.lblCharName.OutlineForeColor = System.Drawing.Color.Black;
            this.lblCharName.OutlineWidth = 2F;
            this.lblCharName.Size = new System.Drawing.Size(212, 23);
            this.lblCharName.TabIndex = 3;
            this.lblCharName.Text = "<CharId>";
            // 
            // UCBuffsList
            // 
            this.Controls.Add(this.lblCharName);
            this.Controls.Add(this.gBuffs);
            this.Name = "UCBuffsList";
            this.Size = new System.Drawing.Size(212, 278);
            ((System.ComponentModel.ISupportInitialize)(this.gBuffs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid gBuffs;
        private System.Windows.Forms.Timer tmrUpdater;
        private UCOutlinedLabel lblCharName;
    }
}
