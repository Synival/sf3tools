using System.Windows.Forms;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.Models.Structs.KAO;

namespace SF3.Win.Views.KAO {
    public class FaceSpritesheetView : ControlSpaceView {
        public FaceSpritesheetView(string name, FaceChunk chunk, INameGetterContext nameGetterContext) : base(name) {
            SpritesheetView = new TextureView("Spritesheet", Spritesheet, imageScale: 2.0f);
            TextureView = new FaceAnimationView("FaceAnimation", chunk);
            Chunk       = chunk;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(SpritesheetView, null);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        public override void Destroy() {
            if (Chunk != null)
                DetachInvalidatedEvents(Chunk);
            base.Destroy();
        }

        private void OnChunkModified(object sender, ByteArrayRangeModifiedArgs args)
            => TextureView.StartAnimation(_chunk);

        private FaceChunk _chunk;
        public FaceChunk Chunk {
            get => _chunk;
            set {
                if (value != _chunk) {
                    if (_chunk != null)
                        DetachInvalidatedEvents(_chunk);

                    _chunk = value;
                    SpritesheetView.Texture = _chunk?.Spritesheet;

                    if (_chunk != null)
                        AttachInvalidatedEvents(_chunk);

                    TextureView.StartAnimation(_chunk);
                }
            }
        }

        private void AttachInvalidatedEvents(FaceChunk chunk) {
            chunk.Data.Data.RangeModified += OnChunkModified;
        }

        private void DetachInvalidatedEvents(FaceChunk chunk) {
            // Invalidate this image if ANY data has changed.
            chunk.Data.Data.RangeModified -= OnChunkModified;
        }

        public FaceSpritesheet Spritesheet => Chunk?.Spritesheet;

        public TextureView SpritesheetView { get; }
        public FaceAnimationView TextureView { get; }
    }
}
