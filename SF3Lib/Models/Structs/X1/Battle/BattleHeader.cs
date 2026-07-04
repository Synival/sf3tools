using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct, ITableContainer {
        private readonly int _mapPointersTableAddressAddr;
        private readonly int _unknown0x04Addr;
        private readonly int _mapMoveCoordFlagsPointerTableAddressAddr;
        private readonly int _teamsCantAttackTableAddressAddr;
        private readonly int _teamsCanSupportTableAddressAddr;

        public BattleHeader(IByteData data, int id, string name, int address, bool hasLargeEnemyTable, ScenarioType scenario, int ramAddress)
        : base(data, id, name, address, 0x14) {
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario           = scenario;
            RamAddress         = ramAddress;

            _mapPointersTableAddressAddr       = address + 0x00; // 4 bytes
            _unknown0x04Addr                   = address + 0x04; // 4 bytes
            _mapMoveCoordFlagsPointerTableAddressAddr = address + 0x08; // 4 bytes
            _teamsCantAttackTableAddressAddr   = address + 0x0C; // 4 bytes
            _teamsCanSupportTableAddressAddr   = address + 0x10; // 4 bytes

            var tableList = new List<ITable>();

            if (MapPointersTableAddress != 0) {
                tableList.Add(MapPointerTable = BattleMapPointerTable.Create(
                    Data, nameof(MapPointerTable), MapPointersTableAddress - RamAddress,
                    HasLargeEnemyTable, Scenario, this
                ));
            }

            if (MapMoveCoordFlagsPointerTableAddress != 0)
                tableList.Add(MapMoveCoordFlagsPointerTable = MapMoveCoordFlagsPointerTable.Create(Data, "MapMoveCoordFlagsPointers", MapMoveCoordFlagsPointerTableAddress - RamAddress, RamAddress));
            if (TeamsCantAttackTableAddress != 0)
                tableList.Add(TeamsCantAttackTable = TeamBitmaskTable.Create(Data, "TeamCantAttack", TeamsCantAttackTableAddress - RamAddress, "TeamCantAttack"));
            if (TeamsCanSupportTableAddress != 0)
                tableList.Add(TeamsCanSupportTable = TeamBitmaskTable.Create(Data, "TeamCanSupport", TeamsCanSupportTableAddress - RamAddress, "TeamCanSupport"));

            Tables = tableList.ToArray();
        }

        [TableViewModelColumn(addressField: nameof(_mapPointersTableAddressAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int MapPointersTableAddress {
            get => Data.GetInt32(_mapPointersTableAddressAddr);
            set => Data.SetInt32(_mapPointersTableAddressAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x04Addr), displayOrder: 1, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Unknown0x04 {
            get => Data.GetInt32(_unknown0x04Addr);
            set => Data.SetInt32(_unknown0x04Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mapMoveCoordFlagsPointerTableAddressAddr), displayOrder: 2, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int MapMoveCoordFlagsPointerTableAddress {
            get => Data.GetInt32(_mapMoveCoordFlagsPointerTableAddressAddr);
            set => Data.SetInt32(_mapMoveCoordFlagsPointerTableAddressAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_teamsCantAttackTableAddressAddr), displayOrder: 3, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int TeamsCantAttackTableAddress {
            get => Data.GetInt32(_teamsCantAttackTableAddressAddr);
            set => Data.SetInt32(_teamsCantAttackTableAddressAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_teamsCanSupportTableAddressAddr), displayOrder: 4, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int TeamsCanSupportTableAddress {
            get => Data.GetInt32(_teamsCanSupportTableAddressAddr);
            set => Data.SetInt32(_teamsCanSupportTableAddressAddr, value);
        }

        public BattleMapPointerTable MapPointerTable { get; }
        public MapMoveCoordFlagsPointerTable MapMoveCoordFlagsPointerTable { get; }
        public TeamBitmaskTable TeamsCantAttackTable { get; }
        public TeamBitmaskTable TeamsCanSupportTable { get; }

        public IEnumerable<ITable> Tables { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public int RamAddress { get; }
    }
}
