using System.IO;
using System.Windows.Forms;
using SF3.Editors.MPD;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;
using SF3.Win.EditorControls;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        const string c_mpdPath = "";

        public frmSF3Editor() {
            RegisterNamedValues();

            SuspendLayout();
            InitializeComponent();

            var containerControl = new EditorControlContainer();

            var control = containerControl.Create();
            control.Dock = DockStyle.Fill;
            Controls.Add(control);

#if false
            var mpdEditor = MPD_Editor.Create(
                new ByteEditor(File.ReadAllBytes(c_mpdPath)),
                new NameGetterContext(ScenarioType.Scenario1),
                ScenarioType.Scenario1);
            var table = mpdEditor.TextureChunks[0].HeaderTable;
            var tableEditorControl = new TableEditorControl(table, mpdEditor.NameGetterContext);
#endif

            ResumeLayout();
        }
    }
}
