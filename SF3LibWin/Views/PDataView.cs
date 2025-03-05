using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class PDataView : ControlView<PDataViewerControl> {
        public PDataView(string name, IMPD_File mpdFile) : base(name) {
            MPD_File = mpdFile;
        }

        public PDataView(string name, IMPD_File mpdFile, PDataModel pdata) : base(name) {
            MPD_File = mpdFile;
            _pdata = pdata;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.Update(MPD_File, PData);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, null);
            Control.Update(MPD_File, PData);
        }

        public IMPD_File MPD_File { get; }

        private PDataModel _pdata = null;
        public PDataModel PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    if (Control != null)
                        Control.Update(MPD_File, _pdata);
                }
            }
        }
    }
}
