using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    public class InitialInfoTable : ResourceTable<InitialInfo> {
        protected InitialInfoTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 100) {
        }

        public static InitialInfoTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new InitialInfoTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new InitialInfo(Data, id, name, address));
    }
}
