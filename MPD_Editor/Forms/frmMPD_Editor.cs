using System.Windows.Forms;
using SF3.Win.Forms;
using SF3.Editors;
using SF3.Editors.MPD;
using SF3.Loaders;
using SF3.NamedValues;
using SF3.Win.EditorControls.MPD;

namespace SF3.MPD_Editor.Forms {

    public partial class frmMPDEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.3";

        public IMPD_Editor Editor => base.FileLoader.Editor as IMPD_Editor;
        public MPD_EditorControl EditorControl { get; private set; } = null;
        public Control Control { get; private set; } = null;

        public frmMPDEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 Data (*.MPD)|*.MPD|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IFileLoader loader)
            => Editors.MPD.MPD_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            SuspendLayout();
            EditorControl = new MPD_EditorControl(FileLoader.Filename, Editor);
            Control = EditorControl.Create();
            Control.Dock = DockStyle.Fill;
            Controls.Add(Control);
            Control.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)
            ResumeLayout();

            return true;
        }

        protected override bool OnClose() {
            bool wasFocused = ContainsFocus;

            SuspendLayout();
            if (Control != null) {
                Controls.Remove(Control);
                Control.Dispose();
                Control = null;
            }
            if (EditorControl != null) {
                EditorControl.Dispose();
                EditorControl = null;
            }
            ResumeLayout();

            if (!base.OnClose())
                return false;

            if (wasFocused && !ContainsFocus)
                Focus();

            return true;
        }
    }
}
