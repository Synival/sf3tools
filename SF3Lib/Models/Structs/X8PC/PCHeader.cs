using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PCHeader : Struct, ITableContainer {
        private readonly int _chunkDefTableAddr;
        private readonly int _extraAnimChunkDefTableAddr;

        public PCHeader(IByteData data, int id, string name, int address, bool hasAnimations)
        : base(data, id, name, address, 0x40) {
            _chunkDefTableAddr          = Address + 0x00; // 0x40 bytes
            _extraAnimChunkDefTableAddr = Address + 0x40; // n bytes

            var tables = new List<ITable>() { (ChunkDefTable = PCChunkDefTable.Create(Data, nameof(ChunkDefTable), _chunkDefTableAddr)) };
            if (hasAnimations)
                tables.Add(ExtraAnimChunkDefTable = PCExtraAnimChunkDefTable.Create(Data, nameof(PCExtraAnimChunkDefTable), _extraAnimChunkDefTableAddr));

            Size = ChunkDefTable.SizeInBytes + (hasAnimations ? ExtraAnimChunkDefTable.SizeInBytes : 0);
            Tables = tables.ToArray();
        }

        public PCChunkDefTable ChunkDefTable { get; }
        public PCExtraAnimChunkDefTable ExtraAnimChunkDefTable { get; }

        public IEnumerable<ITable> Tables { get; }
    }
}
