using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class Model3DView : ControlView<PDataViewerControl> {
        public Model3DView(string name, IMPD_File mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public Model3DView(string name, IMPD_File mpdFile, ModelInstanceBase modelInstance) : base(name) {
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

        public IMPD_File MPD_File { get; }

        private ModelInstanceBase _modelInstance = null;
        public ModelInstanceBase Model {
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

        private void UpdateSGL_Model() {
            var mc = (_modelInstance == null) ? null : MPD_File?.ModelCollections?.FirstOrDefault(x => x.CollectionType == _modelInstance.CollectionType);
            var pdata = (mc?.PDatasByMemoryAddress?.TryGetValue(_modelInstance.PData0, out var pdataOut) == true) ? pdataOut : null;
            if (pdata == null) {
                _sglModel = null;
                return;
            }

            _sglModel = mc?.GetSGLModel(pdata.ID + (int) pdata.Collection * ModelCollection.IDsPerCollectionType);
        }

        private SGL_Model _sglModel = null;

        private void UpdateViewerControl() {
            if (_modelInstance == null)
                Control.Update(MPD_File, _sglModel);
            else
                Control.Update(MPD_File, _sglModel, _modelInstance.AngleX, _modelInstance.AngleY, _modelInstance.AngleZ, _modelInstance.ScaleX, _modelInstance.ScaleY, _modelInstance.ScaleZ);
        }
    }
}
