using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.Models.Tables;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class SGL_ModelTableView<TStruct, TTable> : ControlSpaceView
    where TStruct : ISGL_Model
    where TTable : ITable {
        public SGL_ModelTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc) : base(name) {
            _textureCollection = texCollection;
            _table             = table;

            TableView = new TableView("ModelTable", table, ngc, typeof(TStruct));
            ModelView = new SGL_Model3DView("ModelView", texCollection);
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

                tableControl.SelectedItem = (_table?.Count >= 1) ? TableView.OLVControl.Items[0] : null;
            });

            // Return the top-level control.
            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl?.SelectedItem;
            ModelView.SetModel(_textureCollection, (TStruct) item?.RowObject);
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

        private ITextureMetaCollection _textureCollection;
        private ITable _table;

        public ITextureMetaCollection TextureCollection => _textureCollection;
        public ITable Table => _table;

        public void SetTable(ITextureMetaCollection texCollection, ITable table) {
            if (_textureCollection != texCollection || _table != table) {
                _textureCollection = texCollection;
                _table = table;

                TableView.Table = _table;
                if (TableView.OLVControl != null)
                    TableView.OLVControl.SelectedItem = (_table?.Count >= 1) ? TableView.OLVControl.Items[0] : null;

                var item = (OLVListItem) TableView.OLVControl?.SelectedItem;
                ModelView.SetModel(_textureCollection, (TStruct) item?.RowObject);
            }
        }

        public TableView TableView { get; }
        public SGL_Model3DView ModelView { get; }
    }
}
