using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class WeaponSpellTable : ResourceTable<WeaponSpell> {
        protected WeaponSpellTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 31) {
        }

        public static WeaponSpellTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponSpellTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new WeaponSpell(Data, id, name, address));
    }
}
