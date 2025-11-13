using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    public class DarkModeCheckBox : CheckBox {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeCheckBox>(this);
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = DefaultBackColor;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (DarkModeContext.Enabled && !Enabled) {
                e.Graphics.Clear(Parent?.BackColor ?? Color.Empty);

                var state = Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(0, 2), state);

                int textX = ClientRectangle.X + CheckBoxRenderer.GetGlyphSize(e.Graphics, state).Width + 3;
                int textY = ClientRectangle.Y + ClientRectangle.Height / 2;
        
                TextRenderer.DrawText(
                    e.Graphics, Text, Font, new Point(textX, textY),
                    DarkModeColors.DisabledColor, 
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                );
            }
            else
                base.OnPaint(e);
        }

        private DarkModeControlContext<DarkModeCheckBox> DarkModeContext { get; set; }
    }
}
