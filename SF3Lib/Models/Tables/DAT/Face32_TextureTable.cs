using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;

namespace SF3.Models.Tables.DAT {
    public class Face32_TextureTable : FixedSizeTable<TextureModelBase> {
        protected Face32_TextureTable(IByteData data, string name, int address, INameGetterContext nameGetterContext,
            int faceCount, Palette palette, bool isCompressed)
        : base(data, name, address, faceCount) {
            NameGetterContext = nameGetterContext;
            Palette = palette;
            IsCompressed = isCompressed;
        }

        public static Face32_TextureTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext,
            int faceCount, Palette palette, bool isCompressed)
        {
            return Create(() => new Face32_TextureTable(data, name, address, nameGetterContext, faceCount, palette, isCompressed));
        }

        public override bool Load()
            => Load((id, addr) => new Face32_TextureModel(Data, id, $"FaceTexture_{id:D3}", addr, Palette, IsCompressed));

        public INameGetterContext NameGetterContext { get; }
        public Palette Palette { get; }
        public bool IsCompressed { get; }
    }
}
