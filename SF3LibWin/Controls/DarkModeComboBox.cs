using System;
using System.Drawing;
using System.Windows.Forms;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    public class DarkModeComboBox : ComboBox {
        public DarkModeComboBox() {
            DrawItem += DrawDarkModeItem;
            DrawMode = DrawMode.OwnerDrawFixed;
            EnabledChanged += (s, e) => DropDownStyle = Enabled ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
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

        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x000F /* WM_PAINT */ && Text != "" && Handle != IntPtr.Zero && SelectionLength > 0 && !Focused)
                SelectionLength = 0;

            base.WndProc(ref m);
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
