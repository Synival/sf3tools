using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCTexChunkView : TabView {
        public PCTexChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;
            HeaderView        = new DataModelView("Header", model?.TexDefChunkHeader, ngc, modelType: typeof(PCTexDefChunkHeader));
            TextureTableView  = new PCTextureTableView("Textures", model?.TextureTable, ngc);
            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(HeaderView);
            CreateChild(TextureTableView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    HeaderView.Model  = _model?.TexDefChunkHeader;
                    TextureTableView.Table = _model?.TextureTable;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public PCTextureTableView TextureTableView { get; }
    }
}
