using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationsView : ControlSpaceView {
        public SpriteAnimationsView(string name, AnimationTable model, Dictionary<int, AnimationFrameTable> animationFramesByIndex, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            AnimationFramesByIndex = animationFramesByIndex;

            TableView   = new TableView("Frames", model, nameGetterContext, typeof(Animation));
            TextureView = new SpriteAnimationTextureView("Texture", textureScale: 2);
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
            var animation = (Animation) item?.RowObject;

            if (animation == null)
                TextureView.Image = null;
            else
                StartAnimation(animation.AnimIndex);
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

        private void StartAnimation(int index) {
            var frames = AnimationFramesByIndex.ContainsKey(index) ? AnimationFramesByIndex[index] : null;
            var firstFrame = frames?.Length > 0 ? frames[0] : null;
            var texture = firstFrame?.Texture;

            TextureView.Image = texture;
        }

        public AnimationTable Model { get; }
        public Dictionary<int, AnimationFrameTable> AnimationFramesByIndex { get; }
        public TableView TableView { get; private set; }
        public AnimatedTextureView TextureView { get; private set; }
    }
}