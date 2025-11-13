using System;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace SF3.Win.Controls {
    public class DarkModeGroupBox : GroupBox {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeGroupBox>(this);
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = DefaultBackColor;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (!DarkModeContext.Enabled) {
                base.OnPaint(e);
                return;
            }

            using (Brush textBrush = new SolidBrush(Enabled ? ForeColor : DarkModeColors.DisabledColor))
            using (Brush borderBrush = new SolidBrush(DarkModeColors.BorderColor))
            using (Pen borderPen = new Pen(borderBrush)) {
                var g = e.Graphics;
                SizeF strSize = g.MeasureString(Text, Font);
                Rectangle rect = new Rectangle(
                    ClientRectangle.X,
                    ClientRectangle.Y + (int)(strSize.Height / 2),
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - (int)(strSize.Height / 2) - 1
                );

                // Clear text and border
                g.Clear(BackColor);

                // Draw text
                g.DrawString(Text, Font, textBrush, Padding.Left, 0);

                // Drawing Border
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X,  rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X,  rect.Y), new Point(rect.X + Padding.Left, rect.Y));
                g.DrawLine(borderPen, new Point(rect.X + Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        private DarkModeControlContext<DarkModeGroupBox> DarkModeContext { get; set; }
    }
}
