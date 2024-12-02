using System.Collections.Generic;
using System.Linq;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.RawData;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class AllFramesTable : Table<FrameModel> {
        public AllFramesTable(IRawData data, int address, IEnumerable<TextureAnimationModel> animations) : base(data, address) {
            Animations = animations;
        }

        public override bool Load() {
            _rows = Animations.SelectMany(x => x.Frames).ToArray();
            return true;
        }

        public IEnumerable<TextureAnimationModel> Animations { get; }
    }
}
