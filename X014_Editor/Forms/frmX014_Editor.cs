using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.X014;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win.Forms;
using SF3.Win.Views;
using SF3.Win.Views.X014;

namespace SF3.X014_Editor.Forms {
    public partial class frmX014_Editor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.1";

        public IX014_File File => ModelLoader.Model as IX014_File;

        public frmX014_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 X014 File|X014.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));
            return X014_File.Create(loader.ByteData, nameGetters[Scenario], Scenario);
        }

        protected override IView CreateView(IModelFileLoader loader, IBaseFile model)
            => new X014_View(loader.Filename, (X014_File) model);

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
