using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class AITargetLocationTable : FixedSizeTable<AITargetLocation> {
        protected AITargetLocationTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static AITargetLocationTable Create(IByteData data, string name, int address)
            => Create(() => new AITargetLocationTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new AITargetLocation(Data, id, "Location" + id.ToString("D2"), address));
    }
}
