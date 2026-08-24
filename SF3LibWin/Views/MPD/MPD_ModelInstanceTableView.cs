using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_ModelInstanceTableView : ControlSpaceView {
        public MPD_ModelInstanceTableView(string name, IMPD_File mpdFile, ITable<MPD_ModelInstance> model, INameGetterContext ngc) : base(name) {
            Model = model;
            TableView = new TableView("Models", model, ngc);
            ModelView = new SGL_ModelInstance3DView("Model", mpdFile);
        }

        public override Control Create() {
            base.Create();

            CreateChild(TableView, (c) => {
                var tableControl = (ObjectListView) c;

                // Add a model viewer on the right side of the tab.
                var tableParent = tableControl?.Parent;
                if (tableParent != null) {
                    var modelControl = (SGL_ModelViewerControl) ModelView.Create();
                    if (modelControl != null) {
                        modelControl.Dock = DockStyle.Right;
                        tableParent.Controls.Add(modelControl);
                        tableControl.SelectionChanged += OnModelChanged;
                    }
                }
            });

            // Return the top-level control.
            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            ModelView.ModelInstance = (MPD_ModelInstance) item?.RowObject;
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();
            if (TableView.OLVControl != null)
                TableView.OLVControl.SelectionChanged -= OnModelChanged;
            TableView.Destroy();

            base.Destroy();
        }

        public ITable<MPD_ModelInstance> Model { get; }
        public TableView TableView { get; }
        public SGL_ModelInstance3DView ModelView { get; }

    }
}
