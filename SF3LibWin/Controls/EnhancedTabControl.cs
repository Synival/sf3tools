using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    /// <summary>
    /// TabControl with enhanced drawing.
    /// </summary>
    public class EnhancedTabControl : TabControl {
        public EnhancedTabControl(TabAlignment tabAlignment = TabAlignment.Top) {
            SuspendLayout();

            Alignment = tabAlignment;
            if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right) {
                SizeMode = TabSizeMode.Fixed;
                ItemSize = new Size(40, 100);
            }
            DrawMode = TabDrawMode.OwnerDrawFixed;
            DrawItem += EnhancedDraw;

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            ResumeLayout();
        }

        private void EnhancedDraw(object sender, DrawItemEventArgs e) {
            var tabPage = TabPages[e.Index];
            var foreBrush = new SolidBrush(tabPage.ForeColor);
            var backBrush = new SolidBrush(e.State == DrawItemState.Selected ? Color.FromArgb(0xF8, 0xF8, 0xF8) : tabPage.BackColor);
            var bounds = GetTabRect(e.Index);
            StringFormat stringFlags = new StringFormat {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Draw the background, the text, and a focus rectangle.
            // TODO: there is quite a lot of flickering here that needs to be removed!!
            e.Graphics.FillRectangle(backBrush, bounds);
            e.Graphics.DrawString(tabPage.Text, e.Font, foreBrush, bounds, stringFlags);
            if (e.State == DrawItemState.Selected)
                e.DrawFocusRectangle();
        }
    }
}
