using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PolyCharView : TabView {
        public PolyCharView(string name, PolyChar model, INameGetterContext ngc, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            NameGetterContext = ngc;

            AnimationViewer     = new PCAnimationView("Animation Viewer", model);
            TextureSheetView    = new TextureView("Texture Atlas", model?.TextureAtlas, imageScale: 2.0f);
            PaletteView         = new TextureView("Palette", model?.Palette, imageScale: 16.00f);

            HeaderChunkView     = new PCHeaderChunkView("Header Chunk", model, NameGetterContext);
            TextureChunkView    = new PCTexChunkView("Texture Chunk", model, NameGetterContext);
            ModelChunkView      = new PCModelChunkView("Model Chunk", model, NameGetterContext);
            AnimationChunkView  = new PCAnimationChunkView("Animation Chunk", model, NameGetterContext);

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(AnimationViewer);
            CreateChild(TextureSheetView);
            CreateChild(PaletteView);

            CreateChild(HeaderChunkView);
            CreateChild(TextureChunkView);
            CreateChild(ModelChunkView);
            CreateChild(AnimationChunkView);

            return Control;
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

                    HeaderChunkView.Model    = _model;
                    TextureChunkView.Model   = _model;
                    ModelChunkView.Model     = _model;
                    AnimationChunkView.Model = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }

        public PCAnimationView AnimationViewer { get; }
        public TextureView TextureSheetView { get; }
        public TextureView PaletteView { get; }

        public PCHeaderChunkView HeaderChunkView { get; }
        public PCTexChunkView TextureChunkView { get; }
        public PCModelChunkView ModelChunkView { get; }
        public PCAnimationChunkView AnimationChunkView { get; }
    }
}
