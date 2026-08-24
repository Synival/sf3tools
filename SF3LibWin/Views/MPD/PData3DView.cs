using System.Windows.Forms;
using SF3.Models.Structs.MPD.Model;
using SF3.MPD.Interfaces;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class PData3DView : ControlView<SGL_ModelViewerControl> {
        public PData3DView(string name, IMPD mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PData3DView(string name, IMPD mpdFile, PDataStruct pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
            UpdateMPDModel();
        }

        private IMPD_ModelCollection _models = null;
        private IMPD_ModelLoD _mpdModel = null;

        public override Control Create() {
            var rval = base.Create();
            Control.Update(MPD_File, _mpdModel);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, null);
            Control.Update(MPD_File, _mpdModel);
        }

        private void UpdateMPDModel() {
            _models   = _pdata == null ? null : MPD_File.ModelCollections.TryGetValue(_pdata.Collection, out var mcOut) ? mcOut : null;
            _mpdModel = _pdata == null ? null : _models?.GetModel(_pdata.ModelID, _pdata.LevelOfDetail);
        }

        public IMPD MPD_File { get; }

        private PDataStruct _pdata = null;
        public PDataStruct PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    UpdateMPDModel();
                    if (Control != null)
                        Control.Update(MPD_File, _mpdModel);
                }
            }
        }
    }
}
