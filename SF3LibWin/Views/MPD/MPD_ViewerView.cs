using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_ViewerView : ControlView<MPD_ViewerControl> {
        public MPD_ViewerView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            var rval = base.Create();
            if (ViewerControl != null)
                ViewerControl.Model = Model;
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            // TODO: how to refresh???
        }

        public void UpdateMap() => ViewerControl.UpdateMap();

        public IMPD_File Model { get; }

        public MPD_ViewerControl ViewerControl => (MPD_ViewerControl) Control;
        public MPD_ViewerGLControl ViewerGLControl => ((MPD_ViewerControl) Control).GLControl;
    }
}
