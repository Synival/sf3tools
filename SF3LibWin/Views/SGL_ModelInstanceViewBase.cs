using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Tables;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public abstract class SGL_ModelInstanceViewBase<TStruct, TTable> : ControlSpaceView
    where TTable : ITable {
        public SGL_ModelInstanceViewBase(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc, bool? forceLighting = null, float size = 1.0f, float zoom = 1.0f)
        : base(name) {
            _textureCollection = texCollection;
            _table             = table;

            TableView = new TableView("ModelTable", table, ngc, typeof(TStruct));
            ModelView = new SGL_ModelInstance3DView("ModelView", texCollection, forceLighting: forceLighting, size: size, zoom: zoom);
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

        protected abstract void ModelInstanceSetter(SGL_ModelInstance3DView view, ITextureMetaCollection texCollection, TStruct model);

        private void OnModelChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl?.SelectedItem;
            ModelInstanceSetter(ModelView, _textureCollection, (TStruct) item?.RowObject);
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
                ModelInstanceSetter(ModelView, _textureCollection, (TStruct) item?.RowObject);
            }
        }

        public TableView TableView { get; }
        public SGL_ModelInstance3DView ModelView { get; }
    }
}
