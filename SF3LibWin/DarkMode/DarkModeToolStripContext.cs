using System.Windows.Forms;

namespace SF3.Win.DarkMode {
    /// <summary>
    /// Dark mode context for ToolStrip or MenuStrip.
    /// </summary>
    /// <typeparam name="T">Underlying type of ToolStrip.</typeparam>
    public class DarkModeToolStripContext<T> : DarkModeControlContext<T> where T : ToolStrip {
        private static DarkModeToolStripRenderer s_darkModeRenderer = new DarkModeToolStripRenderer();

        public DarkModeToolStripContext(T control) : base(control) {}

        protected override void OnInit() {
            base.OnInit();
            OriginalRenderer = Control.Renderer;
        }

        protected override void OnDarkModeEnabled() {
            base.OnDarkModeEnabled();
            Control.Renderer = s_darkModeRenderer;
        }

        protected override void OnDarkModeDisabled() {
            base.OnDarkModeDisabled();
            Control.Renderer = OriginalRenderer;
        }

        public ToolStripRenderer OriginalRenderer { get; private set; }
    }
}
