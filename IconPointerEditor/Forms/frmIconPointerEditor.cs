using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.ModelLoaders;
using static SF3.Win.Extensions.TabControlExtensions;
using SF3.NamedValues;
using SF3.Models.Files;
using SF3.Models.Files.IconPointer;

namespace SF3.IconPointerEditor.Forms {
    public partial class frmIconPointerEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.13";

        public IIconPointerFile File => base.FileLoader.Model as IIconPointerFile;

        public frmIconPointerEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 Data (X011.BIN;X021.BIN;X026.BIN)|X011.BIN;X021.BIN;X026.BIN|" + base.FileDialogFilter;

        protected override IBaseFile MakeEditor(IModelFileLoader loader)
            => IconPointerFile.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpellIcons, olvSpellIcons, File.SpellIconTable),
                new PopulateOLVTabConfig(tabItemIcons, olvItemIcons, File.ItemIconTable)
            });
        }
    }
}
