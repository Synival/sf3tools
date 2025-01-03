using System;
using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class SpellTable : Table<Spell> {
        protected SpellTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static SpellTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SpellTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Spell(Data, id, name, address));

        public override int? MaxSize => 78;
    }
}
