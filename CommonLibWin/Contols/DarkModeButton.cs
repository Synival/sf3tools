using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    public class DarkModeButton : Button {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeButton>(this);

                OriginalFlatStyle = FlatStyle;
                OriginalFlatAppearanceBorderColor = FlatAppearance.BorderColor;
                OriginalDisabledForeColor = DisabledForeColor;

                DarkModeContext.EnabledChanged += (s, e) => {
                    if (DarkModeContext.Enabled) {
                        FlatStyle = FlatStyle.Flat;
                        FlatAppearance.BorderColor = DarkModeColors.BorderColor;
                        DisabledForeColor = DarkModeColors.DisabledColor;
                    }
                    else {
                        FlatStyle = OriginalFlatStyle;
                        FlatAppearance.BorderColor = OriginalFlatAppearanceBorderColor;
                        DisabledForeColor = OriginalDisabledForeColor;
                    }
                };

                DarkModeContext.Init();
                if (DarkModeContext.Enabled) {
                    DarkModeContext.OriginalBackColor = SystemColors.ControlLightLight;
                    DarkModeContext.OriginalForeColor = SystemColors.ControlText;
                }
            }
        }

        private static readonly StringFormat c_stringFormat = new StringFormat() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (FlatStyle == FlatStyle.Flat && !Enabled) {
                using (var brush = new SolidBrush(BackColor)) {
                    var padding = FlatAppearance.BorderSize;
                    var padding2 = padding * 2;
                    var innerRect = new Rectangle(padding, padding, Width - padding2, Height - padding2);
                    e.Graphics.FillRectangle(brush, innerRect);
                }
                using (var brush = new SolidBrush(DisabledForeColor))
                    e.Graphics.DrawString(Text, Font, brush, new Rectangle(0, 0, Width, Height), c_stringFormat);
            }
            else {
                base.OnPaint(e);
            }
        }

        private DarkModeControlContext<DarkModeButton> DarkModeContext { get; set; }

        [Browsable(true)]
        public Color DisabledForeColor { get; set; } = Color.Empty;

        public FlatStyle OriginalFlatStyle { get; private set; }
        public Color OriginalFlatAppearanceBorderColor { get; private set; }
        public Color OriginalDisabledForeColor { get; private set; }
    }
}

