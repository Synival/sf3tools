using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.Views.MPD
{
    public class MultiChunkImageView : ImageView {
        public MultiChunkImageView(string name, MultiChunkImage twoChunkImage) : base(name, twoChunkImage.FullTexture.CreateBitmapARGB1555(), 1) {
            MultiChunkImage = twoChunkImage;
        }

        public MultiChunkImage MultiChunkImage { get; }
    }
}
