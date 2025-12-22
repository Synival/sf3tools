using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class TextureAnimationTable : TerminatedTable<TextureAnimation> {
        protected TextureAnimationTable(IByteData data, string name, int address, bool is32Bit, IMPD_File mpdFile)
        : base(data, name, address, is32Bit ? 8 : 4, null) {
            Is32Bit      = is32Bit;
            MPD_File     = mpdFile;
            FrameEndId   = is32Bit ? 0xFFFF_FFFE : 0xFFFE;
            TextureEndId = is32Bit ? 0xFFFF_FFFF : 0xFFFF;
        }

        public static TextureAnimationTable Create(IByteData data, string name, int address, bool is32Bit, IMPD_File mpdFile)
            => Create(() => new TextureAnimationTable(data, name, address, is32Bit, mpdFile));

        public override bool Load() {
            return Load(
                (id, address) => {
                    // For some reason, Scenario 2's SARA23.MPD's final animation has an ID of 0xFFFE instead of 0xFFFF like
                    // everything else. No clue why, but let's consider that the end as well.
                    var textureId = Data.GetData(address, Is32Bit ? 4 : 2);
                    var atEnd = textureId == FrameEndId || textureId == TextureEndId;
                    return new TextureAnimation(Data, id, atEnd ? "--" : "TexAnim" + id, address, Is32Bit, MPD_File);
                },
                (currentRows, model) => model.TextureIDRaw != FrameEndId && model.TextureIDRaw != TextureEndId, addEndModel: false);
        }

        public bool Is32Bit { get; }
        public IMPD_File MPD_File { get; }
        public uint FrameEndId { get; }
        public uint TextureEndId { get; }
    }
}
