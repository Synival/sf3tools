using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PCChunkDefTable : TerminatedTable<PCChunkDef> {
        private static readonly string[] c_chunkNames = {
            "TexDefs",
            "Textures",
            "Meshes",
            "Animations",
        };

        protected PCChunkDefTable(IByteData data, string name, int address, bool hasAnimations)
        : base(data, name, address, hasAnimations ? 4 : 0, hasAnimations ? (int?) null : 0x04) {
            HasAnimations = hasAnimations;
        }

        public static PCChunkDefTable Create(IByteData data, string name, int address, bool hasAnimations)
            => Create(() => new PCChunkDefTable(data, name, address, hasAnimations));

        public override bool Load()
            => Load((id, address) => new PCChunkDef(Data, id, "Chunk_" + ((id < 4) ? c_chunkNames[id] : $"ExtraAnim_{id - 3:D2}"), address),
                    (rowsLoaded, thisRow) => !HasAnimations || Data.GetUInt32(thisRow.Address) != 0xFFFFFFFF,
                    addEndModel: false);

        public bool HasAnimations { get; }
    }
}
