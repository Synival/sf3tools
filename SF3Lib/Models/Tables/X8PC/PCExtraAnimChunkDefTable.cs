using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCExtraAnimChunkDefTable : TerminatedTable<PCChunkDef> {
        protected PCExtraAnimChunkDefTable(IByteData data, string name, int address)
        : base(data, name, address, 4, null) {
        }

        public static PCExtraAnimChunkDefTable Create(IByteData data, string name, int address)
            => Create(() => new PCExtraAnimChunkDefTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new PCChunkDef(Data, id, "Chunk_" + $"ExtraAnim_{id + 1:D2}", address),
                    (rowsLoaded, thisRow) => Data.GetUInt32(thisRow.Address) != 0xFFFFFFFF,
                    addEndModel: false);
    }
}
