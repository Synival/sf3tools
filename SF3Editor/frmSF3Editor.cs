using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.IconPointer;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X1;
using SF3.NamedValues;
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

            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            var controlContainer = new TabView("Test Container");
            var controlContainerControl = controlContainer.Create();

            var x1sToLoad = new List<KeyValuePair<ScenarioType, string>> {
                new KeyValuePair<ScenarioType, string>(ScenarioType.Scenario1, "D://X1ASCASL.BIN"),
                new KeyValuePair<ScenarioType, string>(ScenarioType.Scenario1, "D://X1BTL123.BIN"),
                new KeyValuePair<ScenarioType, string>(ScenarioType.Scenario3, "F://X1ASBRK3.BIN"),
                new KeyValuePair<ScenarioType, string>(ScenarioType.Scenario3, "F://X1BTL323.BIN"),
            };
            foreach (var x1 in x1sToLoad) {
                var scenario = x1.Key;
                var fullPath = x1.Value;
                var x1File = X1_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(fullPath))), nameGetters[scenario], scenario, false);
                controlContainer.CreateChild(new X1_View(Path.GetFileName(fullPath), x1File));
            }

            var iconPointerEditorBinsToLoad = new string[] {"X011.BIN", "X021.BIN"};
            foreach (var bin in iconPointerEditorBinsToLoad) {
                var iconPointerFile = IconPointerFile.Create(new ByteData(new ByteArray(File.ReadAllBytes(c_discPath + bin))), c_nameGetterContext, c_scenario);
                controlContainer.CreateChild(new IconPointerView(bin, iconPointerFile));
            }

            var mpdsToLoad = new string[] {"BTL02.MPD", "BTL03.MPD", "BTL04A.MPD"};
            foreach (var mpd in mpdsToLoad) {
                var mpdFile = MPD_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(c_discPath + mpd))), nameGetters);
                controlContainer.CreateChild(new MPD_View(mpd, mpdFile));
            }

            controlContainerControl.Dock = DockStyle.Fill;
            Controls.Add(controlContainerControl);

            ResumeLayout();
        }
    }
}
