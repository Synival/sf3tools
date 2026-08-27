using System.Linq;
using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class SGL_Model3DView : ControlView<SGL_ModelViewerControl> {
        public SGL_Model3DView(string name, ITextureMetaCollection texContainer, ISGL_Model sglModel = null, bool? forceLighting = null)
        : base(name) {
            _texCollection = texContainer;
            _sglModels     = sglModel == null ? [] : [sglModel];
            _forceLighting = forceLighting;
        }

        public SGL_Model3DView(string name, ITextureMetaCollection texContainer, ISGL_Model[] sglModels, bool? forceLighting = null)
        : base(name) {
            _texCollection = texContainer;
            _sglModels     = sglModels;
            _forceLighting = forceLighting;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.ForceLighting = _forceLighting;
            Control.Update(_texCollection, _sglModels);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;
            Control.Update(null, (ISGL_Model) null);
            Control.Update(_texCollection, _sglModels);
        }

        private ITextureMetaCollection _texCollection = null;
        public ITextureMetaCollection TextureCollection => _texCollection;

        private ISGL_Model[] _sglModels = [];

        public ISGL_Model[] Models => _sglModels;

        public void SetModel(ITextureMetaCollection texCollection, ISGL_Model sglModel)
            => SetModels(texCollection, sglModel == null ? [] : [sglModel]);

        public void SetModels(ITextureMetaCollection texCollection, ISGL_Model[] sglModels) {
            sglModels ??= [];
            if (_texCollection != texCollection || !Enumerable.SequenceEqual(_sglModels, sglModels)) {
                _texCollection = texCollection;
                _sglModels     = sglModels;
                if (Control != null)
                    Control.Update(_texCollection, _sglModels);
            }
        }

        private readonly bool? _forceLighting;
    }
}
