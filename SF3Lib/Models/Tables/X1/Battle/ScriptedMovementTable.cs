using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class ScriptedMovementTable : FixedSizeTable<ScriptedMovement> {
        protected ScriptedMovementTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static ScriptedMovementTable Create(IByteData data, string name, int address) {
            var newTable = new ScriptedMovementTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ScriptedMovement(Data, id, "ScriptedMovement" + id.ToString("D2"), address));
    }
}
