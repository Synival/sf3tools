using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct, ITableContainer {
        private readonly int _battlePointersAddr;
        private readonly int _unknown0x04Addr;
        private readonly int _unknown0x08Addr;
        private readonly int _unknown0x0CAddr;
        private readonly int _unknown0x10Addr;

        public BattleHeader(IByteData data, int id, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, int ramAddress)
        : base(data, id, name, address, 0x14) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            RamAddress         = ramAddress;

            _battlePointersAddr = address + 0x00; // 4 bytes
            _unknown0x04Addr    = address + 0x04; // 4 bytes
            _unknown0x08Addr    = address + 0x08; // 4 bytes
            _unknown0x0CAddr    = address + 0x0C; // 4 bytes
            _unknown0x10Addr    = address + 0x10; // 4 bytes

            var tableList = new List<ITable>();

            if (MapPointersTableAddress != 0) {
                tableList.Add(MapPointerTable = BattleMapPointerTable.Create(
                    Data, nameof(MapPointerTable), ResourceFile("BattlePointersList.xml"), MapPointersTableAddress - RamAddress,
                    HasLargeEnemyTable, Scenario, RamAddress
                ));
            }

            Tables = tableList.ToArray();
        }

        [TableViewModelColumn(addressField: nameof(_battlePointersAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int MapPointersTableAddress {
            get => Data.GetDouble(_battlePointersAddr);
            set => Data.SetDouble(_battlePointersAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x04Addr), displayOrder: 1, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Unknown0x04 {
            get => Data.GetDouble(_unknown0x04Addr);
            set => Data.SetDouble(_unknown0x04Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x08Addr), displayOrder: 2, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Unknown0x08 {
            get => Data.GetDouble(_unknown0x08Addr);
            set => Data.SetDouble(_unknown0x08Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x0CAddr), displayOrder: 3, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Unknown0x0C {
            get => Data.GetDouble(_unknown0x0CAddr);
            set => Data.SetDouble(_unknown0x0CAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x10Addr), displayOrder: 4, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Unknown0x10 {
            get => Data.GetDouble(_unknown0x10Addr);
            set => Data.SetDouble(_unknown0x10Addr, value);
        }

        public BattleMapPointerTable MapPointerTable { get; }

        public IEnumerable<ITable> Tables { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public int RamAddress { get; }
    }
}
