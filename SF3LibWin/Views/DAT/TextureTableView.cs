using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Win.Extensions;

namespace SF3.Win.Views.DAT {
    public class TextureTableView<T> : ControlSpaceView where T : TextureModelBase {
        public TextureTableView(string name, INameGetterContext nameGetterContext, Table<TextureModelBase> model, int textureScale)
        : base(name) {
            NameGetterContext = nameGetterContext;

            var type = (model.Length > 0) ? model[0].GetType() : typeof(T);
            TableView   = new TableView("Textures", model, nameGetterContext, type);
            TextureView = new ImageView("Texture", textureScale);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnFrameChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnFrameChanged(object sender, EventArgs e)
            => UpdateTexture();

        public void UpdateTexture() {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (TextureModelBase) item?.RowObject;
            TextureView.Image = frame?.Texture?.CreateBitmapARGB1555();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnFrameChanged;

            TableView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public INameGetterContext NameGetterContext { get; }

        public Table<TextureModelBase> Table {
            get => (Table<TextureModelBase>) TableView.Table;
            set => TableView.Table = value;
        }

        public TableView TableView { get; private set; }
        public ImageView TextureView { get; private set; }
    }
}