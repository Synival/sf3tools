namespace SF3.Sprites {
    /// <summary>
    /// Serializable defintion of animations by sprite family and variant
    /// </summary>
    public struct UniqueSpriteAnimationCollectionDef {
        public string Name;

        public struct Variant {
            public int Width;
            public int Height;
            public int Directions;

            public struct Animation {
                public string Name;
                public string Hash;

                public struct Frame {
                    public string[] FrameHashes;
                    public int Command;
                    public int ParameterOrDuration;
                };
                public Frame[] Frames;
            };
            public Animation[] Animations;
        }

        public Variant[] Variants;
    }
}
