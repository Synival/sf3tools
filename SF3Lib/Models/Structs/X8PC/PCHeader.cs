using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PCHeader : Struct, ITableContainer {
        private readonly int _chunkDefTableAddr;
        private readonly int _attackAnimChunkDefTableAddr;

        public PCHeader(IByteData data, int id, string name, int address, bool hasAttackAnims)
        : base(data, id, name, address, 0x40) {
            _chunkDefTableAddr           = Address + 0x00; // 0x40 bytes
            _attackAnimChunkDefTableAddr = Address + 0x40; // n bytes

            var tables = new List<ITable>() { (ChunkDefTable = PCChunkDefTable.Create(Data, nameof(ChunkDefTable), _chunkDefTableAddr)) };
            if (hasAttackAnims)
                tables.Add(AttackAnimChunkDefTable = PCAttackAnimChunkDefTable.Create(Data, nameof(PCAttackAnimChunkDefTable), _attackAnimChunkDefTableAddr));

            Size = ChunkDefTable.SizeInBytes + (hasAttackAnims ? AttackAnimChunkDefTable.SizeInBytes : 0);
            Tables = tables.ToArray();
        }

        public PCChunkDefTable ChunkDefTable { get; }
        public PCAttackAnimChunkDefTable AttackAnimChunkDefTable { get; }

        public IEnumerable<ITable> Tables { get; }
    }
}
