using System.Windows.Forms;
using SF3.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_FlagsView : ControlView<MPD_FlagEditor> {
        public MPD_FlagsView(string name, IMPD_Flags model) : base(name) {
            _model = model;
        }

        public override Control Create() {
            var rval = base.Create();
            if (rval == null)
                return null;

            Control.FlagsSource = Model;
            return rval;
        }

        public override void RefreshContent()
            => Control?.UpdateFlagsFromSource();

        private IMPD_Flags _model;
        public IMPD_Flags Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    if (Control != null)
                        Control.FlagsSource = value;
                }
            }
        }
    }
}
