using System.Windows.Forms;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class Model3DView : ControlView<SGL_ModelViewerControl> {
        public Model3DView(string name, ITextureContainer texContainer) : base(name) {
            TextureContainer = texContainer;
        }

        public Model3DView(string name, ITextureContainer texContainer, ISGL_ModelInstance modelInstance) : base(name) {
            TextureContainer = texContainer;
            _modelInstance = modelInstance;
            UpdateSGL_Model();
        }

        public override Control Create() {
            var rval = base.Create();
            UpdateViewerControl();
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, null);
            UpdateViewerControl();
        }

        public ITextureContainer TextureContainer { get; }

        private ISGL_ModelInstance _modelInstance = null;
        public ISGL_ModelInstance Model {
            get => _modelInstance;
            set {
                if (value != _modelInstance) {
                    _modelInstance = value;
                    UpdateSGL_Model();

                    if (Control != null)
                        UpdateViewerControl();
                }
            }
        }

        private void UpdateSGL_Model()
            => _sglModel = _modelInstance?.GetModel(0);
        private ISGL_Model _sglModel = null;

        private void UpdateViewerControl() {
            if (_modelInstance == null)
                Control.Update(TextureContainer, _sglModel);
            else
                Control.Update(TextureContainer, _sglModel, _modelInstance.AngleX, _modelInstance.AngleY, _modelInstance.AngleZ, _modelInstance.ScaleX, _modelInstance.ScaleY, _modelInstance.ScaleZ);
        }
    }
}
