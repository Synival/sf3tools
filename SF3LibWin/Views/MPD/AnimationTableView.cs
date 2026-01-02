using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.Animation;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class AnimationTableView : TableImageViewBase<AnimationStruct, AnimationTable, AnimationStructView> {
        public AnimationTableView(string name, AnimationTable model, INameGetterContext nameGetterContext)
        : base(name, model, nameGetterContext) {}

        protected override AnimationStructView CreateImageView(float? imageScale)
            => new AnimationStructView("Animation", null, imageScale);

        protected override void SetImage(AnimationStruct anim)
            => ImageView.Animation = anim;
    }
}