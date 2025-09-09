using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class BattlePointersTable : ResourceTable<BattlePointers> {
        protected BattlePointersTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 5) {
        }

        public static BattlePointersTable Create(IByteData data, string name, string resourceFile, int address)
            => Create(() => new BattlePointersTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new BattlePointers(Data, id, name, address));
    }
}
