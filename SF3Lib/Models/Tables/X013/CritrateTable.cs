using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.X013 {
    public class CritrateTable : Table<Critrate> {
        protected CritrateTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static CritrateTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new CritrateTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Critrate(Data, id, name, address));

        public override int? MaxSize => 3;
    }
}
