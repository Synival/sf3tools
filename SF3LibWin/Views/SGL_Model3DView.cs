using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class SGL_Model3DView : ControlView<SGL_ModelViewerControl> {
        public SGL_Model3DView(string name, ITextureMetaCollection texContainer, ISGL_Model sglModel = null, bool forceLighting = false)
        : base(name) {
            _texCollection = texContainer;
            _sglModel      = sglModel;
            _forceLighting = forceLighting;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.ForceLighting = _forceLighting;
            Control.Update(_texCollection, _sglModel);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;
            Control.Update(null, null);
            Control.Update(_texCollection, _sglModel);
        }

        private ITextureMetaCollection _texCollection = null;
        public ITextureMetaCollection TextureCollection => _texCollection;

        private ISGL_Model _sglModel = null;

        public ISGL_Model Model => _sglModel;

        public void SetModel(ITextureMetaCollection texCollection, ISGL_Model sglModel) {
            if (_texCollection != texCollection || _sglModel != sglModel) {
                _texCollection = texCollection;
                _sglModel      = sglModel;
                if (Control != null)
                    Control.Update(_texCollection, _sglModel);
            }
        }

        private readonly bool _forceLighting;
    }
}
