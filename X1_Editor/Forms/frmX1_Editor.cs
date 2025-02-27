using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Forms;
using SF3.Win.Views;
using SF3.Win.Views.X1;

namespace SF3.X1_Editor.Forms {
    public partial class frmX1_Editor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.39 (2025-02-26 dev)";

        private bool _isBTL99 = false;

        public bool IsBTL99 {
            get => _isBTL99;
            set {
                if (_isBTL99 != value) {
                    _isBTL99 = value;
                    tsmiScenario_BTL99.Checked = _isBTL99;
                }
            }
        }

        public IX1_File File => ModelLoader.Model as IX1_File;

        public frmX1_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => (IsBTL99 ? "SF3 Data (X1BTL99.BIN)|X1BTL99.BIN|" : "SF3 Data (X1*.BIN)|X1*.BIN|") + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));
            return X1_File.Create(loader.ByteData, nameGetters[Scenario], Scenario, IsBTL99);
        }

        protected override IView CreateView(IModelFileLoader loader, IBaseFile model)
            => new X1_View(loader.Filename, (X1_File) model);

        private void tsmiHelp_About_Click(object sender, EventArgs e) {
            var versionInfo = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);
            var legalCopyright = versionInfo.LegalCopyright;

            MessageBox.Show(
                VersionTitle + "\n\n" +
                legalCopyright + "\n\n" +
                "About " + BaseTitle
            );
        }

        private void tsmiScenario_BTL99_Click(object sender, EventArgs e) => IsBTL99 = !IsBTL99;
    }
}
