using System;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Types;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class ModelIDTableView : ControlSpaceView {
        public ModelIDTableView(string name, IMPD_File mpdFile, ModelIDTable table, INameGetterContext ngc) : base(name) {
            MPD_File = mpdFile;
            _table = table;
            TableView = new TableView("Models", table, ngc, typeof(ModelIDStruct));
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
                        tableControl.SelectionChanged += OnModelChanged;
                    }
                }

                tableControl.SelectedItem = (_table?.Length >= 1) ? TableView.OLVControl.Items[0] : null;
            });

            // Return the top-level control.
            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;

            // TODO: what about other collections??
            var collection = (MPD_File.ModelCollections?.TryGetValue(MPD_CollectionType.Primary, out var mc) == true) ? mc : null;
            var modelIdStr = (ModelIDStruct) item?.RowObject;

            ModelView.Model = (collection != null && modelIdStr != null)
                ? (ModelInstanceBase) (collection.ModelInstances.FirstOrDefault(x => x.ID == modelIdStr.ModelID))
                : null;
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

        private ModelIDTable _table;

        public IMPD_File MPD_File { get; }

        public ModelIDTable Table {
            get => _table;
            set {
                if (_table != value) {
                    _table = value;
                    TableView.Table = value;
                    if (TableView.OLVControl != null)
                        TableView.OLVControl.SelectedItem = (value?.Length >= 1) ? TableView.OLVControl.Items[0] : null;
                }
            }
        }

        public TableView TableView { get; }
        public Model3DView ModelView { get; }
    }
}
