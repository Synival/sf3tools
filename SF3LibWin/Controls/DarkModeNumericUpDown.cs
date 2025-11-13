using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    public class DarkModeNumericUpDown : NumericUpDown {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeNumericUpDown>(this);
                DarkModeContext.Init();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (DarkModeContext.Enabled) {
                using (var brush = new SolidBrush(Parent.BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                var borderBox = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                using (var pen = new Pen(DarkModeColors.BorderColor))
                    e.Graphics.DrawRectangle(pen, borderBox);
            }
            else
                base.OnPaint(e);
        }

        private DarkModeControlContext<DarkModeNumericUpDown> DarkModeContext { get; set; }
    }
}
