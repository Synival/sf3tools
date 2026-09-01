using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCBoneKeyframeTable : TerminatedTable<PCBoneKeyframeStruct> {
        protected PCBoneKeyframeTable(IByteData data, string name, int address)
        : base(data, name, address, terminatedBytes: 4, maxSize: 1000) {
        }

        public static PCBoneKeyframeTable Create(IByteData data, string name, int address)
            => Create(() => new PCBoneKeyframeTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, addr) => new PCBoneKeyframeStruct(Data, id, $"BoneKeyframe_{id:D3}", addr),
                (rows, prevRow) => (int) prevRow.NumPosKeyFrames != -1,
                false
            );
        }
    }
}
