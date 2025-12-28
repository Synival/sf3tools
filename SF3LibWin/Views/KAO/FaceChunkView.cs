using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.KAO;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Win.Views.KAO {
    public class FaceChunkView : TabView {
        public FaceChunkView(string name, FaceChunk chunk, INameGetterContext ngc, bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top) : base(name, lazyLoad, tabAlignment) {
            Chunk = chunk;
            NameGetterContext = ngc;

            HeaderView     = new DataModelView("Header", Chunk?.Header, ngc, typeof(FaceHeader));
            PaletteView    = new ColorTableView("Palette", Chunk?.PaletteTable, NameGetterContext);
            ImageTableView = new TextureDataTableView<FaceImage, FaceImageTable>("Images", Chunk?.ImageTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(PaletteView);
            CreateChild(ImageTableView);

            return Control;
        }

        private FaceChunk _chunk = null;
        public FaceChunk Chunk {
            get => _chunk;
            set {
                if (_chunk != value) {
                    _chunk = value;

                    HeaderView.Model     = _chunk?.Header;
                    PaletteView.Table    = _chunk?.PaletteTable;
                    ImageTableView.Table = _chunk?.ImageTable;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public ColorTableView PaletteView { get; }
        public TextureDataTableView<FaceImage, FaceImageTable> ImageTableView { get; }
    }
}
