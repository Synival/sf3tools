using System.Collections.Generic;

namespace SF3.Sprites {
    public class UniqueSpriteInfoDef {
        public void AddInfo(int spriteId, byte verticalOffset, byte unknown0x08, byte collisionSize, float scale, bool isCharacterSprite) {
            AddToDict(SpriteIDCount,       spriteId,       isCharacterSprite);
            AddToDict(VerticalOffsetCount, verticalOffset, isCharacterSprite);
            AddToDict(Unknown0x08Count,    unknown0x08,    isCharacterSprite);
            AddToDict(CollisionSizeCount,  collisionSize,  isCharacterSprite);
            AddToDict(ScaleCount,          scale,          isCharacterSprite);
        }

        private void AddToDict<T>(Dictionary<T, int> dict, T value, bool isCharacterSprite) {
            int amount = isCharacterSprite ? 1000 : 1;
            if (!dict.ContainsKey(value))
                dict.Add(value, amount);
            else
                dict[value] += amount;
        }

        public Dictionary<int, int>   SpriteIDCount       = new Dictionary<int, int>();
        public Dictionary<byte, int>  VerticalOffsetCount = new Dictionary<byte, int>();
        public Dictionary<byte, int>  Unknown0x08Count    = new Dictionary<byte, int>();
        public Dictionary<byte, int>  CollisionSizeCount  = new Dictionary<byte, int>();
        public Dictionary<float, int> ScaleCount          = new Dictionary<float, int>();
    }
}
