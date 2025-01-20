using System;
using SF3.ByteData;
using SF3.Models.Structs.X033_X031;

namespace SF3.Models.Tables.X033_X031 {
    public class WeaponLevelTable : ResourceTable<WeaponLevel> {
        protected WeaponLevelTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 2) {
        }

        public static WeaponLevelTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponLevelTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new WeaponLevel(Data, id, name, address));
    }
}
