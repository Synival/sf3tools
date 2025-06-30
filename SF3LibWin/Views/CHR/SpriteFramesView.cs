using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteFramesView : ControlSpaceView {
        public SpriteFramesView(string name, FrameTable model, INameGetterContext nameGetterContext) : base(name) {
            TableView   = new TableView("Frames", model, nameGetterContext, typeof(Frame));
            TextureView = new ImageView("Texture");
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
            var frame = (Frame) item?.RowObject;
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

        public FrameTable Table {
            get => (FrameTable) TableView.Table;
            set => TableView.Table = value;
        }

        public TableView TableView { get; private set; }
        public ImageView TextureView { get; private set; }
    }
}