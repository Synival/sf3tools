using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace SF3.Win.Controls {
    /// <summary>
    /// Renderer that will display hex values will a special font.
    /// </summary>
    public class EnhancedOLVRenderer : BaseRenderer {
        public static readonly EnhancedOLVRenderer Instance = new EnhancedOLVRenderer();

        private static readonly Color _readOnlyColor = Color.FromArgb(96, 96, 96);
        private static readonly Font _defaultFont = Control.DefaultFont;
        private static readonly Font _hexFont = new Font("Courier New", Control.DefaultFont.Size);

        private Font _currentRenderFont = Control.DefaultFont;
        private Color _currentRenderColor = Color.Black;

        public static Font GetCellFont(string formatString)
            => formatString.StartsWith("{0:X") ? _hexFont : _defaultFont;

        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object rowObject) {
            var lvc = ((ObjectListView) e.Item.ListView).GetColumn(e.ColumnIndex);
            _ = lvc.AspectGetter(rowObject);

            // If an AspectToStringConverter was supplied, this is probably a named value. Just use the default font.
            var formatString = (lvc.AspectToStringConverter == null) ? (lvc.AspectToStringFormat ?? "") : "";
            _currentRenderFont = GetCellFont(formatString);
            _currentRenderColor = lvc.IsEditable ? Color.Black : _readOnlyColor;

            return base.RenderSubItem(e, g, cellBounds, rowObject);
        }

        protected override Color GetForegroundColor()
            => _currentRenderColor;

        public override void Render(Graphics g, Rectangle r) {
            Font = _currentRenderFont;
            base.Render(g, r);
        }
    }
}
