using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class LoadingOverrideTable : ResourceTable<LoadingOverride> {
        protected LoadingOverrideTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static LoadingOverrideTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new LoadingOverrideTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new LoadingOverride(Data, id, name, address));
    }
}
