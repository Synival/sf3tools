using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class AITargetPathTable : FixedSizeTable<AITargetPath> {
        protected AITargetPathTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static AITargetPathTable Create(IByteData data, string name, int address)
            => Create(() => new AITargetPathTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new AITargetPath(Data, id, "Path" + id.ToString("D2"), address));
    }
}
