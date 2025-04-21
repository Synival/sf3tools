using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class ModelTableView : ControlSpaceView {
        public ModelTableView(string name, IMPD_File mpdFile, ITable<Model> model, INameGetterContext ngc) : base(name) {
            Model = model;
            TableView = new TableView("Models", model, ngc);
            ModelView = new Model3DView("Model", mpdFile);
        }

        public override Control Create() {
            base.Create();

            CreateChild(TableView, (c) => {
                var tableControl = (ObjectListView) c;

                // Add a model viewer on the right side of the tab.
                var tableParent = tableControl?.Parent;
                if (tableParent != null) {
                    var modelControl = (PDataViewerControl) ModelView.Create();
                    if (modelControl != null) {
                        modelControl.Dock = DockStyle.Right;
                        tableParent.Controls.Add(modelControl);
                        tableControl.ItemSelectionChanged += OnModelChanged;
                    }
                }
            });

            // Return the top-level control.
            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            ModelView.Model = (Model) item?.RowObject;
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();
            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnModelChanged;
            TableView.Destroy();

            base.Destroy();
        }

        public ITable<Model> Model { get; }
        public TableView TableView { get; }
        public Model3DView ModelView { get; }

    }
}
