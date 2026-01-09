using System.Windows.Forms;
using CommonLib.SGL;
using SF3.Models.Structs.MPD.Model;
using SF3.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class PData3DView : ControlView<PDataViewerControl> {
        public PData3DView(string name, IMPD mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PData3DView(string name, IMPD mpdFile, PDataStruct pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
            UpdateSGLModel();
        }

        private IMPD_ModelCollection _models = null;
        private ISGL_Model _sglModel = null;

        public override Control Create() {
            var rval = base.Create();
            Control.Update(MPD_File, _sglModel);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, null);
            Control.Update(MPD_File, _sglModel);
        }

        private void UpdateSGLModel() {
            _models   = (_pdata == null) ? null : MPD_File.ModelCollections.TryGetValue(_pdata.Collection, out var mcOut) ? mcOut : null;
            _sglModel = (_pdata == null) ? null : _models?.GetModel(_pdata.ID);
        }

        public IMPD MPD_File { get; }

        private PDataStruct _pdata = null;
        public PDataStruct PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    UpdateSGLModel();
                    if (Control != null)
                        Control.Update(MPD_File, _sglModel);
                }
            }
        }
    }
}
