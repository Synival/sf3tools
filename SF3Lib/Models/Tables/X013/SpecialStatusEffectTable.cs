using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialStatusEffectTable : ResourceTable<SpecialStatusEffect> {
        protected SpecialStatusEffectTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 500) {
        }

        public static SpecialStatusEffectTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new SpecialStatusEffectTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new SpecialStatusEffect(Data, id, name, address));
    }
}
