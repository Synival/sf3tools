using System.Drawing;
using SF3.Win.Controls;

namespace SF3.Win.DarkMode {
    public class DarkModeTabControlContext<T> : DarkModeControlContext<T> where T : EnhancedTabControl {
        public DarkModeTabControlContext(T control) : base(control) {}

        protected override void OnInit() {
            base.OnInit();
            OriginalHighlightedBackColor = Control.HighlightedBackColor;
            OriginalHighlightedForeColor = Control.HighlightedForeColor;
        }

        protected override void OnDarkModeEnabled() {
            base.OnDarkModeEnabled();
            Control.CustomPaint = true;
            Control.HighlightedBackColor = DarkModeColors.HighlightedBackColor;
            Control.HighlightedForeColor = DarkModeColors.HighlightedForeColor;
        }

        protected override void OnDarkModeDisabled() {
            base.OnDarkModeDisabled();
            Control.CustomPaint = false;
            Control.HighlightedBackColor = OriginalHighlightedBackColor;
            Control.HighlightedForeColor = OriginalHighlightedForeColor;
        }

        public Color OriginalHighlightedBackColor { get; private set; }
        public Color OriginalHighlightedForeColor { get; private set; }
    }
}
