using System.Windows.Forms;
using CommonLib.SGL;
using SF3.MPD.Interfaces;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class Model3DView : ControlView<MPD_ModelViewerControl> {
        public Model3DView(string name, IMPD mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public Model3DView(string name, IMPD mpdFile, ISGL_ModelInstance modelInstance) : base(name) {
            MPD_File = mpdFile;
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

        public IMPD MPD_File { get; }

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
                Control.Update(MPD_File, _sglModel);
            else
                Control.Update(MPD_File, _sglModel, _modelInstance.AngleX, _modelInstance.AngleY, _modelInstance.AngleZ, _modelInstance.ScaleX, _modelInstance.ScaleY, _modelInstance.ScaleZ);
        }
    }
}
