using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PolyCharView : TabView {
        public PolyCharView(string name, PolyChar model, INameGetterContext ngc, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            NameGetterContext = ngc;

            AnimationViewer = new PCAnimationView("Animation Viewer", model);
            ChunkDefView   = new TableView("Header", model?.Header?.ChunkDefTable, ngc, modelType: typeof(PCChunkDef));
            TexturesView   = new PCTexChunkView("Textures", model, NameGetterContext);
            ModelsView     = new PCModelChunkView("Models", model, NameGetterContext);
            AnimationsView = new PCAnimationChunkView("Animations", model, NameGetterContext);

            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(AnimationViewer);
            CreateChild(ChunkDefView);
            CreateChild(TexturesView);
            CreateChild(ModelsView);
            CreateChild(AnimationsView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    AnimationViewer.PolyChar = _model;
                    ChunkDefView.Table   = _model?.Header?.ChunkDefTable;
                    TexturesView.Model   = _model;
                    ModelsView.Model     = _model;
                    AnimationsView.Model = _model;
                }
            }
        }

        public PCAnimationView AnimationViewer { get; }
        public INameGetterContext NameGetterContext { get; }
        public TableView ChunkDefView { get; }
        public PCTexChunkView TexturesView { get; }
        public PCModelChunkView ModelsView { get; }
        public PCAnimationChunkView AnimationsView { get; }
    }
}
