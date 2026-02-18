using System.Windows.Forms;
using SF3.MPD.Interfaces;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_ViewerView : ControlView<MPD_ViewerControl> {
        public MPD_ViewerView(string name, IMPD model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            var rval = base.Create();
            if (ViewerControl != null)
                ViewerControl.MPD_File = Model;
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            // TODO: how to refresh???
        }

        public void UpdateMap() {
            ViewerControl.InvalidateLighting();
            ViewerControl.UpdateModels();
        }

        public IMPD Model { get; }

        public MPD_ViewerControl ViewerControl => (MPD_ViewerControl) Control;
        public MPD_ViewerGLControl ViewerGLControl => ((MPD_ViewerControl) Control).GLControl;
    }
}
