using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationsViewContext {
        public SpriteAnimationsViewContext(int spriteDirections, AnimationTable animationTable, Dictionary<int, AnimationCommandTable> aniCommandTablesByIndex, FrameTable frameTable) {
            SpriteDirections              = spriteDirections;
            AnimationTable                = animationTable;
            AnimationCommandTablesByIndex = aniCommandTablesByIndex;
            FrameTable                    = frameTable;
        }

        public readonly int SpriteDirections;
        public readonly AnimationTable AnimationTable;
        public readonly Dictionary<int, AnimationCommandTable> AnimationCommandTablesByIndex;
        public readonly FrameTable FrameTable;

        public string Name => AnimationTable.Name;
    }

    public class SpriteAnimationsView : ControlSpaceView {
        public SpriteAnimationsView(string name, SpriteAnimationsViewContext context, INameGetterContext nameGetterContext) : base(name) {
            _context    = context;
            TableView   = new TableView("Frames", context.AnimationTable, nameGetterContext, typeof(Animation));
            TextureView = new SpriteAnimationTextureView("Texture", new SpriteAnimationTextureViewContext(context.SpriteDirections, context.AnimationCommandTablesByIndex, context.FrameTable), textureScale: 2);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            CreateChild(TableView, (c) => ((ObjectListView) c).ItemSelectionChanged += OnAnimationChanged);
            CreateChild(TextureView, (c) => c.Dock = DockStyle.Right, autoFill: false);

            return Control;
        }

        private void OnAnimationChanged(object sender, EventArgs e) {
            var item = (OLVListItem) TableView.OLVControl.SelectedItem;
            var animation = (Animation) item?.RowObject;

            if (animation == null || !Context.AnimationCommandTablesByIndex.ContainsKey(animation.ID))
                TextureView.ClearAnimation();
            else
                TextureView.StartAnimation(animation.AnimationIndex);
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

        private SpriteAnimationsViewContext _context;
        public SpriteAnimationsViewContext Context {
            get => _context;
            set {
                if (_context != value) {
                    _context = value;
                    TextureView.Context = new SpriteAnimationTextureViewContext(value?.SpriteDirections ?? 1, value?.AnimationCommandTablesByIndex, value?.FrameTable);
                    TableView.Table = _context?.AnimationTable;
                }
            }
        }

        public TableView TableView { get; private set; }
        public SpriteAnimationTextureView TextureView { get; private set; }
    }
}