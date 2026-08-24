using System.Windows.Forms;
using CommonLib.SGL;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class PData3DView : ControlView<SGL_ModelViewerControl> {
        public PData3DView(string name, IMPD mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PData3DView(string name, IMPD mpdFile, ISGL_Model pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
            UpdateMPDModel();
        }

        private ISGL_ModelCollection _models = null;
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

        private void UpdateMPDModel() {
            _models   = _pdata == null ? null : MPD_File.ModelCollections.TryGetValue((MPD_CollectionType) _pdata.ModelCollectionID, out var mcOut) ? mcOut : null;
            _sglModel = _pdata == null ? null : _models?.GetModel(_pdata.ModelID, _pdata.LevelOfDetail);
        }

        public IMPD MPD_File { get; }

        private ISGL_Model _pdata = null;
        public ISGL_Model PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    UpdateMPDModel();
                    if (Control != null)
                        Control.Update(MPD_File, _sglModel);
                }
            }
        }
    }
}
