using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Models.Tables.MPD.Animation {
    public class AllTextureAnimationFramesTable : Table<TextureAnimationFrame> {
        protected AllTextureAnimationFramesTable(IByteData data, string name, int address, IEnumerable<TextureAnimation> animations) : base(data, name, address) {
            Animations = animations;
        }

        public static AllTextureAnimationFramesTable Create(IByteData data, string name, int address, IEnumerable<TextureAnimation> animations)
            => Create(() => new AllTextureAnimationFramesTable(data, name, address, animations));

        public override bool Load() {
            _rows = Animations.SelectMany(x => x.TextureAnimationFrameTable).ToArray();
            return true;
        }

        public IEnumerable<TextureAnimation> Animations { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
