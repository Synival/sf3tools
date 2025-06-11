using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class BattleTalkTable : TerminatedTable<BattleTalk> {
        protected BattleTalkTable(IByteData data, string name, int address) : base(data, name, address, 4, 100) {
        }

        public static BattleTalkTable Create(IByteData data, string name, int address) {
            var newTable = new BattleTalkTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new BattleTalk(Data, id, $"{nameof(BattleTalk)}{id:D2}", address),
                (rows, model) => (uint) model.CharacterID != 0xFFFFFFFF,
                false);
    }
}
