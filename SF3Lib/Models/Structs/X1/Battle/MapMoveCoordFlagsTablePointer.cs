using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class MapMoveCoordFlagsTablePointer : Struct {
        private readonly int _pointerAddr;

        public MapMoveCoordFlagsTablePointer(IByteData data, int id, string name, int address, MapLeaderType mapLeader, MapMoveCoordFlagsPointerTable allWarps)
        : base(data, id, name, address, 0x04) {
            MapLeader = mapLeader;
            AllWarps  = allWarps;

            _pointerAddr = Address; // 2 bytes

            UpdateWarps();
        }

        [TableViewModelColumn(addressField: nameof(_pointerAddr), displayOrder: 0, isPointer: true, minWidth: 80)]
        [BulkCopy]
        public int Pointer {
            get => Data.GetInt32(_pointerAddr);
            set => Data.SetInt32(_pointerAddr, value);
        }

        private void UpdateWarps() {
            if (Pointer == 0) {
                _mapMoveCoordFlagsTable = null;
                return;
            }

            if (_mapMoveCoordFlagsTable != null && Pointer == _mapMoveCoordFlagsTable.Address)
                return;

            _mapMoveCoordFlagsTable = MapMoveCoordFlagsTable.Create(Data, $"MapMoveFlags_{MapLeader}", Pointer - AllWarps.RamAddress, MapLeader);
        }

        public MapLeaderType MapLeader { get; }
        public MapMoveCoordFlagsPointerTable AllWarps { get; }

        private MapMoveCoordFlagsTable _mapMoveCoordFlagsTable = null;
        public MapMoveCoordFlagsTable MapMoveCoordFlagsTable => _mapMoveCoordFlagsTable;
    }
}
