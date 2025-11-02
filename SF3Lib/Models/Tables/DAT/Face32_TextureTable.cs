using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.DAT;

namespace SF3.Models.Tables.DAT {
    public class Face32_TextureTable : FixedSizeTable<TextureModelBase> {
        protected Face32_TextureTable(IByteData data, string name, int address, INameGetterContext nameGetterContext,
            int faceCount, Palette palette)
        : base(data, name, address, faceCount) {
            NameGetterContext = nameGetterContext;
            Palette = palette;
        }

        public static Face32_TextureTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext,
            int faceCount, Palette palette)
        {
            return Create(() => new Face32_TextureTable(data, name, address, nameGetterContext, faceCount, palette));
        }

        public override bool Load() {
            // Determine whether or not this table has compressed textures.
            IsCompressed = false;
            for (int i = 0; i < Size; i++) {
                // Test the first image to see if it's compressed.
                var firstOffset = Data.GetDouble(Address + i * 0x04);
                if (firstOffset != -1) {
                    var decompressedFirstChunk = Compression.DecompressLZSS(Data.Data.GetDataCopyOrReference(), firstOffset, null, out var _bytesRead, out var endDataFound);
                    IsCompressed = decompressedFirstChunk.Length == (32 * 32);
                    break;
                }
            }
            return Load((id, addr) => new Face32_TextureModel(Data, id, $"FaceTexture_{id:D3}", addr, Palette, IsCompressed.Value));
        }

        public INameGetterContext NameGetterContext { get; }
        public Palette Palette { get; }
        public bool? IsCompressed { get; private set; }
    }
}
