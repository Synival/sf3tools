using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.Editors;
using SF3.Editors.MPD;
using SF3.Loaders;
using SF3.Models.MPD.TextureChunk;
using SF3.MPDEditor.Extensions;
using SF3.NamedValues;
using SF3.X1_Editor.Controls;
using static SF3.Win.Extensions.TabControlExtensions;

namespace SF3.MPD_Editor.Forms {

    public partial class frmMPDEditor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.3";

        public IMPD_Editor Editor => base.FileLoader.Editor as IMPD_Editor;

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
        }

        private void OnTextureChanged(object sender, ListViewItemSelectionChangedEventArgs e, TextureChunkControl tcec) {
            var item = (OLVListItem) e.Item;
            var texture = (Texture) item.RowObject;
            tcec.TextureControl.TextureImage = texture.CreateBitmap();
        }

        protected override string FileDialogFilter
            => "SF3 Data (*.MPD)|*.MPD|" + base.FileDialogFilter;

        protected override IBaseEditor MakeEditor(IFileLoader loader)
            => Editors.MPD.MPD_Editor.Create(loader.RawEditor, new NameGetterContext(Scenario), Scenario);

        private class PopulateTextureChunkTabConfig : IPopulateTabConfig {
            public PopulateTextureChunkTabConfig(TabPage tabPage, TextureChunkEditor textureChunk) {
                TabPage = tabPage;
                TextureChunk = textureChunk;
            }

            public TabPage TabPage { get; }
            public TextureChunkEditor TextureChunk { get; }

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
                new PopulateOLVTabConfig(tabHeader,                olvHeader,                Editor.Header),
                new PopulateOLVTabConfig(tabPalette1,              olvPalette1,              Editor.Palettes?[0]),
                new PopulateOLVTabConfig(tabPalette2,              olvPalette2,              Editor.Palettes?[1]),
                new PopulateOLVTabConfig(tabPalette3,              olvPalette3,              Editor.Palettes?[2]),
                new PopulateOLVTabConfig(tabChunkHeader,           olvChunkHeader,           Editor.ChunkHeader),
                new PopulateOLVTabConfig(tabTileSurfaceCharacters, olvTileSurfaceCharacters, Editor.TileSurfaceCharacterRows),
                new PopulateOLVTabConfig(tabTileHeightmap,         olvTileHeightmap,         Editor.TileHeightmapRows),
                new PopulateOLVTabConfig(tabTileHeights,           olvTileHeights,           Editor.TileHeightRows),
                new PopulateOLVTabConfig(tabTileTerrain,           olvTileTerrain,           Editor.TileTerrainRows),
                new PopulateOLVTabConfig(tabTileItems,             olvTileItems,             Editor.TileItemRows),

                new PopulateTextureChunkTabConfig(tabChunk6, Editor.TextureChunks?[0]),
                new PopulateTextureChunkTabConfig(tabChunk7, Editor.TextureChunks?[1]),
                new PopulateTextureChunkTabConfig(tabChunk8, Editor.TextureChunks?[2]),
                new PopulateTextureChunkTabConfig(tabChunk9, Editor.TextureChunks?[3]),

                // TODO: just a true/false predicate would work here
                new PopulateOLVTabConfig(tabSurfaceMap, null, Editor.Header), // Should always be present
            });

            if (Editor.TileSurfaceCharacterRows != null)
                this.surfaceMapControl.UpdateTextures(Editor.TileSurfaceCharacterRows.TextureData, Editor.TextureChunks);
            else
                this.surfaceMapControl.UpdateTextures(null, null);

            return populateResult;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);
    }
}
