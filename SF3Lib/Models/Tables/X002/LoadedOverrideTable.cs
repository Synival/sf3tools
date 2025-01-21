using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class LoadedOverrideTable : ResourceTable<LoadedOverride> {
        protected LoadedOverrideTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static LoadedOverrideTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new LoadedOverrideTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new LoadedOverride(Data, id, name, address));
    }
}
