using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AAShared;

namespace AAStats
{
    public partial class TransparentForm : Form
    {
        private bool m_clickThrough;

        public TransparentForm()
        {
            InitializeComponent();
        }

        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnResize(EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
            base.OnResize(e);
            this.CenterToScreen();
            this.Left += Offset.X;
            this.Top += Offset.Y;
        }

        public Point Offset { get; set; }

        private void TransparentForm_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode) return;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = this.MaximizeBox = false;
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.Left = 0;
            this.Top = 0;
        }

        public bool ClickThrough
        {
            get
            {
                return m_clickThrough;
            }
            set
            {
                m_clickThrough = value;
            }
        }


        protected override void DefWndProc(ref Message m)
        {
            const int WM_MOUSEACTIVATE = 0x21;
            const int MA_NOACTIVATE = 0x0003;
            if (m_clickThrough)
            {
                switch (m.Msg)
                {
                    case WM_MOUSEACTIVATE:
                        m.Result = (IntPtr)MA_NOACTIVATE;
                        return;
                }
            }
            base.DefWndProc(ref m);
        }


        /// <summary>
        ///   Кликабельность формы
        /// </summary>
        public bool ExTransparent
        {
            get
            {
                var exStyle = Utils.GetWindowLong(this.Handle, Utils.GWL_EXSTYLE);
                var result = true;
                result &= (exStyle & Utils.WS_EX_TRANSPARENT) == Utils.WS_EX_TRANSPARENT;
                return result;
            }
            set
            {
                var exStyle = Utils.GetWindowLong(this.Handle, Utils.GWL_EXSTYLE);
                if (value)
                {
                    exStyle |= Utils.WS_EX_TRANSPARENT;
                } else
                {
                    exStyle &= ~Utils.WS_EX_TRANSPARENT;
                }
                Utils.SetWindowLong(this.Handle, Utils.GWL_EXSTYLE, exStyle);
            }
        }

        public bool ExLayered
        {
            get
            {
                var exStyle = Utils.GetWindowLong(this.Handle, Utils.GWL_EXSTYLE);
                var result = true;
                result &= (exStyle & Utils.WS_EX_LAYERED) == Utils.WS_EX_LAYERED;
                return result;
            }
            set
            {
                var exStyle = Utils.GetWindowLong(this.Handle, Utils.GWL_EXSTYLE);
                if (value)
                {
                    exStyle |= Utils.WS_EX_LAYERED;
                }
                else
                {
                    exStyle &= ~Utils.WS_EX_LAYERED;
                }
                Utils.SetWindowLong(this.Handle, Utils.GWL_EXSTYLE, exStyle);
            }
        }
    }
}
