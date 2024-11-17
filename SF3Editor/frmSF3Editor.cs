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

            var mpdEditor = MPD_Editor.Create(
                new ByteEditor(File.ReadAllBytes(c_mpdPath)),
                new NameGetterContext(ScenarioType.Scenario1),
                ScenarioType.Scenario1);

            var table = mpdEditor.ChunkHeader;
            var tableEditorControl = new TableEditorControl(table, mpdEditor.NameGetterContext);

            var olv = tableEditorControl.Create();
            olv.Location = new Point(masterEditorControl1.Padding.Left, masterEditorControl1.Padding.Top);
            olv.Size = new Size(masterEditorControl1.Size.Width - masterEditorControl1.Padding.Horizontal, masterEditorControl1.Size.Height - masterEditorControl1.Padding.Vertical);

            masterEditorControl1.Controls.Add(olv);
            ResumeLayout();

        }
    }
}
