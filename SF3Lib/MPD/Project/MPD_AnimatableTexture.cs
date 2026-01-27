using SF3.Imaging;

namespace SF3.MPD.Project {
    public class MPD_AnimatableTexture : MPD_Texture, IMPD_AnimatableTexture {
        public MPD_AnimatableTexture(IMPD_AnimatableTexture original) : base(original) {
            if (original.Animation != null)
                Animation = new MPD_Animation(this, original.Animation);
        }

        public IMPD_Animation Animation { get; }
    }
}
