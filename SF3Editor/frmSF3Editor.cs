using System.IO;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Files.IconPointer;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;
using SF3.Win.Views;
using SF3.Win.Views.MPD;
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

            var controlContainer = new TabView("Test Container");
            var controlContainerControl = controlContainer.Create();

            var iconPointerEditorBinsToLoad = new string[] {"X011.BIN", "X021.BIN"};
            foreach (var bin in iconPointerEditorBinsToLoad) {
                var iconPointerFile = IconPointerFile.Create(new ByteData(File.ReadAllBytes(c_discPath + bin)), c_nameGetterContext, c_scenario);
                _ = controlContainer.CreateChild(new IconPointerView(bin, iconPointerFile));
            }

            var mpdsToLoad = new string[] {"BTL02.MPD", "BTL03.MPD", "BTL04A.MPD"};
            foreach (var mpd in mpdsToLoad) {
                var mpdFile = MPD_File.Create(new ByteData(File.ReadAllBytes(c_discPath + mpd)), c_nameGetterContext, c_scenario);
                _ = controlContainer.CreateChild(new MPD_View(mpd, mpdFile));
            }

            controlContainerControl.Dock = DockStyle.Fill;
            Controls.Add(controlContainerControl);

            ResumeLayout();
        }
    }
}
