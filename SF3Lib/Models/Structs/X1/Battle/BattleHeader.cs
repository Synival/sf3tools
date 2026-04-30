using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct {
        private readonly int _numSlotsAddr;
        private readonly int _numZonesAddr;
        private readonly int _numPositionsAddr;
        private readonly int _numPathsAddr;
        private readonly int _numMapMoveCoordsAddr;

        public BattleHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0A) {
            _numSlotsAddr         = Address + 0x00; // 2 bytes
            _numZonesAddr         = Address + 0x02; // 2 bytes
            _numPositionsAddr     = Address + 0x04; // 2 bytes
            _numPathsAddr         = Address + 0x06; // 2 bytes
            _numMapMoveCoordsAddr = Address + 0x08; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_numSlotsAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int NumSlots {
            get => Data.GetWord(_numSlotsAddr);
            set => Data.SetWord(_numSlotsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numZonesAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int NumZones {
            get => Data.GetWord(_numZonesAddr);
            set => Data.SetWord(_numZonesAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numPositionsAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int NumPositions {
            get => Data.GetWord(_numPositionsAddr);
            set => Data.SetWord(_numPositionsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numPathsAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int NumPaths {
            get => Data.GetWord(_numPathsAddr);
            set => Data.SetWord(_numPathsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numMapMoveCoordsAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int NumMapMoveCoords {
            get => Data.GetWord(_numMapMoveCoordsAddr);
            set => Data.SetWord(_numMapMoveCoordsAddr, (byte) value);
        }
    }
}
