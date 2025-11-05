using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteFramesView : TableTextureView<Frame, FrameTable> {
        public SpriteFramesView(string name, FrameTable table, INameGetterContext nameGetterContext) : base(name, table, nameGetterContext) {}

        protected override ITexture GetTextureFromModel(Frame frame)
            => frame?.Texture;
    }
}