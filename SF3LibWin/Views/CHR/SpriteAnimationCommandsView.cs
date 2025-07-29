using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationCommandsViewContext {
        public SpriteAnimationCommandsViewContext(AnimationCommandTable animationCommandTable, FrameTable frameTable) {
            AnimationCommandTable = animationCommandTable;
            FrameTable            = frameTable;
        }

        public readonly AnimationCommandTable AnimationCommandTable;
        public readonly FrameTable FrameTable;
        public string Name => AnimationCommandTable.Name;
    }

    public class SpriteAnimationCommandsView : ControlSpaceView {
        public SpriteAnimationCommandsView(string name, SpriteAnimationCommandsViewContext model, INameGetterContext nameGetterContext) : base(name) {
            Context     = model;
            TableView   = new TableView("Frames", Context?.AnimationCommandTable, nameGetterContext, typeof(AnimationCommand));
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
            var aniCommand = (AnimationCommand) item?.RowObject;
            TextureView.Image = aniCommand?.GetTexture(aniCommand.Directions)?.CreateBitmapARGB1555();
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

        private SpriteAnimationCommandsViewContext _context = null;
        public SpriteAnimationCommandsViewContext Context {
            get => _context;
            set {
                if (_context != value) {
                    _context = value;
                    TableView.Table = value?.AnimationCommandTable;
                }
            }
        }

        public TableView TableView { get; private set; }
        public ImageView TextureView { get; private set; }
    }
}