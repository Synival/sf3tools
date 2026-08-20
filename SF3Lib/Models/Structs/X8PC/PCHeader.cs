using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PCHeader : Struct, ITableContainer {
        private readonly int _battleModelChunkDefTableAddr;

        public PCHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x40) {
            _battleModelChunkDefTableAddr = Address + 0x00; // 0x40 bytes

            Tables = new ITable[] {
                ChunkDefTable = PCChunkDefTable.Create(Data, nameof(ChunkDefTable), _battleModelChunkDefTableAddr)
            };
        }

        public PCChunkDefTable ChunkDefTable { get; }

        public IEnumerable<ITable> Tables { get; }
    }
}
