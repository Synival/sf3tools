using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCAnimationDefTable : TerminatedTable<PCAnimationDefStruct> {
        protected PCAnimationDefTable(IByteData data, string name, int address)
        : base(data, name, address, 2, null) {
        }

        public static PCAnimationDefTable Create(IByteData data, string name, int address)
            => Create(() => new PCAnimationDefTable(data, name, address));

        public override bool Load() {
            PCAnimationDefStruct NeighborGetter(int id) => (id >= 0 && id < _rows.Length) ? _rows[id] : null;
            return Load(
                (id, address) => new PCAnimationDefStruct(Data, id, $"Animation_{id:D2}", address, NeighborGetter),
                (rowsLoaded, thisRow) => Data.GetUInt16(thisRow.Address) != 0xFFFF,
                addEndModel: false
            );
        }
    }
}
