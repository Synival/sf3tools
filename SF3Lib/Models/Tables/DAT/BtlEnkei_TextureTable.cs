using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;

namespace SF3.Models.Tables.DAT {
    public class BtlEnkei_TextureTable : TerminatedTable<TextureModelBase> {
        protected BtlEnkei_TextureTable(IByteData data, string name, int address, INameGetterContext nameGetterContext)
        : base(data, name, address, 0x10, 100) {
            NameGetterContext = nameGetterContext;
        }

        public static BtlEnkei_TextureTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext)
            => Create(() => new BtlEnkei_TextureTable(data, name, address, nameGetterContext));

        public override bool Load()
            => Load((id, addr) => new BtlEnkei_TextureModel(Data, id, $"FaceTexture_{id:D3}", addr),
                (rows, last) => last.HasImage,
                false);

        public INameGetterContext NameGetterContext { get; }
    }
}
