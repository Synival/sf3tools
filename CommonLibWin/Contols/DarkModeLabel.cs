using System;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    public class DarkModeLabel : Label {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeLabel>(this);
                DarkModeContext.EnabledChanged += (s, e) => {
                    DisabledColor = DarkModeContext.Enabled ? DarkModeColors.DisabledColor : Color.Empty;
                };
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = DefaultBackColor;
            }
        }

        private static TextFormatFlags ToTextFormatFlags(ContentAlignment align) {
            switch (align) {
                case ContentAlignment.TopLeft:      return TextFormatFlags.Top            | TextFormatFlags.Left;
                case ContentAlignment.TopCenter:    return TextFormatFlags.Top            | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight:     return TextFormatFlags.Top            | TextFormatFlags.Right;
                case ContentAlignment.MiddleLeft:   return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight:  return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                case ContentAlignment.BottomLeft:   return TextFormatFlags.Bottom         | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom         | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight:  return TextFormatFlags.Bottom         | TextFormatFlags.Right;
                default: return TextFormatFlags.Default;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            var textFormatFlags = ToTextFormatFlags(TextAlign);
            if (!Enabled && DisabledColor != Color.Empty) {
                e.Graphics.Clear(Parent?.BackColor ?? Color.Empty);
                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, DisabledColor, textFormatFlags);
            }
            else
                base.OnPaint(e);
        }

        public Color DisabledColor { get; set; } = Color.Empty;
        private DarkModeControlContext<DarkModeLabel> DarkModeContext { get; set; }
    }
}
