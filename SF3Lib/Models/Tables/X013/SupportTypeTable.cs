using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SupportTypeTable : ResourceTable<SupportType> {
        protected SupportTypeTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 120) {
        }

        public static SupportTypeTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SupportTypeTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SupportType(Data, id, name, address));
    }
}
