using System.Linq;
using SF3.Types;

namespace SF3.CHR {
    public class FrameDef {
        public override string ToString()
            => Direction.ToString() + (DuplicateKey != null ? $", {DuplicateKey}" : "");

        public SpriteFrameDirection Direction;
        public string DuplicateKey;
    }
}
