using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationsView : ControlSpaceView {
        public SpriteAnimationsView(string name, AnimationTable model, INameGetterContext nameGetterContext) : base(name) {
            Model       = model;
            TableView   = new TableView("Frames", model, nameGetterContext, typeof(Animation));
            TextureView = new SpriteAnimationTextureView("Texture");
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnAnimationChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnAnimationChanged(object sender, EventArgs e)
            => UpdateTexture();

        public void UpdateTexture() {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var frame = (Animation) item?.RowObject;
            //TextureView.Image = frame?.Texture?.CreateBitmapARGB1555();
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (TableView.OLVControl != null)
                TableView.OLVControl.ItemSelectionChanged -= OnAnimationChanged;

            TableView.Destroy();
            TextureView.Destroy();

            base.Destroy();
        }

        public AnimationTable Model { get; }
        public TableView TableView { get; private set; }
        public TextureView TextureView { get; private set; }
    }
}