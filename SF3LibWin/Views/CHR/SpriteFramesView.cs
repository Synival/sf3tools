using System.Drawing;
using CommonLib.NamedValues;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables.CHR;
using SF3.Win.Extensions;

namespace SF3.Win.Views.CHR {
    public class SpriteFramesView : TableImageView<Frame, FrameTable> {
        public SpriteFramesView(string name, FrameTable table, INameGetterContext nameGetterContext) : base(name, table, nameGetterContext) {}

        protected override Image GetImageFromModel(Frame frame)
            => frame?.Texture?.CreateBitmapARGB1555();
    }
}