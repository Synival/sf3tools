using SF3.ModelLoaders;
using SF3.Models.Files;
using SF3.Models.Files.IconPointer;
using SF3.NamedValues;
using SF3.Win.Forms;
using SF3.Win.Views;
using SF3.Win.Views.MPD;

namespace SF3.IconPointerEditor.Forms {
    public partial class frmIconPointerEditor : EditorFormNew {
        // Used to display version in the application
        protected override string Version => "0.13";

        public IIconPointerFile File => ModelLoader.Model as IIconPointerFile;

        public frmIconPointerEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 Data (X011.BIN;X021.BIN;X026.BIN)|X011.BIN;X021.BIN;X026.BIN|" + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => IconPointerFile.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario);

        protected override IView CreateView(IModelFileLoader loader, IBaseFile model)
            => new IconPointerView(loader.Filename, (IconPointerFile) model);
    }
}
