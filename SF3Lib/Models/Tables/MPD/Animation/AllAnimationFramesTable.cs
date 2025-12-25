using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Models.Tables.MPD.Animation {
    public class AllAnimationFramesTable : Table<AnimationFrame> {
        protected AllAnimationFramesTable(IByteData data, string name, int address, IEnumerable<Structs.MPD.Animation.AnimationStruct> animations) : base(data, name, address) {
            Animations = animations;
        }

        public static AllAnimationFramesTable Create(IByteData data, string name, int address, IEnumerable<Structs.MPD.Animation.AnimationStruct> animations)
            => Create(() => new AllAnimationFramesTable(data, name, address, animations));

        public override bool Load() {
            _rows = Animations.SelectMany(x => x.AnimationFrameTable).ToArray();
            return true;
        }

        public IEnumerable<Structs.MPD.Animation.AnimationStruct> Animations { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
