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

            TabControl.Selected += UpdateViewerMapEvent;

            CreateChild(new MPD_FlagsView("Flags", Model.Flags));
            CreateChild(new LightingView("Lighting", Model));
            CreateChild(new TableView("Boundaries", Model.BoundariesTable, Model.NameGetterContext));
            CreateChild(new ModelsTabView("Models", Model));
            CreateChild(new TexturesView("Textures", Model));
            CreateChild(new PlanesView("Planes", Model.Planes));
            CreateChild(new DataView("Data (advanced)", Model));

            return Control;
        }

        private TabPage _viewerTab = null;

        void UpdateViewerMapEvent(object sender, EventArgs eventArgs)
            => UpdateViewerMap();

        public void UpdateViewerMap() {
            if (TabControl.SelectedTab == _viewerTab)
                ViewerView?.UpdateMap();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            if (TabControl != null)
                TabControl.Selected -= UpdateViewerMapEvent;

            ViewerView.Destroy();
            _viewerTab = null;

            base.Destroy();
        }

        public IMPD_File Model { get; }
        public MPD_ViewerView ViewerView { get; }
    }
}
