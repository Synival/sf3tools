using System;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Structs.KAO;

namespace SF3.Win.Views {
    public class FaceHeaderView : ControlSpaceView {
        public FaceHeaderView(string name, FaceChunk chunk, INameGetterContext nameGetterContext) : base(name) {
            HeaderView  = new DataModelView("Header", Header, nameGetterContext, typeof(FaceHeader));
            TextureView = new TextureView("Texture");
            Chunk       = chunk;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(HeaderView, null);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        public override void Destroy() {
            if (Chunk != null) {
                Chunk.ImageTable[0].Invalidated -= OnUpdateImage;
                Chunk.ImageTable[1].Invalidated -= OnUpdateImage;
                Chunk.ImageTable[9].Invalidated -= OnUpdateImage;
            }
            base.Destroy();
        }

        private void OnUpdateImage(object sender, EventArgs ags)
            => TextureView.ReloadImage();

        private FaceChunk _chunk;
        public FaceChunk Chunk {
            get => _chunk;
            set {
                if (value != _chunk) {
                    if (_chunk != null) {
                        _chunk.ImageTable[0].Invalidated -= OnUpdateImage;
                        _chunk.ImageTable[1].Invalidated -= OnUpdateImage;
                        _chunk.ImageTable[9].Invalidated -= OnUpdateImage;
                    }

                    _chunk = value;
                    HeaderView.Model = _chunk?.Header;

                    if (_chunk != null) {
                        _chunk.ImageTable[0].Invalidated += OnUpdateImage;
                        _chunk.ImageTable[1].Invalidated += OnUpdateImage;
                        _chunk.ImageTable[9].Invalidated += OnUpdateImage;

                        var newData  = _chunk.ImageTable[0].ImageData8Bit.Clone() as byte[,];

                        var blinkRef = _chunk.ImageTable[1].FrameRef ?? _chunk.ImageTable[1].SubstituteFrameRef;
                        if (blinkRef.HasValue)
                            FaceCompositeImage.AddFaceImageToData(newData, _chunk.ImageTable[blinkRef.Value]);

                        var talkRef  = _chunk.ImageTable[9].FrameRef ?? _chunk.ImageTable[9].SubstituteFrameRef;
                        if (talkRef.HasValue)
                            FaceCompositeImage.AddFaceImageToData(newData, _chunk.ImageTable[talkRef.Value]);

                        TextureView.Texture = new TextureIndexed(
                            SF3.Types.CollectionType.Primary, 0, 0, 0, newData,
                            SF3.Types.TexturePixelFormat.Palette1, _chunk.Palette, true
                        );
                    }
                    else {
                        TextureView.Texture = null;
                    }
                }
            }
        }

        public FaceHeader Header => Chunk?.Header;

        public DataModelView HeaderView { get; }
        public TextureView TextureView { get; }
    }
}
