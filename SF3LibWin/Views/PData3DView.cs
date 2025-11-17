using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class PData3DView : ControlView<PDataViewerControl> {
        public PData3DView(string name, IMPD_File mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PData3DView(string name, IMPD_File mpdFile, PDataModel pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
            _models = (pdata == null) ? null : mpdFile.ModelCollections.FirstOrDefault(x => x.PDatasByMemoryAddress.ContainsKey(pdata.RamAddress));
            _sglModel = _models?.MakeSGLModel(_pdata);
        }

        private ModelCollection _models = null;
        private SGL_Model _sglModel = null;

        public override Control Create() {
            var rval = base.Create();
            Control.Update(MPD_File, _pdata?.RamAddress ?? 0, _sglModel);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, 0, null);
            Control.Update(MPD_File, _pdata?.RamAddress ?? 0, _sglModel);
        }

        public IMPD_File MPD_File { get; }

        private PDataModel _pdata = null;
        public PDataModel PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    _models = (_pdata == null) ? null : MPD_File.ModelCollections.FirstOrDefault(x => x.PDatasByMemoryAddress.ContainsKey(_pdata.RamAddress));
                    _sglModel = _models?.MakeSGLModel(_pdata);
                    if (Control != null)
                        Control.Update(MPD_File, _pdata?.RamAddress ?? 0, _sglModel);
                }
            }
        }
    }
}
