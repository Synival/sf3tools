using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;
using SF3.Models.Tables.MPD.Animation;

namespace SF3.Win.Views.MPD {
    public class TextureAnimationFramesView : TableTextureView<TextureAnimationFrame, AllTextureAnimationFramesTable> {
        public TextureAnimationFramesView(string name, IMPD_File model, INameGetterContext nameGetterContext)
        : base(name, CreateTable(model), nameGetterContext) {
            Model = model;
        }

        private static AllTextureAnimationFramesTable CreateTable(IMPD_File model)
            => AllTextureAnimationFramesTable.Create(model.TextureAnimations.Data, "AllFrames", model.TextureAnimations.Address, model.TextureAnimations);

        protected override ITextureData GetTextureFromModel(TextureAnimationFrame frame)
            => frame;

        public IMPD_File Model { get; }
    }
}