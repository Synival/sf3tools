using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;

namespace SF3.Win.Views.MPD {
    public class MPD_View : TabView {
        public MPD_View(string name, IMPD model) : base(name) {
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
            CreateChild(new PlanesView("Planes", Model.Planes));

            if (Model is IMPD_File mpdFile) {
                CreateChild(new LightingView("Lighting", mpdFile));
                CreateChild(new TableView("Boundaries", mpdFile.BoundariesTable, mpdFile.NameGetterContext));
                CreateChild(new ModelsTabView("Models", mpdFile));
                CreateChild(new TexturesView("Textures", mpdFile));
                CreateChild(new DataView("Data (advanced)", mpdFile));
            }

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

        public IMPD Model { get; }
        public MPD_ViewerView ViewerView { get; }
    }
}
