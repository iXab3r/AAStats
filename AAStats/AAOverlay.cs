using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using AAShared;

using AAStats.Properties;

using XMLib;
using XMLib.Log;

using Timer = System.Windows.Forms.Timer;

namespace AAStats
{
    public partial class AAOverlay : AAStats.TransparentForm
    {
        private Timer m_timer;

        private bool m_configMode;

        public bool ConfigMode
        {
            get
            {
                return m_configMode;
            }
            set
            {
                m_configMode = value;
                UpdateUi();
            }
        }

        public AAOverlay()
        {
            InitializeComponent();
            ctrlTray.BalloonTipText = String.Format("{0} v{1}", Assembly.GetEntryAssembly().GetName(), Assembly.GetEntryAssembly().GetName().Version);
            m_timer = new Timer();
            m_timer.Tick += TimerOnTick;
            this.Visible = false;
        }
        
        private void TimerOnTick(object _sender, EventArgs _eventArgs)
        {
            try
            {
                var activeWindowTitle = Utils.GetActiveWindowTitle();
                var activeWindowHandler = Utils.GetForegroundWindow();
                if (m_configMode || activeWindowTitle.Contains("- ArcheAge"))
                {
                    Utils.ShowWindow(this.Handle, 8);
                }
                else
                {
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public bool WarningVisibility
        {
            get
            {
                return ucMsg.Visible;
            }
            set
            {
                ucMsg.Visible = value;
            }
        }

        public static AAOverlay Create()
        {
            AAOverlay result = null;
            var resetEvent = new ManualResetEvent(false);
            var thread = new Thread(
                () =>
                {
                    result = new AAOverlay();
                    resetEvent.Set();
                    Application.Run(result);
                }) { Name = "Overlay", IsBackground = false };
            thread.Start();
            resetEvent.WaitOne();
            return result;
        }

        private void AAOverlay_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
            {
                return;
            }
            this.TopMost = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = this.MaximizeBox = false;
            this.TransparencyKey = this.BackColor = Color.DeepPink;
            this.Left = 0;
            this.Top = 0;
            ConfigMode = false;
            LoadSettings();

            UpdateUi();

            m_timer.Enabled = true;
        }

        private void LoadSettings()
        {
            ucBuffsListOfPlayer.Filter = Properties.Settings.Default.PlayerFilter;
            ucBuffsListOfTarget.Filter = Properties.Settings.Default.TargetFilter;
            if (Settings.Default.PlayerFramePos != default(Point))
            {
                ucBuffsListOfPlayer.Location = Settings.Default.PlayerFramePos;
            }

            if (Settings.Default.TargetFramePos != default(Point))
            {
                ucBuffsListOfTarget.Location = Settings.Default.TargetFramePos;
            }

        }

        private void SaveSettings()
        {
            Settings.Default.PlayerFramePos = ucBuffsListOfPlayer.Location;
            Settings.Default.TargetFramePos = ucBuffsListOfTarget.Location;
            Properties.Settings.Default.Save();
        }

        public void InitializePlayerCharacter(Character _char)
        {
            ucBuffsListOfPlayer.Initialize(_char);
        }

        public void InitializeTargetCharacter(Character _char)
        {
            ucBuffsListOfTarget.Initialize(_char);
        }

        private void ucBuffsListOfPlayer_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Suicide();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Restart();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void Restart()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var exePath = entryAssembly.Location;
            Process.Start(exePath);
            Suicide();
        }

        private void Suicide()
        {
            Close();
        }

        private void cmTray_Opening(object sender, CancelEventArgs e)
        {
            RebuildTrayMenu();
        }

        private void RebuildTrayMenu()
        {
            configModeToolStripMenuItem.CheckState = m_configMode ? CheckState.Checked : CheckState.Unchecked;
            showIdToolStripMenuItem.CheckState = Settings.Default.ShowIds ? CheckState.Checked : CheckState.Unchecked;
        }

        private void configModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigMode = !ConfigMode;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                if (m_configMode)
                {
                    ConfigMode = false;
                }
            }
        }

        private void AAOverlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Utils.TerminateProcess(Process.GetCurrentProcess().Handle, 0);
        }

        private void AAOverlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_timer.Enabled = false;
            SaveSettings();
        }

        private void showIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowIds = !Settings.Default.ShowIds;
            UpdateUi();
        }

        private void UpdateUi()
        {
            ucBuffsListOfPlayer.ShowId = ucBuffsListOfTarget.ShowId = Settings.Default.ShowIds;

            if (m_configMode)
            {
                ucBuffsListOfPlayer.BackColor = Color.DimGray;
                ucBuffsListOfTarget.BackColor = Color.DimGray;
                this.ExTransparent = false;
            }
            else
            {
                ucBuffsListOfPlayer.BackColor = this.TransparencyKey;
                ucBuffsListOfTarget.BackColor = this.TransparencyKey;
                this.ExTransparent = true;
            }
        }

    }
}
