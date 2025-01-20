using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class StatusEffectTable : ResourceTable<StatusEffect> {
        protected StatusEffectTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 1000) {
        }

        public static StatusEffectTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new StatusEffectTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new StatusEffect(Data, id, name, address));
    }
}
