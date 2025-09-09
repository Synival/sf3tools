using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class AITargetPositionTable : FixedSizeTable<AITargetPosition> {
        protected AITargetPositionTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static AITargetPositionTable Create(IByteData data, string name, int address)
            => Create(() => new AITargetPositionTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new AITargetPosition(Data, id, "AITargetPosition" + id.ToString("D2"), address));
    }
}
