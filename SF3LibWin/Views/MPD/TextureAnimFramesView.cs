using System.Drawing;
using CommonLib.NamedValues;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD {
    public class TextureAnimFramesView : TableImageView<FrameModel, AllFramesTable> {
        public TextureAnimFramesView(string name, IMPD_File model, INameGetterContext nameGetterContext)
        : base(name, CreateTable(model), nameGetterContext) {
            Model = model;
        }

        private static AllFramesTable CreateTable(IMPD_File model)
            => AllFramesTable.Create(model.TextureAnimations.Data, "AllFrames", model.TextureAnimations.Address, model.TextureAnimations);

        public override Image GetImageFromModel(FrameModel frame)
            => frame?.Texture?.CreateBitmapARGB1555();

        public IMPD_File Model { get; }
    }
}