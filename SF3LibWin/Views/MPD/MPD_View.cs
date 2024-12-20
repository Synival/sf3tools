using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD_File model) : base(name) {
            Model = model;
            ViewerView = new MPD_ViewerView("Viewer", Model);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(ViewerView, (c) => {
                _viewerTab = (TabPage) c.Parent;
                ViewerView.UpdateMap();
            }, autoFill: true);

            TabControl.Selected += UpdateViewer;

            CreateChild(new HeadersView ("Headers",  Model));
            CreateChild(new PalettesView("Palettes", Model));
            CreateChild(new SurfaceView ("Surface",  Model));
            CreateChild(new TexturesView("Textures", Model));

            return Control;
        }

        private TabPage _viewerTab = null;

        void UpdateViewer(object sender, EventArgs eventArgs) {
            if (TabControl.SelectedTab == _viewerTab)
                ViewerView?.UpdateMap();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            if (TabControl != null)
                TabControl.Selected -= UpdateViewer;

            ViewerView.Destroy();
            _viewerTab = null;

            base.Destroy();
        }

        public IMPD_File Model { get; }
        public MPD_ViewerView ViewerView { get; }
    }
}
