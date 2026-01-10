using SF3.ByteData;
using SF3.Models.Structs.MPD.Main;

namespace SF3.Models.Tables.MPD.Main {
    public class GradientTable : TerminatedTable<Gradient> {
        protected GradientTable(IByteData data, string name, int address) : base(data, name, address, 2, null) { }

        public static GradientTable Create(IByteData data, string name, int address)
            => Create(() => new GradientTable(data, name, address));

        public override bool Load() {
            return Load(
                (id, address) => new Gradient(Data, id, "Gradient" + id.ToString("D2"), address),
                (rows, lastRow) => lastRow.TopPositionRaw != 0xFFFF,
                addEndModel: false
            );
        }
    }
}
