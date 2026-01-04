using CommonLib.NamedValues;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class AnimationFramesView : TableTextureView<AnimationFrame, AllAnimationFramesTable> {
        public AnimationFramesView(string name, IMPD_File model, INameGetterContext nameGetterContext)
        : base(name, CreateTable(model), nameGetterContext) {
            Model = model;
        }

        private static AllAnimationFramesTable CreateTable(IMPD_File model)
            => AllAnimationFramesTable.Create(model.Animations.Data, "AllFrames", model.Animations.Address, model.Animations);

        protected override ITextureData GetTextureFromModel(AnimationFrame frame)
            => frame;

        public IMPD_File Model { get; }
    }
}