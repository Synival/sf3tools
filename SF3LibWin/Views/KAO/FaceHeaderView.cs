using System;
using System.Windows.Forms;
using CommonLib.NamedValues;
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
            if (Chunk != null)
                Chunk.CompositeImageTable[8].Invalidated -= OnUpdateImage;
            base.Destroy();
        }

        private void OnUpdateImage(object sender, EventArgs ags)
            => TextureView.ReloadImage();

        private FaceChunk _chunk;
        public FaceChunk Chunk {
            get => _chunk;
            set {
                if (value != _chunk) {
                    if (_chunk != null)
                        _chunk.CompositeImageTable[8].Invalidated -= OnUpdateImage;

                    _chunk = value;
                    HeaderView.Model = _chunk?.Header;

                    var tex = _chunk?.CompositeImageTable?[8];
                    TextureView.Texture = tex;
                    if (tex != null)
                        tex.Invalidated += OnUpdateImage;
                }
            }
        }

        public FaceHeader Header => Chunk?.Header;

        public DataModelView HeaderView { get; }
        public TextureView TextureView { get; }
    }
}
