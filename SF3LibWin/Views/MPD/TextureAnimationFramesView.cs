using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Tables.MPD.TextureAnimation;

namespace SF3.Win.Views.MPD {
    public class TextureAnimationFramesView : TableTextureView<TextureAnimationFrameModel, AllTextureAnimationFramesTable> {
        public TextureAnimationFramesView(string name, IMPD_File model, INameGetterContext nameGetterContext)
        : base(name, CreateTable(model), nameGetterContext) {
            Model = model;
        }

        private static AllTextureAnimationFramesTable CreateTable(IMPD_File model)
            => AllTextureAnimationFramesTable.Create(model.TextureAnimations.Data, "AllFrames", model.TextureAnimations.Address, model.TextureAnimations);

        protected override ITextureData GetTextureFromModel(TextureAnimationFrameModel frame)
            => frame;

        public IMPD_File Model { get; }
    }
}