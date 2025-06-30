using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationFramesViewItem {
        public SpriteAnimationFramesViewItem(AnimationFrameTable animationFrameTable, FrameTable frameTable) {
            AnimationFrameTable = animationFrameTable;
            FrameTable          = frameTable;
        }

        public readonly int SpriteDirections;
        public readonly AnimationFrameTable AnimationFrameTable;
        public readonly FrameTable FrameTable;
        public string Name => AnimationFrameTable.Name;
    }

    public class SpriteAnimationFramesView : ControlSpaceView {
        public SpriteAnimationFramesView(string name, int spriteDirections, SpriteAnimationFramesViewItem tables, INameGetterContext nameGetterContext) : base(name) {
            SpriteDirections = spriteDirections;
            Model       = tables?.AnimationFrameTable;
            FrameTable  = tables?.FrameTable;
            TableView   = new TableView("Frames", Model, nameGetterContext, typeof(AnimationFrame));
            TextureView = new ImageView("Texture", textureScale: 2);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnTextureChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnTextureChanged(object sender, EventArgs e)
            => UpdateTexture();

        public void UpdateTexture() {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var animationFrame = (AnimationFrame) item?.RowObject;
            TextureView.Image = animationFrame?.GetTexture(SpriteDirections)?.CreateBitmapARGB1555();
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

        AnimationFrameTable _model = null;

        public int SpriteDirections { get; }

        public AnimationFrameTable Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    TableView.Table = value;
                }
            }
        }

        public FrameTable FrameTable { get; set; }
        public TableView TableView { get; private set; }
        public ImageView TextureView { get; private set; }
    }
}