using RecoundLib;

namespace AAStats
{
    partial class AAOverlay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AAOverlay));
            this.ucBuffsListOfPlayer = new AAStats.UCBuffsList();
            this.ucBuffsListOfTarget = new AAStats.UCBuffsList();
            this.ctrlTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetRecountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucMsg = new AAStats.UCOutlinedLabel();
            this.cmTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucBuffsListOfPlayer
            // 
            this.ucBuffsListOfPlayer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ucBuffsListOfPlayer.Filter = null;
            this.ucBuffsListOfPlayer.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ucBuffsListOfPlayer.Location = new System.Drawing.Point(357, 83);
            this.ucBuffsListOfPlayer.Name = "ucBuffsListOfPlayer";
            this.ucBuffsListOfPlayer.ShowId = true;
            this.ucBuffsListOfPlayer.Size = new System.Drawing.Size(138, 396);
            this.ucBuffsListOfPlayer.TabIndex = 0;
            this.ucBuffsListOfPlayer.Load += new System.EventHandler(this.ucBuffsListOfPlayer_Load);
            // 
            // ucBuffsListOfTarget
            // 
            this.ucBuffsListOfTarget.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ucBuffsListOfTarget.Filter = null;
            this.ucBuffsListOfTarget.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ucBuffsListOfTarget.Location = new System.Drawing.Point(658, 84);
            this.ucBuffsListOfTarget.Name = "ucBuffsListOfTarget";
            this.ucBuffsListOfTarget.ShowId = true;
            this.ucBuffsListOfTarget.Size = new System.Drawing.Size(129, 395);
            this.ucBuffsListOfTarget.TabIndex = 1;
            // 
            // ctrlTray
            // 
            this.ctrlTray.ContextMenuStrip = this.cmTray;
            this.ctrlTray.Icon = ((System.Drawing.Icon)(resources.GetObject("ctrlTray.Icon")));
            this.ctrlTray.Text = "AA Stats";
            this.ctrlTray.Visible = true;
            // 
            // cmTray
            // 
            this.cmTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetRecountToolStripMenuItem,
            this.configModeToolStripMenuItem,
            this.showIdToolStripMenuItem,
            this.toolStripMenuItem1,
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.cmTray.Name = "cmTray";
            this.cmTray.Size = new System.Drawing.Size(147, 120);
            this.cmTray.Opening += new System.ComponentModel.CancelEventHandler(this.cmTray_Opening);
            // 
            // resetRecountToolStripMenuItem
            // 
            this.resetRecountToolStripMenuItem.Name = "resetRecountToolStripMenuItem";
            this.resetRecountToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.resetRecountToolStripMenuItem.Text = "Reset recount";
            // 
            // configModeToolStripMenuItem
            // 
            this.configModeToolStripMenuItem.Name = "configModeToolStripMenuItem";
            this.configModeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.configModeToolStripMenuItem.Text = "Config mode";
            this.configModeToolStripMenuItem.Click += new System.EventHandler(this.configModeToolStripMenuItem_Click);
            // 
            // showIdToolStripMenuItem
            // 
            this.showIdToolStripMenuItem.Name = "showIdToolStripMenuItem";
            this.showIdToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.showIdToolStripMenuItem.Text = "Show Id";
            this.showIdToolStripMenuItem.Click += new System.EventHandler(this.showIdToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ucMsg
            // 
            this.ucMsg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ucMsg.BackColor = System.Drawing.Color.DeepPink;
            this.ucMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ucMsg.ForeColor = System.Drawing.Color.White;
            this.ucMsg.Location = new System.Drawing.Point(200, 46);
            this.ucMsg.Name = "ucMsg";
            this.ucMsg.OutlineForeColor = System.Drawing.Color.Black;
            this.ucMsg.OutlineWidth = 2F;
            this.ucMsg.Size = new System.Drawing.Size(747, 35);
            this.ucMsg.TabIndex = 2;
            this.ucMsg.Text = "Выберите себя в цель";
        
            // 
            // AAOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1131, 531);
            this.Controls.Add(this.ucMsg);
            this.Controls.Add(this.ucBuffsListOfTarget);
            this.Controls.Add(this.ucBuffsListOfPlayer);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "AAOverlay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AAOverlay_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AAOverlay_FormClosed);
            this.Load += new System.EventHandler(this.AAOverlay_Load);
            this.cmTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UCBuffsList ucBuffsListOfPlayer;
        private UCBuffsList ucBuffsListOfTarget;
        private System.Windows.Forms.NotifyIcon ctrlTray;
        private System.Windows.Forms.ContextMenuStrip cmTray;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showIdToolStripMenuItem;
        private UCOutlinedLabel ucMsg;
        private System.Windows.Forms.ToolStripMenuItem resetRecountToolStripMenuItem;

    }
}
