using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class WeaponRankTable : ResourceTable<WeaponRank> {
        protected WeaponRankTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 5) {
        }

        public static WeaponRankTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponRankTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new WeaponRank(Data, id, name, address));
    }
}
