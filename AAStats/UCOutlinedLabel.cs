#region Usings

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace AAStats
{
    public class UCOutlinedLabel : Label
    {
        public UCOutlinedLabel()
        {
            OutlineForeColor = Color.Green;
            OutlineWidth = 2;
        }

        public Color OutlineForeColor { get; set; }

        public float OutlineWidth { get; set; }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains information about the control to paint. </param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (BackColor != Color.Transparent)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            }
            using (var gp = new GraphicsPath())
            {
                using (var outline = new Pen(OutlineForeColor, OutlineWidth) { LineJoin = LineJoin.Round })
                {
                    using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        using (Brush foreBrush = new SolidBrush(ForeColor))
                        {
                            gp.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, this.ClientRectangle, sf);
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                            e.Graphics.DrawPath(outline, gp);
                            e.Graphics.FillPath(foreBrush, gp);
                        }
                    }
                }
            }
        }
    }
}