using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.Types;

namespace SF3.Imaging {
    public class MPD_MockAnimationFrame : MockAnimatedTextureFrame, IMPD_AnimationFrame, IDisposable {
        public MPD_MockAnimationFrame(IMPD_Texture texture) : base(texture) {}

        public MPD_CollectionType Collection => (MPD_CollectionType) TextureCollectionID;
        public Dictionary<TagKey, TagValue> Tags => (_texture as IMPD_Texture)?.Tags;
        public bool IsIgnored => false;
    }
}
