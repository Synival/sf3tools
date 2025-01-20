using System;
using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class AllFramesTable : BaseTable<FrameModel> {
        protected AllFramesTable(IByteData data, int address, IEnumerable<TextureAnimationModel> animations) : base(data, address) {
            Animations = animations;
        }

        public static AllFramesTable Create(IByteData data, int address, IEnumerable<TextureAnimationModel> animations) {
            var newTable = new AllFramesTable(data, address, animations);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            _rows = Animations.SelectMany(x => x.Frames).ToArray();
            return true;
        }

        public IEnumerable<TextureAnimationModel> Animations { get; }
    }
}
