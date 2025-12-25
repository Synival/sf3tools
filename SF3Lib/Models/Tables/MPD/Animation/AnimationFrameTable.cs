using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;

namespace SF3.Models.Tables.MPD.Animation {
    public class AnimationFrameTable : TerminatedTable<AnimationFrame> {
        protected AnimationFrameTable(IByteData data, string name, int address, bool is32Bit, IMPD_File mpdFile, Structs.MPD.Animation.AnimationStruct animation)
        : base(data, name, address, is32Bit ? 4 : 2, null) {
            Is32Bit   = is32Bit;
            MPD_File  = mpdFile;
            Animation = animation;
            _frameEndOffset = Is32Bit ? 0xFFFF_FFFE : 0xFFFE;
        }

        public static AnimationFrameTable Create(IByteData data, string name, int address, bool is32Bit, IMPD_File mpdFile, Structs.MPD.Animation.AnimationStruct animation)
            => Create(() => new AnimationFrameTable(data, name, address, is32Bit, mpdFile, animation));

        public override bool Load() {
            return Load(
                (id, address) => new AnimationFrame(Data, $"TexAnim{Animation.ID:D2}_Frame{id + 1:D2}", address, Is32Bit, id + 1, MPD_File, Animation),
                (currentRows, model) => (uint) model.ImageDataOffset != _frameEndOffset,
                false);
        }

        private uint _frameEndOffset;

        public bool Is32Bit { get; }
        public IMPD_File MPD_File { get; }
        public Structs.MPD.Animation.AnimationStruct Animation { get; }
    }
}
