using System.Linq;
using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class SGL_ModelInstance3DView : ControlView<SGL_ModelViewerControl> {
        public SGL_ModelInstance3DView(string name, ITextureMetaCollection texContainer, ISGL_ModelInstance sglModelInstance = null, bool? forceLighting = null, float size = 1.0f, float zoom = 1.0f)
        : base(name) {
            _texCollection = texContainer;
            _sglModelInstances = (sglModelInstance == null) ? [] : [sglModelInstance];
            _forceLighting = forceLighting;
            _size = size;
            _zoom = zoom;
        }

        public SGL_ModelInstance3DView(string name, ITextureMetaCollection texContainer, ISGL_ModelInstance[] sglModelInstances, bool? forceLighting = null, float size = 1.0f, float zoom = 1.0f)
        : base(name) {
            _texCollection = texContainer;
            _sglModelInstances = sglModelInstances ?? [];
            _forceLighting = forceLighting;
            _size = size;
            _zoom = zoom;
        }

        public override Control Create() {
            var rval = base.Create();
            if (_size != 1.0f) {
                Control.MaximumSize = new System.Drawing.Size((int) (Control.MaximumSize.Width * _size), (int) (Control.MaximumSize.Height * _size));
                Control.MinimumSize = new System.Drawing.Size((int) (Control.MinimumSize.Width * _size), (int) (Control.MinimumSize.Height * _size));
                Control.Size = new System.Drawing.Size((int) (Control.Size.Width * _size), (int) (Control.Size.Height * _size));
            }
            Control.Zoom = _zoom;

            Control.ForceLighting = _forceLighting;
            UpdateViewerControl();
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;
            Control.ForceLighting = _forceLighting;
            Control.Update(null, (ISGL_Model) null);
            UpdateViewerControl();
        }

        private ITextureMetaCollection _texCollection = null;
        public ITextureMetaCollection TextureCollection => _texCollection;

        private ISGL_ModelInstance[] _sglModelInstances = [];

        public ISGL_ModelInstance[] ModelInstances => _sglModelInstances;

        public void SetModelInstance(ITextureMetaCollection texCollection, ISGL_ModelInstance sglModelInstance)
            => SetModelInstances(texCollection, (sglModelInstance == null) ? [] : [sglModelInstance]);

        public void SetModelInstances(ITextureMetaCollection texCollection, ISGL_ModelInstance[] sglModelInstances) {
            sglModelInstances ??= [];
            if (_texCollection != texCollection || !Enumerable.SequenceEqual(_sglModelInstances, sglModelInstances)) {
                _texCollection = texCollection;
                _sglModelInstances = sglModelInstances;
                UpdateViewerControl();
            }
        }

        private void UpdateViewerControl() {
            if (Control != null)
                Control.Update(_texCollection, _sglModelInstances);
        }

        private readonly bool? _forceLighting;
        private readonly float _size;
        private readonly float _zoom;
    }
}
