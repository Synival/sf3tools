using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialEffectTable : ResourceTable<SpecialEffect> {
        protected SpecialEffectTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 500) {
        }

        public static SpecialEffectTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new SpecialEffectTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SpecialEffect(Data, id, name, address));
    }
}
