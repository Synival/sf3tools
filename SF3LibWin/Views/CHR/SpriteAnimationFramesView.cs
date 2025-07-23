using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationFramesViewContext {
        public SpriteAnimationFramesViewContext(AnimationFrameTable animationFrameTable, FrameTable frameTable) {
            AnimationFrameTable = animationFrameTable;
            FrameTable          = frameTable;
        }

        public readonly AnimationFrameTable AnimationFrameTable;
        public readonly FrameTable FrameTable;
        public string Name => AnimationFrameTable.Name;
    }

    public class SpriteAnimationFramesView : ControlSpaceView {
        public SpriteAnimationFramesView(string name, SpriteAnimationFramesViewContext model, INameGetterContext nameGetterContext) : base(name) {
            Context       = model;
            TableView   = new TableView("Frames", Context?.AnimationFrameTable, nameGetterContext, typeof(AnimationFrame));
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
            TextureView.Image = animationFrame?.GetTexture(animationFrame.Directions)?.CreateBitmapARGB1555();
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

        private SpriteAnimationFramesViewContext _context = null;
        public SpriteAnimationFramesViewContext Context {
            get => _context;
            set {
                if (_context != value) {
                    _context = value;
                    TableView.Table = value?.AnimationFrameTable;
                }
            }
        }

        public TableView TableView { get; private set; }
        public ImageView TextureView { get; private set; }
    }
}