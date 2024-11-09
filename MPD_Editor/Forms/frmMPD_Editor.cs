using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.MPD_Editor.Forms {

    public partial class frmMPDEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.3";

        public new IMPD_FileEditor FileEditor => base.FileEditor as IMPD_FileEditor;

        public frmMPDEditor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);

            // Scenario is currently irrelevant.
            // TODO: fetch by name, not by index!!
            MenuStrip.Items.Remove(MenuStrip.Items[2]);
        }

        protected override string FileDialogFilter
            => "SF3 Data (*_decompressed.BIN)|*_decompressed.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new MPD_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabPage1, objectListView1, FileEditor.TileRows),
                new PopulateOLVTabConfig(tabItems, olvItems, FileEditor.ItemTileRows),
            });
        }

        private bool initialise() {
/*
            if (!ItemList.loadItemList()) {
                _ = MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            if (!PresetList.loadPresetList()) {
                _ = MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }
*/
            olvItems.ClearObjects();
            objectListView1.ClearObjects();

/*
            olvItems.AddObjects(PresetList.getPresetList());
            objectListView1.AddObjects(ItemList.getItemList());
*/

            return true;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
