using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.KAO;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Win.Views.KAO {
    public class FaceChunkView : TabView {
        public FaceChunkView(string name, FaceChunk chunk, INameGetterContext ngc, bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top) : base(name, lazyLoad, tabAlignment) {
            Chunk = chunk;
            NameGetterContext = ngc;

            HeaderView      = new FaceHeaderView("Header", Chunk, ngc);
            SpritesheetView = new TextureView("Spritesheet", imageScale: 1.0f);
            PaletteView     = new ColorTableView("Palette", Chunk?.PaletteTable, NameGetterContext);
            ImageTableView  = new TextureDataTableView<FaceImage, FaceImageTable>("Images", Chunk?.ImageTable, ngc);
            CompositeImageTableView = new TextureDataTableView<FaceCompositeImage, FaceCompositeImageTable>("Composite Images", Chunk?.CompositeImageTable, ngc);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView);
            CreateChild(SpritesheetView);
            CreateChild(PaletteView);
            CreateChild(ImageTableView);
            CreateChild(CompositeImageTableView);

            return Control;
        }

        private FaceChunk _chunk = null;
        public FaceChunk Chunk {
            get => _chunk;
            set {
                if (_chunk != value) {
                    _chunk = value;

                    HeaderView.Chunk        = _chunk;
                    SpritesheetView.Texture = _chunk.Spritesheet;
                    PaletteView.Table       = _chunk?.PaletteTable;
                    ImageTableView.Table    = _chunk?.ImageTable;
                    CompositeImageTableView.Table = _chunk?.CompositeImageTable;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public FaceHeaderView HeaderView { get; }
        public TextureView SpritesheetView { get; }
        public ColorTableView PaletteView { get; }
        public TextureDataTableView<FaceImage, FaceImageTable> ImageTableView { get; }
        public TextureDataTableView<FaceCompositeImage, FaceCompositeImageTable> CompositeImageTableView { get; }
    }
}
