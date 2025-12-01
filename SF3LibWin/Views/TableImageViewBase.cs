using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public abstract class TableImageViewBase<TTableItem, TTable, TImageView> : ControlSpaceView
        where TTableItem : class, IStruct
        where TTable : class, ITable<TTableItem>
        where TImageView : ImageView
    {
        public TableImageViewBase(string name, TTable table, INameGetterContext nameGetterContext, float? imageScale = null) : base(name) {
            var firstItem = (table?.Length > 0) ? table[0] : null;
            TableView = new TableView("Table", table, nameGetterContext, firstItem?.GetType() ?? typeof(TTableItem));
            ImageView = CreateImageView(imageScale);
        }

        protected abstract TImageView CreateImageView(float? imageScale);

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnModelChanged);
            CreateChild(ImageView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnModelChanged(object sender, EventArgs e)
            => UpdateImage();

        public void UpdateImage() {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (TTableItem) item?.RowObject;
            SetImage(frame);
        }

        protected abstract void SetImage(TTableItem item);

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

        public TableView TableView { get; }
        public TImageView ImageView { get; }
    }
}
