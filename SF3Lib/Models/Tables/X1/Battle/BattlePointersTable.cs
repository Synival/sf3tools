using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.X1.Battle {
    public class BattlePointersTable : Table<BattlePointers> {
        protected BattlePointersTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static BattlePointersTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new BattlePointersTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new BattlePointers(Data, id, name, address));

        public override int? MaxSize => 5;
    }
}
