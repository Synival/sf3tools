using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Models.MPD;
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
            => "SF3 Data (*.MPD)|*.MPD|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new MPD_FileEditor(Scenario);

        private class PopulateTextureChunkTabConfig : IPopulateTabConfig {
            public PopulateTextureChunkTabConfig(TabPage tabPage, TextureChunk textureChunk) {
                TabPage = tabPage;
                TextureChunk = textureChunk;
            }

            public TabPage TabPage { get; }
            public TextureChunk TextureChunk { get; }

            public bool CanPopulate => TextureChunk != null;

            public bool Populate() {
                // TODO: more tables!
                return true;
#if false
                var tcec = TabPage.Controls[0] as TextureChunkEditorControl;
                return bec.Tabs.PopulateTabs(new List<IPopulateTabConfig>() {
                    new PopulateOLVTabConfig(bec.TabHeader,           bec.OLVHeader,           BattleTable.HeaderTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab1,         bec.OLVSlotTab1,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab2,         bec.OLVSlotTab2,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab3,         bec.OLVSlotTab3,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab4,         bec.OLVSlotTab4,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSpawnZones,       bec.OLVSpawnZones,       BattleTable.SpawnZoneTable),
                    new PopulateOLVTabConfig(bec.TabAITargetPosition, bec.OLVAITargetPosition, BattleTable.AITable),
                    new PopulateOLVTabConfig(bec.TabScriptedMovement, bec.OLVScriptedMovement, BattleTable.CustomMovementTable)
                });
#endif
            }
        }

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabHeader,                olvHeader,                FileEditor.Header),
                new PopulateOLVTabConfig(tabTileSurfaceCharacters, olvTileSurfaceCharacters, FileEditor.TileSurfaceCharacterRows),
                new PopulateOLVTabConfig(tabTileHeightmap,         olvTileHeightmap,         FileEditor.TileHeightmapRows),
                new PopulateOLVTabConfig(tabTileHeights,           olvTileHeights,           FileEditor.TileHeightRows),
                new PopulateOLVTabConfig(tabTileTerrain,           olvTileTerrain,           FileEditor.TileTerrainRows),
                new PopulateOLVTabConfig(tabTileItems,             olvTileItems,             FileEditor.TileItemRows),

                new PopulateTextureChunkTabConfig(tabTextures1, FileEditor.TextureChunks?[0]),
                new PopulateTextureChunkTabConfig(tabTextures2, FileEditor.TextureChunks?[1]),
                new PopulateTextureChunkTabConfig(tabTextures3, FileEditor.TextureChunks?[2]),
                new PopulateTextureChunkTabConfig(tabTextures4, FileEditor.TextureChunks?[3]),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
