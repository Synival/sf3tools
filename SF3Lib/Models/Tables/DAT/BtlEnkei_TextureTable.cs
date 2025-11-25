using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;

namespace SF3.Models.Tables.DAT {
    public class BtlEnkei_TextureTable : TerminatedTable<TextureModelBase> {
        protected BtlEnkei_TextureTable(IByteData data, string name, int address, INameGetterContext nameGetterContext, bool headerless)
        : base(data, name, address, 0x10, 100) {
            Headerless = headerless;
            NameGetterContext = nameGetterContext;
        }

        public static BtlEnkei_TextureTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext, bool headerless)
            => Create(() => new BtlEnkei_TextureTable(data, name, address, nameGetterContext, headerless));

        public override bool Load() {
            if (Headerless) {
                const int span = 0x10000;
                return Load(
                    (id, addr) => (id * span + Address < Data.Length) ? new BtlEnkeiHeaderless_TextureModel(Data, id, $"BattleTexture_{id:D3}", id * span + Address) : null,
                    (rows, last) => last?.HasImage == true,
                    false
                );
            }
            else {
                return Load(
                    (id, addr) => new BtlEnkei_TextureModel(Data, id, $"BattleTexture_{id:D3}", addr),
                    (rows, last) => last?.HasImage == true,
                    false
                );
            }
        }

        public bool Headerless { get; }
        public INameGetterContext NameGetterContext { get; }
    }
}
