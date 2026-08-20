using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCTextureTable : FixedSizeTable<PCTexture> {
        protected PCTextureTable(IByteData data, IByteArray texData, string name, int address, int size) : base(data, name, address, size) {
            TexData = texData;
        }

        public static PCTextureTable Create(IByteData data, IByteArray texData, string name, int address, int size)
            => Create(() => new PCTextureTable(data, texData, name, address, size));

        public override bool Load()
            => Load((id, address) => new PCTexture(Data, TexData, id, $"Texture{id:D3}", address));

        public IByteArray TexData { get; }
    }
}
