using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD_File model) : base(name) {
            Model = model;
            ViewerView = new MPD_ViewerView("Main Viewer", Model);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(ViewerView, (c) => {
                _viewerTab = (TabPage) c.Parent;
                ViewerView.UpdateMap();
            }, autoFill: true);

            TabControl.Selected += UpdateViewer;

            CreateChild(new LightingView("Lighting", Model));
            CreateChild(new TableView("Boundaries", Model.BoundariesTable, Model.NameGetterContext));
            CreateChild(new ModelsView("Models", Model));
            CreateChild(new TexturesView("Textures", Model));

            if (Model.RepeatingGroundImage != null)
                CreateChild(new TextureView("Ground", Model.RepeatingGroundImage, 1));
            if (Model.TiledGroundImage != null)
                CreateChild(new TextureView("Ground", Model.TiledGroundImage, 0.50f));
            if (Model.TiledGroundTileImage != null)
                CreateChild(new TextureView("Ground Tiles", Model.TiledGroundTileImage, 1));
            if (Model.SkyBoxImage != null)
                CreateChild(new TextureView("Sky Box", Model.SkyBoxImage, 1));
            if (Model.BackgroundImage != null)
                CreateChild(new TextureView("Background", Model.BackgroundImage, 1));
            if (Model.ForegroundTileImage != null)
                CreateChild(new TextureView("Foreground Tiles", Model.ForegroundTileImage, 1));
            if (Model.ForegroundImage != null)
                CreateChild(new TextureView("Foreground", Model.ForegroundImage, 1));

            CreateChild(new DataView("Data (only modify if you know what you're doing!)", Model));

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
