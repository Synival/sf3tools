using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class SGL_Model3DView : ControlView<SGL_ModelViewerControl> {
        public SGL_Model3DView(string name, ITextureMetaCollection texContainer, ISGL_Model sglModel = null) : base(name) {
            TextureContainer = texContainer;
            _sglModel = sglModel;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.Update(TextureContainer, _sglModel);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;
            Control.Update(null, null);
            Control.Update(TextureContainer, _sglModel);
        }

        public ITextureMetaCollection TextureContainer { get; }

        private ISGL_Model _sglModel = null;
        public ISGL_Model Model {
            get => _sglModel;
            set {
                if (value != _sglModel) {
                    _sglModel = value;
                    if (Control != null)
                        Control.Update(TextureContainer, _sglModel);
                }
            }
        }
    }
}
