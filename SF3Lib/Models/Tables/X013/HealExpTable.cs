using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class HealExpTable : ResourceTable<HealExp> {
        protected HealExpTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 2) {
        }

        public static HealExpTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new HealExpTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new HealExp(Data, id, name, address));
    }
}
