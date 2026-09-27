using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCBoneKeyframesTable : TerminatedTable<PCBoneKeyframesStruct> {
        protected PCBoneKeyframesTable(IByteData data, string name, int address, bool hasFixedRotations)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
            HasFixedRotations = hasFixedRotations;
        }

        public bool HasFixedRotations { get; }

        public static PCBoneKeyframesTable Create(IByteData data, string name, int address, bool hasFixedRotations)
            => Create(() => new PCBoneKeyframesTable(data, name, address, hasFixedRotations));

        public override bool Load() {
            return Load(
                (id, addr) => new PCBoneKeyframesStruct(Data, id, $"Bone{id:D2}_Keyframes", addr, HasFixedRotations),
                (rows, prevRow) => (int) prevRow.NumPosKeyFrames != -1,
                false
            );
        }
    }
}
