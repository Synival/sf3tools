using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class BattlePointersTable : ResourceTable<BattlePointers> {
        protected BattlePointersTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 5) {
        }

        public static BattlePointersTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new BattlePointersTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new BattlePointers(Data, id, name, address));
    }
}
