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

        public Model3DView(string name, IMPD_File mpdFile, Model model) : base(name) {
            MPD_File = mpdFile;
            _model = model;
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

            Control.Update(null, 0, null);
            UpdateViewerControl();
        }

        public IMPD_File MPD_File { get; }

        private Model _model = null;
        public Model Model {
            get => _model;
            set {
                if (value != _model) {
                    _model = value;
                    UpdateSGL_Model();

                    if (Control != null)
                        UpdateViewerControl();
                }
            }
        }

        private void UpdateSGL_Model() {
            var mc = (_model == null) ? null : MPD_File?.ModelCollections
                ?.FirstOrDefault(x => x.CollectionType == _model.CollectionType && x.PDatasByMemoryAddress?.ContainsKey(_model.PData0) == true);

            if (mc == null) {
                _sglModel = null;
                _pdataAddr = 0;
            }
            else {
                var pdata = mc.PDatasByMemoryAddress[_model.PData0];
                _pdataAddr = pdata.RamAddress;
                _sglModel = mc.MakeSGLModel(pdata);
            }
        }

        private SGL_Model _sglModel = null;
        private uint _pdataAddr;

        private void UpdateViewerControl() {
            if (_model == null)
                Control.Update(MPD_File, _pdataAddr, _sglModel);
            else
                Control.Update(MPD_File, _pdataAddr, _sglModel, _model.AngleX, _model.AngleY, _model.AngleZ, _model.ScaleX, _model.ScaleY, _model.ScaleZ);
        }
    }
}
