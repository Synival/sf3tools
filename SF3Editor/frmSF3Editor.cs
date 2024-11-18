using System.IO;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Editors.IconPointer;
using SF3.Editors.MPD;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;
using SF3.Win.EditorControls;
using SF3.Win.EditorControls.MPD;
using static SF3.Win.Extensions.ObjectListViewExtensions;

namespace SF3Editor {
    public partial class frmSF3Editor : Form {
        private const ScenarioType c_scenario = ScenarioType.Scenario1;
        private const string c_discPath = "D:\\"; // Change to wherever your files are located
        private readonly INameGetterContext c_nameGetterContext = new NameGetterContext(c_scenario);

        public frmSF3Editor() {
            RegisterNamedValues();

            SuspendLayout();
            InitializeComponent();

            var controlContainer = new EditorControlContainer("Test Container");
            var controlContainerControl = controlContainer.Create();

            var iconPointerEditorBinsToLoad = new string[] {"X011.BIN", "X021.BIN"};
            foreach (var bin in iconPointerEditorBinsToLoad) {
                var iconPointerEditor = IconPointerEditor.Create(new ByteEditor(File.ReadAllBytes(c_discPath + bin)), c_nameGetterContext, c_scenario);
                _ = controlContainer.CreateChild(new IconPointerEditorControl(bin, iconPointerEditor));
            }

            var mpdsToLoad = new string[] {"BTL02.MPD", "BTL03.MPD", "BTL04A.MPD"};
            foreach (var mpd in mpdsToLoad) {
                var mpdEditor = MPD_Editor.Create(new ByteEditor(File.ReadAllBytes(c_discPath + mpd)), c_nameGetterContext, c_scenario);
                _ = controlContainer.CreateChild(new MPD_EditorControl(mpd, mpdEditor));
            }

            controlContainerControl.Dock = DockStyle.Fill;
            Controls.Add(controlContainerControl);

            ResumeLayout();
        }
    }
}
