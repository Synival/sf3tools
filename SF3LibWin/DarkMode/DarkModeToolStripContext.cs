using System.Windows.Forms;

namespace SF3.Win.DarkMode {
    /// <summary>
    /// Dark mode context for ToolStrip or MenuStrip.
    /// </summary>
    /// <typeparam name="T">Underlying type of ToolStrip.</typeparam>
    public class DarkModeToolStripContext<T> : DarkModeControlContext<T> where T : ToolStrip {
        public DarkModeToolStripContext(T control, bool drawBorder) : base(control) {
            DarkModeRenderer = new DarkModeToolStripRenderer(drawBorder: drawBorder);
        }

        protected override void OnInit() {
            base.OnInit();
            OriginalRenderer = Control.Renderer;
        }

        protected override void OnDarkModeEnabled() {
            base.OnDarkModeEnabled();
            Control.Renderer = DarkModeRenderer;
        }

        protected override void OnDarkModeDisabled() {
            base.OnDarkModeDisabled();
            Control.Renderer = OriginalRenderer;
        }

        private DarkModeToolStripRenderer DarkModeRenderer { get; set; }
        public ToolStripRenderer OriginalRenderer { get; private set; }
    }
}
