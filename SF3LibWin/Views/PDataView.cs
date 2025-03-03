using System.Drawing;
using System.Windows.Forms;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class PDataView : ControlView<PDataViewerControl> {
        public PDataView(string name) : base(name) {
        }

        public PDataView(string name, PDataModel pdata) : base(name) {
            _pdata = pdata;
        }

        public override Control Create() {
            var rval = base.Create();
            Control.PData = _pdata;
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            var old = Control.PData;
            Control.PData = null;
            Control.PData = old;
        }

        private PDataModel _pdata = null;
        public PDataModel PData {
            get => _pdata;
            set {
                if (value != _pdata) {
                    _pdata = value;
                    if (Control != null)
                        Control.PData = value;
                }
            }
        }
    }
}
