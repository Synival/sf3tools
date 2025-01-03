using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class MagicBonusTable : Table<MagicBonus> {
        protected MagicBonusTable(IByteData data, string resourceFile, int address, bool has32BitValues) : base(data, resourceFile, address) {
            Has32BitValues = has32BitValues;
        }

        public static MagicBonusTable Create(IByteData data, string resourceFile, int address, bool has32BitValues) {
            var newTable = new MagicBonusTable(data, resourceFile, address, has32BitValues);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new MagicBonus(Data, id, name, address, Has32BitValues));

        public override int? MaxSize => 256;

        public bool Has32BitValues { get; }
    }
}
