using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class BattleModelHeader : Struct, ITableContainer {
        private readonly int _battleModelChunkDefTableAddr;

        public BattleModelHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x40) {
            _battleModelChunkDefTableAddr = Address + 0x00; // 0x40 bytes

            Tables = new ITable[] {
                BattleModelChunkDefTable = BattleModelChunkDefTable.Create(Data, nameof(BattleModelChunkDefTable), _battleModelChunkDefTableAddr)
            };
        }

        public BattleModelChunkDefTable BattleModelChunkDefTable { get; }

        public IEnumerable<ITable> Tables { get; }
    }
}
