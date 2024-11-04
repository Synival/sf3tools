using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.IconPointerEditor.Forms {
    public partial class frmIconPointerEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.11";

        public new IIconPointerFileEditor FileEditor => base.FileEditor as IIconPointerFileEditor;

        public frmIconPointerEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter
            => "SF3 Data (X011.BIN;X021.BIN;X026.BIN)|X011.BIN;X021.BIN;X026.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new IconPointerFileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabSpellIcons, olvSpellIcons, FileEditor.SpellIconTable),
                new PopulateOLVTabConfig(tabItemIcons, olvItemIcons, FileEditor.ItemIconTable)
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
