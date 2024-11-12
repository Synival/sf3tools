using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Models.MPD.TextureChunk;
using SF3.MPDEditor.Extensions;
using SF3.X1_Editor.Controls;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.MPD_Editor.Forms {

    public partial class frmMPDEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.3";

        public new IMPD_FileEditor FileEditor => base.FileEditor as IMPD_FileEditor;

        public frmMPDEditor() {
            InitializeComponent();

            // Gather all tables in our battleEditors.
            // TODO: InitializeEditor() should do just do this recursively
            var textureChunkEditorControls = new List<TextureChunkControl>() {
                textureChunkControl1,
                textureChunkControl2,
                textureChunkControl3,
                textureChunkControl4,
            };
            var textureChunkOLVs = textureChunkEditorControls.SelectMany(x => x.GetAllObjectsOfTypeInFields<ObjectListView>(false)).ToList();

            // Synchronize the tabs in the battle editors
            void tabSyncFunc(object sender, TabControlEventArgs e) {
                foreach (var tcec in textureChunkEditorControls)
                    tcec.Tabs.SelectedIndex = e.TabPageIndex;
            };
            foreach (var tcec in textureChunkEditorControls) {
                tcec.Tabs.Selected += tabSyncFunc;
                tcec.OLVTextures.ItemSelectionChanged += (s, e) => OnTextureChanged(s, e, tcec);
            }

            InitializeEditor(menuStrip2, textureChunkOLVs);

            // Scenario is currently irrelevant.
            // TODO: fetch by name, not by index!!
            MenuStrip.Items.Remove(MenuStrip.Items[2]);
        }

        private void OnTextureChanged(object sender, ListViewItemSelectionChangedEventArgs e, TextureChunkControl tcec) {
            var item = (OLVListItem) e.Item;
            var texture = (Texture) item.RowObject;
            tcec.TextureControl.TextureImage = texture.GetImage();
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
                var tcec = TabPage.Controls[0] as TextureChunkControl;
                return tcec.Tabs.PopulateTabs(new List<IPopulateTabConfig>() {
                    new PopulateOLVTabConfig(tcec.TabHeader,   tcec.OLVHeader,   TextureChunk.HeaderTable),
                    new PopulateOLVTabConfig(tcec.TabTextures, tcec.OLVTextures, TextureChunk.TextureTable),
                });
            }
        }

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            var populateResult = tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabHeader,                olvHeader,                FileEditor.Header),
                new PopulateOLVTabConfig(tabChunkHeader,           olvChunkHeader,           FileEditor.ChunkHeader),
                new PopulateOLVTabConfig(tabTileSurfaceCharacters, olvTileSurfaceCharacters, FileEditor.TileSurfaceCharacterRows),
                new PopulateOLVTabConfig(tabBattleMap,             null,                     FileEditor.TileSurfaceCharacterRows),
                new PopulateOLVTabConfig(tabTileHeightmap,         olvTileHeightmap,         FileEditor.TileHeightmapRows),
                new PopulateOLVTabConfig(tabTileHeights,           olvTileHeights,           FileEditor.TileHeightRows),
                new PopulateOLVTabConfig(tabTileTerrain,           olvTileTerrain,           FileEditor.TileTerrainRows),
                new PopulateOLVTabConfig(tabTileItems,             olvTileItems,             FileEditor.TileItemRows),

                new PopulateTextureChunkTabConfig(tabTextures1, FileEditor.TextureChunks?[0]),
                new PopulateTextureChunkTabConfig(tabTextures2, FileEditor.TextureChunks?[1]),
                new PopulateTextureChunkTabConfig(tabTextures3, FileEditor.TextureChunks?[2]),
                new PopulateTextureChunkTabConfig(tabTextures4, FileEditor.TextureChunks?[3]),
            });

            if (FileEditor.TileSurfaceCharacterRows != null) {
                this.battleMapControl1.UpdateTextures(FileEditor.TileSurfaceCharacterRows.TextureData, FileEditor.TextureChunks);
            }

            return populateResult;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
