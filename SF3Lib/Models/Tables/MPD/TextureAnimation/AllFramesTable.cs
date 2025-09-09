using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class AllFramesTable : Table<FrameModel> {
        protected AllFramesTable(IByteData data, string name, int address, IEnumerable<TextureAnimationModel> animations) : base(data, name, address) {
            Animations = animations;
        }

        public static AllFramesTable Create(IByteData data, string name, int address, IEnumerable<TextureAnimationModel> animations)
            => CreateBase(() => new AllFramesTable(data, name, address, animations));

        public override bool Load() {
            _rows = Animations.SelectMany(x => x.FrameTable).ToArray();
            return true;
        }

        public IEnumerable<TextureAnimationModel> Animations { get; }
        public override int TerminatorSize => 0;
        public override bool IsContiguous => false;
    }
}
