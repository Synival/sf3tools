using System.Linq;
using System.Windows.Forms;
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
        }

        public override Control Create() {
            var rval = base.Create();
            Control.Update(MPD_File, _pdata);
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            Control.Update(null, null);
            Control.Update(MPD_File, _pdata);
        }

        public IMPD_File MPD_File { get; }

        private Model _model = null;
        public Model Model {
            get => _model;
            set {
                if (value != _model) {
                    _model = value;
                    _pdata = (_model == null) ? null : ((MPD_File?.ModelCollections
                        ?.FirstOrDefault(x => x.CollectionType == _model.CollectionType)
                        ?.PDatasByMemoryAddress?.TryGetValue(_model.PData0, out var pdataVal) == true) ? pdataVal : null);
                        
                    if (Control != null)
                        Control.Update(MPD_File, _pdata);
                }
            }
        }

        private PDataModel _pdata = null;
    }
}
