using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class PData3DView : ControlView<PDataViewerControl> {
        public PData3DView(string name, IMPD_File mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PData3DView(string name, IMPD_File mpdFile, PDataModel pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
            UpdateSGLModel();
        }

        private IMPD_ModelCollection _models = null;
        private SGL_Model _sglModel = null;

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
            _models = (_pdata == null) ? null : MPD_File.ModelCollections.FirstOrDefault(x => x.CollectionType == _pdata.Collection);
            _sglModel = (_pdata == null) ? null : _models?.GetSGLModel(_pdata.ID + (int) _pdata.Collection * ModelCollection.IDsPerCollectionType);
        }

        public IMPD_File MPD_File { get; }

        private PDataModel _pdata = null;
        public PDataModel PData {
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
