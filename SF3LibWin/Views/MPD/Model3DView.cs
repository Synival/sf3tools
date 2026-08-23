using System.Windows.Forms;
using SF3.Models.Structs.MPD.Model;
using SF3.MPD.Interfaces;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class Model3DView : ControlView<MPD_ModelViewerControl> {
        public Model3DView(string name, IMPD mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public Model3DView(string name, IMPD mpdFile, ModelInstanceBase modelInstance) : base(name) {
            MPD_File = mpdFile;
            _modelInstance = modelInstance;
            UpdateMPD_Model();
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

        private ModelInstanceBase _modelInstance = null;
        public ModelInstanceBase Model {
            get => _modelInstance;
            set {
                if (value != _modelInstance) {
                    _modelInstance = value;
                    UpdateMPD_Model();

                    if (Control != null)
                        UpdateViewerControl();
                }
            }
        }

        private void UpdateMPD_Model()
            => _mpdModel = _modelInstance?.GetMPD_ModelLoD(0);
        private IMPD_ModelLoD _mpdModel = null;

        private void UpdateViewerControl() {
            if (_modelInstance == null)
                Control.Update(MPD_File, _mpdModel);
            else
                Control.Update(MPD_File, _mpdModel, _modelInstance.AngleX, _modelInstance.AngleY, _modelInstance.AngleZ, _modelInstance.ScaleX, _modelInstance.ScaleY, _modelInstance.ScaleZ);
        }
    }
}
