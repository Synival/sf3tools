using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD
{
    public class TwoChunkImageView : TextureView {
        public TwoChunkImageView(string name, TwoChunkImage twoChunkImage) : base(name, twoChunkImage.FullTexture.CreateBitmap(), 1) {
            TwoChunkImage = twoChunkImage;
        }

        public TwoChunkImage TwoChunkImage { get; }
    }
}
