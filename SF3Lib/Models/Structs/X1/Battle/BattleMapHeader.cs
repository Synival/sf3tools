using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleMapHeader : Struct {
        private readonly int _numUnitsAddr;
        private readonly int _numZonesAddr;
        private readonly int _numLocationsAddr;
        private readonly int _numPathsAddr;
        private readonly int _numMapMoveCoordsAddr;

        public BattleMapHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0A) {
            _numUnitsAddr         = Address + 0x00; // 2 bytes
            _numZonesAddr         = Address + 0x02; // 2 bytes
            _numLocationsAddr     = Address + 0x04; // 2 bytes
            _numPathsAddr         = Address + 0x06; // 2 bytes
            _numMapMoveCoordsAddr = Address + 0x08; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_numUnitsAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public ushort NumUnits {
            get => Data.GetUInt16(_numUnitsAddr);
            set => Data.SetUInt16(_numUnitsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numZonesAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public ushort NumZones {
            get => Data.GetUInt16(_numZonesAddr);
            set => Data.SetUInt16(_numZonesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numLocationsAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public ushort NumLocations {
            get => Data.GetUInt16(_numLocationsAddr);
            set => Data.SetUInt16(_numLocationsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numPathsAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public ushort NumPaths {
            get => Data.GetUInt16(_numPathsAddr);
            set => Data.SetUInt16(_numPathsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numMapMoveCoordsAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public ushort NumMapMoveCoords {
            get => Data.GetUInt16(_numMapMoveCoordsAddr);
            set => Data.SetUInt16(_numMapMoveCoordsAddr, (byte) value);
        }
    }
}
