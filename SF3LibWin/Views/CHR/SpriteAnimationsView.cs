using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Types;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationsView : ControlSpaceView {
        public SpriteAnimationsView(string name, AnimationTable model, Dictionary<AnimationType, AnimationFrameTable> animationFramesByIndex, INameGetterContext nameGetterContext) : base(name) {
            Model = model;
            AnimationFramesByType = animationFramesByIndex;

            TableView   = new TableView("Frames", model, nameGetterContext, typeof(Animation));
            TextureView = new SpriteAnimationTextureView("Texture", animationFramesByIndex.Values.ToArray(), textureScale: 2);
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

            if (animation == null || !AnimationFramesByType.ContainsKey(animation.AnimationType))
                TextureView.ClearAnimation();
            else
                TextureView.StartAnimation((int) animation.AnimationType);
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
        public Dictionary<AnimationType, AnimationFrameTable> AnimationFramesByType { get; }
        public TableView TableView { get; private set; }
        public SpriteAnimationTextureView TextureView { get; private set; }
    }
}