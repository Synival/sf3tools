using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialTable : ResourceTable<Special> {
        protected SpecialTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 256) {
        }

        public static SpecialTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SpecialTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Special(Data, id, name, address));
    }
}
