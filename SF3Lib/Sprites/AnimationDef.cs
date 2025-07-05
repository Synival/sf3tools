namespace SF3.Sprites {
    public class AnimationDef {
        public AnimationDef() { }

        public AnimationDef(UniqueAnimationDef aniInfo) : this() {
            Name = aniInfo.AnimationName;
            Hash = aniInfo.AnimationHash;
            AnimationFrames = aniInfo.AnimationFrames;
        }

        public string Name;
        public string Hash;
        public AnimationFrameDef[] AnimationFrames;
    };
}
