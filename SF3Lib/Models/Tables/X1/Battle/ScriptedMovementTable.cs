using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class ScriptedMovementTable : FixedSizeTable<ScriptedMovement> {
        protected ScriptedMovementTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static ScriptedMovementTable Create(IByteData data, string name, int address)
            => CreateBase(() => new ScriptedMovementTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new ScriptedMovement(Data, id, "ScriptedMovement" + id.ToString("D2"), address));
    }
}
