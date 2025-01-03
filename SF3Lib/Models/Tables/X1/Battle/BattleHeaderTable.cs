using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class BattleHeaderTable : Table<BattleHeader> {
        protected BattleHeaderTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static BattleHeaderTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new BattleHeaderTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new BattleHeader(Data, id, name, address));

        public override int? MaxSize => 31;
    }
}
