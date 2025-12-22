using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Models.Tables.MPD.TextureAnimation {
    public class TextureAnimationFrameTable : TerminatedTable<TextureAnimationFrameModel> {
        protected TextureAnimationFrameTable(IByteData data, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, IMPD_File mpdFile)
        : base(data, name, address, is32Bit ? 4 : 2, null) {
            Is32Bit   = is32Bit;
            TexID     = texId;
            Width     = width;
            Height    = height;
            TexAnimID = texAnimId;
            MPD_File  = mpdFile;
            _frameEndOffset = Is32Bit ? 0xFFFF_FFFE : 0xFFFE;
        }

        public static TextureAnimationFrameTable Create(IByteData data, string name, int address, bool is32Bit, int texId, int width, int height, int texAnimId, IMPD_File mpdFile)
            => Create(() => new TextureAnimationFrameTable(data, name, address, is32Bit, texId, width, height, texAnimId, mpdFile));

        public override bool Load() {
            return Load(
                (id, address) => new TextureAnimationFrameModel(
                    Data, TexID, "TexAnim" + TexAnimID + "_" + (id + 1), address, Is32Bit, Width, Height, TexAnimID, id + 1, MPD_File
                ),
                (currentRows, model) => (uint) model.ImageDataOffset != _frameEndOffset,
                false);
        }

        private uint _frameEndOffset;

        public bool Is32Bit { get; }
        public int TexID { get; }
        public int Width { get; }
        public int Height { get; }
        public int TexAnimID { get; }
        public IMPD_File MPD_File { get; }
    }
}
