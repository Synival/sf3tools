using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleHeader : Struct {
        private readonly int _numSlotsAddr;
        private readonly int _numZonesAddr;
        private readonly int _numAITargetsAddr;
        private readonly int _numScriptedMovementsAddr;
        private readonly int _numMapMoveCoordsAddr;

        public BattleHeader(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x0A) {
            _numSlotsAddr             = Address + 0x00; // 2 bytes
            _numZonesAddr             = Address + 0x02; // 2 bytes
            _numAITargetsAddr         = Address + 0x04; // 2 bytes
            _numScriptedMovementsAddr = Address + 0x06; // 2 bytes
            _numMapMoveCoordsAddr     = Address + 0x08; // 2 bytes
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

        [TableViewModelColumn(addressField: nameof(_numAITargetsAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int NumAITargets {
            get => Data.GetWord(_numAITargetsAddr);
            set => Data.SetWord(_numAITargetsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numScriptedMovementsAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int NumScriptedMovements {
            get => Data.GetWord(_numScriptedMovementsAddr);
            set => Data.SetWord(_numScriptedMovementsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_numMapMoveCoordsAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int NumMapMoveCoords {
            get => Data.GetWord(_numMapMoveCoordsAddr);
            set => Data.SetWord(_numMapMoveCoordsAddr, (byte) value);
        }
    }
}
