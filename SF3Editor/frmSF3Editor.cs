using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SF3.Editors.MPD;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;
using SF3.Win.EditorControls;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        const string c_mpdPath = "";

        public frmSF3Editor() {
            SuspendLayout();
            InitializeComponent();
            RegisterNamedValues();

            var mpdEditor = MPD_Editor.Create(
                new ByteEditor(File.ReadAllBytes(c_mpdPath)),
                new NameGetterContext(ScenarioType.Scenario1),
                ScenarioType.Scenario1);

            var table = mpdEditor.Palettes[0];
            var tableEditorControl = new TableEditorControl(table, mpdEditor.NameGetterContext);

            var olv = tableEditorControl.Create();
            olv.Dock = DockStyle.Fill;
            masterEditorControl1.Controls.Add(olv);

            ResumeLayout();
        }
    }
}
