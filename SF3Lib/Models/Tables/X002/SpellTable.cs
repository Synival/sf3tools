using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class SpellTable : ResourceTable<Spell> {
        protected SpellTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 78) {
        }

        public static SpellTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new SpellTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Spell(Data, id, name, address));
    }
}
