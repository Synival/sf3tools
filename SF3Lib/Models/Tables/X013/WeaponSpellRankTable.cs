using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class WeaponSpellRankTable : ResourceTable<WeaponSpellRank> {
        protected WeaponSpellRankTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 4) {
        }

        public static WeaponSpellRankTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponSpellRankTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new WeaponSpellRank(Data, id, name, address));
    }
}
