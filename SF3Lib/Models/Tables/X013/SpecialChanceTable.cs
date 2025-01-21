using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SpecialChanceTable : ResourceTable<SpecialChance> {
        protected SpecialChanceTable(IByteData data, string name, string resourceFile, int address, bool hasLargeTable) : base(data, name, resourceFile, address, 1) {
            HasLargeTable = hasLargeTable;
        }

        public static SpecialChanceTable Create(IByteData data, string name, string resourceFile, int address, bool hasLargeTable) {
            var newTable = new SpecialChanceTable(data, name, resourceFile, address, hasLargeTable);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SpecialChance(Data, id, name, address, HasLargeTable));

        public bool HasLargeTable { get; }
    }
}
