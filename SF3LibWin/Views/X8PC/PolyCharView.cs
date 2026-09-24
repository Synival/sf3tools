using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PolyCharView : TabView {
        public PolyCharView(string name, PolyChar model, INameGetterContext ngc, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            NameGetterContext = ngc;

            AnimationViewer = new PCAnimationView("Animation Viewer", model);
            TextureSheetView = new TextureView("Texture Atlas", model?.TextureAtlas, imageScale: 2.0f);
            PaletteView     = new TextureView("Palette", model?.Palette, imageScale: 16.00f);
            ChunkDefView    = new TableView("Chunks", model?.Header?.ChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            ExtraAnimChunkDefView = new TableView("Extra Anim. Chunks", model?.Header?.ExtraAnimChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            TexturesView    = new PCTexChunkView("Textures", model, NameGetterContext);
            ModelsView      = new PCModelChunkView("Models", model, NameGetterContext);
            AnimationsView  = new PCAnimationChunkView("Animations", model, NameGetterContext);

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(AnimationViewer);
            CreateChild(TextureSheetView);
            CreateChild(PaletteView);
            CreateChild(ChunkDefView);
            CreateChild(ExtraAnimChunkDefView);
            CreateChild(TexturesView);
            CreateChild(ModelsView);
            CreateChild(AnimationsView);

            foreach (var tabObj in TabControl.TabPages) {
                var tab = tabObj as TabPage;
                if (tab != null) {
                    if (tab.Text == "Chunks")
                        _chunkDefViewTab = tab;
                    else if (tab.Text == "Extra Anim. Chunks")
                        _extraAnimChunkDefViewTab = tab;
                }
            }

            ShowHideTables();

            return Control;
        }

        private void ShowHideTables() {
            if (TabControl == null || TabControl.TabPages == null)
                return;

            var showExtraAnims = _model?.Header?.ExtraAnimChunkDefTable != null;
            if (showExtraAnims && !TabControl.TabPages.Contains(_extraAnimChunkDefViewTab))
                TabControl.TabPages.Insert(TabControl.TabPages.IndexOf(_chunkDefViewTab) + 1, _extraAnimChunkDefViewTab);
            else if (!showExtraAnims && TabControl.TabPages.Contains(_extraAnimChunkDefViewTab))
                TabControl.TabPages.Remove(_extraAnimChunkDefViewTab);
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    AnimationViewer.PolyChar = _model;
                    TextureSheetView.Texture = _model?.TextureAtlas;
                    PaletteView.Texture  = _model?.Palette;
                    ChunkDefView.Table   = _model?.Header?.ChunkDefTable;
                    ExtraAnimChunkDefView.Table = _model?.Header?.ExtraAnimChunkDefTable;
                    TexturesView.Model   = _model;
                    ModelsView.Model     = _model;
                    AnimationsView.Model = _model;
                    ShowHideTables();
                }
            }
        }

        public PCAnimationView AnimationViewer { get; }
        public TextureView PaletteView { get; }
        public INameGetterContext NameGetterContext { get; }
        public TableView ChunkDefView { get; }
        public TableView ExtraAnimChunkDefView { get; }
        public PCTexChunkView TexturesView { get; }
        public PCModelChunkView ModelsView { get; }
        public PCAnimationChunkView AnimationsView { get; }
        public TextureView TextureSheetView { get; }

        public TabPage _chunkDefViewTab;
        public TabPage _extraAnimChunkDefViewTab;
    }
}
