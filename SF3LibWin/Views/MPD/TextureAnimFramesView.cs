using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimFramesView : ControlSpaceView {
        public TextureAnimFramesView(string name, IMPD_File model, INameGetterContext nameGetterContext) : base(name) {
            var frameTable = AllFramesTable.Create(model.TextureAnimations.Data, model.TextureAnimations.Address, model.TextureAnimations);

            Model       = model;
            TableView   = new TableView("Frames", frameTable, nameGetterContext);
            TextureView = new TextureView("Texture");
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnTextureChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        void OnTextureChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (FrameModel) item?.RowObject;
            TextureView.Image = frame?.Texture?.CreateBitmap();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnTextureChanged;

            TableView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public IMPD_File Model { get; }
        public TableView TableView { get; private set; }
        public TextureView TextureView { get; private set; }
    }
}