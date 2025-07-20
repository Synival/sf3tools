namespace SF3.Sprites {
    public class CHR_SpriteDef {
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

        public CHR_SpriteFramesDef[] SpriteFrames;
        public CHR_SpriteAnimationsDef[] SpriteAnimations;
    }
}
