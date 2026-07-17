using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class BattleModelChunkDefTable : FixedSizeTable<BattleModelChunkDef> {
        private static readonly string[] c_chunkNames = {
            "TexDefs",
            "Textures",
            "Meshes",
            "Animations",
        };

        protected BattleModelChunkDefTable(IByteData data, string name, int address) : base(data, name, address, 0x04) {
        }

        public static BattleModelChunkDefTable Create(IByteData data, string name, int address)
            => Create(() => new BattleModelChunkDefTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new BattleModelChunkDef(Data, id, "Chunk_" + c_chunkNames[id], address));
    }
}
