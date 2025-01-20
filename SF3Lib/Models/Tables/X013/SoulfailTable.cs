using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SoulfailTable : ResourceTable<Soulfail> {
        protected SoulfailTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 1) {
        }

        public static SoulfailTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SoulfailTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Soulfail(Data, id, name, address));
    }
}
