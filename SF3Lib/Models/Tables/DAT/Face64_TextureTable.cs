using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;

namespace SF3.Models.Tables.DAT {
    public class Face64_TextureTable : FixedSizeTable<TextureModelBase> {
        protected Face64_TextureTable(IByteData data, string name, int address, INameGetterContext nameGetterContext)
        : base(data, name, address, data.Length / 0x2000) {
            NameGetterContext = nameGetterContext;
        }

        public static Face64_TextureTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext)
            => Create(() => new Face64_TextureTable(data, name, address, nameGetterContext));

        public override bool Load()
            => Load((id, addr) => new Face64_TextureModel(Data, id, $"FaceTexture_{id:D3}", addr));

        public INameGetterContext NameGetterContext { get; }
    }
}
