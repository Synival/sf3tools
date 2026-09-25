using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCChunkView : TabView {
        public PCChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;

            HeaderChunkView     = new PCHeaderChunkView("Header Chunk", model, NameGetterContext);
            TextureChunkView    = new PCTexChunkView("Texture Chunk", model, NameGetterContext);
            ModelChunkView      = new PCModelChunkView("Model Chunk", model, NameGetterContext);
            AnimationChunkView  = new PCAnimationChunkView("Animation Chunk", model, NameGetterContext);

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

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

                    HeaderChunkView.Model    = _model;
                    TextureChunkView.Model   = _model;
                    ModelChunkView.Model     = _model;
                    AnimationChunkView.Model = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }

        public PCHeaderChunkView HeaderChunkView { get; }
        public PCTexChunkView TextureChunkView { get; }
        public PCModelChunkView ModelChunkView { get; }
        public PCAnimationChunkView AnimationChunkView { get; }
    }
}
