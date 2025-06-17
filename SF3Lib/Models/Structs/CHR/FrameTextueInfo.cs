namespace SF3.Models.Structs.CHR {
    /// <summary>
    /// Information about a particular texture read from a sprite.
    /// 
    /// </summary>
    public class FrameTextueInfo {
        public FrameTextueInfo(string textureHash, string spriteName, string animationName) {
            TextureHash   = textureHash;
            SpriteName    = spriteName;
            AnimationName = animationName;
        }

        public string TextureHash { get; }
        public string SpriteName { get; }
        public string AnimationName { get; }
    }
}
