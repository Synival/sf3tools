using System.Collections.Generic;

namespace SF3.Sprites {
    public class UniqueSpriteInfoDef {
        public void AddInfo(byte verticalOffset, byte unknown0x08, byte collisionSize, float scale) {
            AddToDict(VerticalOffsetValueCount, verticalOffset);
            AddToDict(Unknown0x08Count,         unknown0x08);
            AddToDict(CollisionSizeCount,       collisionSize);
            AddToDict(ScaleCount,               scale);
        }

        private void AddToDict<T>(Dictionary<T, int> dict, T value) {
            if (!dict.ContainsKey(value))
                dict.Add(value, 1);
            else
                dict[value]++;
        }

        public Dictionary<byte, int> VerticalOffsetValueCount = new Dictionary<byte, int>();
        public Dictionary<byte, int> Unknown0x08Count         = new Dictionary<byte, int>();
        public Dictionary<byte, int> CollisionSizeCount       = new Dictionary<byte, int>();
        public Dictionary<float, int> ScaleCount              = new Dictionary<float, int>();
    }
}
