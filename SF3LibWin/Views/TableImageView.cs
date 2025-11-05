using System;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public abstract class TableImageView<TTableItem, TTable> : ControlSpaceView
        where TTableItem : class, IStruct
        where TTable : class, ITable<TTableItem>
    {
        public TableImageView(string name, TTable table, INameGetterContext nameGetterContext, float? textureScale = null) : base(name) {
            TableView = new TableView("Table", table, nameGetterContext, typeof(TTableItem));
            ImageView = new ImageView("Texture", textureScale ?? 0);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnModelChanged);
            CreateChild(ImageView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e)
            => UpdateTexture();

        public void UpdateTexture() {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (TTableItem) item?.RowObject;
            ImageView.Image = GetImageFromModel(frame);
        }

        public abstract Image GetImageFromModel(TTableItem item);

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnModelChanged;

            TableView.Destroy();
            ImageView.Destroy();

            base.Destroy();
        }

        public TTable Table {
            get => (TTable) TableView.Table;
            set => TableView.Table = value;
        }

        public TableView TableView { get; private set; }
        public ImageView ImageView { get; private set; }
    }
}
