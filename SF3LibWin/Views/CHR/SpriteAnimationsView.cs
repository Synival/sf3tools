using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationsView : ControlSpaceView {
        public SpriteAnimationsView(string name, int spriteDirections, AnimationTable model, Dictionary<int, AnimationFrameTable> animationFramesByIndex, INameGetterContext nameGetterContext) : base(name) {
            Model                 = model;
            SpriteDirections      = spriteDirections;
            AnimationFramesByIndex = animationFramesByIndex;

            TableView   = new TableView("Frames", model, nameGetterContext, typeof(Animation));
            TextureView = new SpriteAnimationTextureView("Texture", new SpriteAnimationTextureViewContext(spriteDirections, animationFramesByIndex, model.FrameTable), textureScale: 2);
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

            if (animation == null || !AnimationFramesByIndex.ContainsKey(animation.ID))
                TextureView.ClearAnimation();
            else
                TextureView.StartAnimation(animation.ID);
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
        public int SpriteDirections { get; }
        public Dictionary<int, AnimationFrameTable> AnimationFramesByIndex { get; }
        public TableView TableView { get; private set; }
        public SpriteAnimationTextureView TextureView { get; private set; }
    }
}