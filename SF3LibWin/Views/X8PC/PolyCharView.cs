using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PolyCharView : TabView {
        public PolyCharView(string name, PolyChar model, INameGetterContext ngc, TabAlignment tabAlignment) : base(name, tabAlignment: tabAlignment) {
            NameGetterContext = ngc;

            AnimationViewer  = new PCAnimationView("Animation Viewer", model);
            TextureSheetView = new TextureView("Texture Atlas", model?.TextureAtlas, imageScale: 2.0f);
            PaletteView      = new TextureView("Palette", model?.Palette, imageScale: 16.00f);
            ChunkView        = new PCChunkView("Chunks", model, NameGetterContext);

            // Testing only!!
            AttackAnimChunkView = new DataHexView("Test", (model?.AttackAnimChunks?.Length >= 1) ? DecompressedAttackAnimChunk(model.AttackAnimChunks[0].DecompressedData.Data) : null);

            Model = model;
        }

        // Testing only!!
        private IByteArray DecompressedAttackAnimChunk(IByteArray dataIn)
            => new ByteArray(Compression.DecompressZigZag(dataIn.GetDataCopyOrReference()).ToBytes());

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(AnimationViewer);
            CreateChild(TextureSheetView);
            CreateChild(PaletteView);
            CreateChild(ChunkView);

            // Testing only!!
            CreateChild(AttackAnimChunkView);

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
                    PaletteView.Texture      = _model?.Palette;
                    ChunkView.Model          = _model;

                    // Testing only!!
                    AttackAnimChunkView.Data = (_model?.AttackAnimChunks?.Length >= 1) ? DecompressedAttackAnimChunk(_model.AttackAnimChunks[0].DecompressedData.Data) : null;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }

        public PCAnimationView AnimationViewer { get; }
        public TextureView TextureSheetView { get; }
        public TextureView PaletteView { get; }
        public PCChunkView ChunkView { get; }

        // Testing only!!
        public DataHexView AttackAnimChunkView { get; }
    }
}
