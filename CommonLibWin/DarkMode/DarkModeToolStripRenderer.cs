using System.Drawing;
using System.Windows.Forms;

namespace CommonLib.Win.DarkMode {
    /// <summary>
    /// Dark mode renderer for ToolStrip and MenuStrip controls.
    /// </summary>
    public class DarkModeToolStripRenderer : ToolStripProfessionalRenderer {
        public bool DrawBorder { get; }

        public DarkModeToolStripRenderer(bool drawBorder) : base(new DarkModeColorTable()) {
            DrawBorder = drawBorder;
            RoundedEdges = false;
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
            e.TextColor = e.Item.Selected ? DarkModeColors.HighlightedForeColor : DarkModeColors.ForeColor;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
            if (DrawBorder)
                base.OnRenderToolStripBorder(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.Item is not ToolStripSeparator)
            {
                base.OnRenderSeparator(e);
                return;
            }

            var width = e.Item.Size.Width;
            var y = e.Item.Size.Height / 2;
            using (Pen pen = new Pen(DarkModeColors.BorderColor))
                e.Graphics.DrawLine(pen, 32, y, width - 1, y);
        }
    }
}