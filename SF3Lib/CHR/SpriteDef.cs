namespace SF3.CHR {
    public class SpriteDef {
        public string SpriteName;

        public int SpriteID;
        public int Width;
        public int Height;
        public int Directions;
        public int? PromotionLevel;

        public int VerticalOffset;
        public int Unknown0x08;
        public int CollisionSize;
        public float? Scale;

        public SpriteFramesDef[] SpriteFrames;
        public SpriteAnimationsDef[] SpriteAnimations;
    }
}
