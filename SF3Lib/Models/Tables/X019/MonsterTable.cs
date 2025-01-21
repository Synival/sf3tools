using System;
using SF3.ByteData;
using SF3.Models.Structs.X019;

namespace SF3.Models.Tables.X019 {
    public class MonsterTable : ResourceTable<Monster> {
        protected MonsterTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static MonsterTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new MonsterTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Monster(Data, id, name, address));
    }
}
