using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCAttackAnimChunkDefTable : TerminatedTable<PCChunkDef> {
        protected PCAttackAnimChunkDefTable(IByteData data, string name, int address)
        : base(data, name, address, 4, null) {
        }

        public static PCAttackAnimChunkDefTable Create(IByteData data, string name, int address)
            => Create(() => new PCAttackAnimChunkDefTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new PCChunkDef(Data, id, $"AttackAnimChunk_{id}", address),
                    (rowsLoaded, thisRow) => Data.GetUInt32(thisRow.Address) != 0xFFFFFFFF,
                    addEndModel: false);
    }
}
