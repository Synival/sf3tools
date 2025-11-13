using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    public class DarkModeComboBox : ComboBox {
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateSolidBrush(int color);

        public DarkModeComboBox() {
            DrawItem += DrawDarkModeItem;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeComboBox>(this);
                OriginalFlatStyle = FlatStyle;
                DarkModeContext.EnabledChanged += (s, e) => {
                    FlatStyle = DarkModeContext.Enabled ? FlatStyle.Flat : OriginalFlatStyle;
                };
                DarkModeContext.Init();
            }
        }

        private static IntPtr _darkModeDisabledBackColorBrush = CreateSolidBrush(ColorTranslator.ToWin32(DarkModeColors.BackColor));
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case 0x000F /* WM_PAINT */:
                    if (Text != "" && Handle != IntPtr.Zero && SelectionLength > 0 && !Focused)
                        SelectionLength = 0;

                    base.WndProc(ref m);

                    if (DarkModeContext.Enabled) {
                        using (var g = Graphics.FromHwnd(Handle)) {
                            using (var brush = new SolidBrush(DarkModeColors.BackColor))
                                g.FillRectangle(brush, 0, 0, ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth, ClientRectangle.Height);
                            using (var pen = new Pen(DarkModeColors.BorderColor))
                                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                        }
                    }
                    break;

                case 0x0138 /* WM_CTLCOLORSTATIC */: {
                    if (DarkModeContext.Enabled)
                        m.Result = _darkModeDisabledBackColorBrush;
                    break;
                }

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void DrawDarkModeItem(object sender, DrawItemEventArgs e) {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;

            string label = (e.Index >= 0) ? GetItemText(Items[e.Index]) : string.Empty;

            var backColor = BackColor;
            var foreColor = ForeColor;

            if (e.State.HasFlag(DrawItemState.Selected)) {
                backColor = DarkModeContext.Enabled ? DarkModeColors.HighlightedBackColor : SystemColors.Highlight;
                foreColor = DarkModeContext.Enabled ? DarkModeColors.HighlightedForeColor : SystemColors.HighlightText;
            }
            else if (e.State.HasFlag(DrawItemState.Disabled)) {
                if (!DarkModeContext.Enabled)
                    backColor = SystemColors.Control;
                foreColor = DarkModeContext.Enabled ? DarkModeColors.DisabledColor : SystemColors.GrayText;
                rect = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2);
            }

            using (var brush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(brush, rect);
            using (var brush = new SolidBrush(foreColor))
                g.DrawString(label, e.Font, brush, rect);

            if (!e.State.HasFlag(DrawItemState.NoFocusRect))
                e.DrawFocusRectangle();
        }

        public FlatStyle OriginalFlatStyle { get; private set; }

        private DarkModeControlContext<DarkModeComboBox> DarkModeContext { get; set; }
    }
}
