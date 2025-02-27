using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X012;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Forms;
using SF3.Win.Views;
using SF3.Win.Views.X012;

namespace SF3.X012_Editor.Forms {
    public partial class frmX012_Editor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.1";

        public IX012_File File => ModelLoader.Model as IX012_File;

        public frmX012_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 X012 File|X012.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));
            return X012_File.Create(loader.ByteData, nameGetters[Scenario], Scenario);
        }

        protected override IView CreateView(IModelFileLoader loader, IBaseFile model)
            => new X012_View(loader.Filename, (X012_File) model);

        private void tsmiHelp_About_Click(object sender, EventArgs e) {
            var versionInfo = FileVersionInfo.GetVersionInfo(Environment.ProcessPath);
            var legalCopyright = versionInfo.LegalCopyright;

            MessageBox.Show(
                VersionTitle + "\n\n" +
                legalCopyright + "\n\n" +
                "About " + BaseTitle
            );
        }
    }
}
