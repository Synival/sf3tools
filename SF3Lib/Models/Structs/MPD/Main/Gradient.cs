using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Main {
    public class Gradient : Struct {
        private readonly int _startPositionAddr;
        private readonly int _stopPositionAddr;
        private readonly int _topRAddr;
        private readonly int _topGAddr;
        private readonly int _topBAddr;
        private readonly int _bottomRAddr;
        private readonly int _bottomGAddr;
        private readonly int _bottomBAddr;
        private readonly int _partsAffectedBitsAddr;
        private readonly int _groundOpacityAddr;
        private readonly int _skyOpacityAddr;
        private readonly int _modelsAndTilesOpacityAddr;

        public Gradient(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _startPositionAddr         = Address + 0x00; // 2 bytes
            _stopPositionAddr          = Address + 0x02; // 2 bytes
            _topRAddr                  = Address + 0x04; // 2 bytes
            _topGAddr                  = Address + 0x06; // 2 bytes
            _topBAddr                  = Address + 0x08; // 2 bytes
            _bottomRAddr               = Address + 0x0A; // 2 bytes
            _bottomGAddr               = Address + 0x0C; // 2 bytes
            _bottomBAddr               = Address + 0x0E; // 2 bytes
            _partsAffectedBitsAddr     = Address + 0x10; // 2 bytes
            _groundOpacityAddr         = Address + 0x12; // 2 bytes
            _skyOpacityAddr            = Address + 0x14; // 2 bytes
            _modelsAndTilesOpacityAddr = Address + 0x16; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_startPositionAddr), displayOrder: 0, displayFormat: "X2")]
        public ushort StartPosition {
            get => (ushort) Data.GetWord(_startPositionAddr);
            set => Data.SetWord(_startPositionAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_stopPositionAddr), displayOrder: 1, displayFormat: "X2")]
        public ushort StopPosition {
            get => (ushort) Data.GetWord(_stopPositionAddr);
            set => Data.SetWord(_stopPositionAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topRAddr), displayOrder: 2, displayFormat: "X2")]
        public ushort TopR {
            get => (ushort) Data.GetWord(_topRAddr);
            set => Data.SetWord(_topRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topGAddr), displayOrder: 3, displayFormat: "X2")]
        public ushort TopG {
            get => (ushort) Data.GetWord(_topGAddr);
            set => Data.SetWord(_topGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_topBAddr), displayOrder: 4, displayFormat: "X2")]
        public ushort TopB {
            get => (ushort) Data.GetWord(_topBAddr);
            set => Data.SetWord(_topBAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomRAddr), displayOrder: 5, displayFormat: "X2")]
        public ushort BottomR {
            get => (ushort) Data.GetWord(_bottomRAddr);
            set => Data.SetWord(_bottomRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomGAddr), displayOrder: 6, displayFormat: "X2")]
        public ushort BottomG {
            get => (ushort) Data.GetWord(_bottomGAddr);
            set => Data.SetWord(_bottomGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_bottomBAddr), displayOrder: 7, displayFormat: "X2")]
        public ushort BottomB {
            get => (ushort) Data.GetWord(_bottomBAddr);
            set => Data.SetWord(_bottomBAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_partsAffectedBitsAddr), displayOrder: 8, displayFormat: "X1")]
        public ushort PartsAffectedBits {
            get => (ushort) Data.GetWord(_partsAffectedBitsAddr);
            set => Data.SetWord(_partsAffectedBitsAddr, value);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.1f)]
        public bool AffectsGround {
            get => (PartsAffectedBits & 0x01) == 0x01;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x01 | (value ? 0x01 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.2f)]
        public bool AffectsSky {
            get => (PartsAffectedBits & 0x02) == 0x02;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x02 | (value ? 0x02 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.3f)]
        public bool AffectsModelsAndTiles {
            get => (PartsAffectedBits & 0x04) == 0x04;
            set => PartsAffectedBits = (ushort) (PartsAffectedBits & ~0x04 | (value ? 0x04 : 0x00));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundOpacityAddr), displayOrder: 9, displayFormat: "X2")]
        public ushort GroundOpacity {
            get => (ushort) Data.GetWord(_groundOpacityAddr);
            set => Data.SetWord(_groundOpacityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_skyOpacityAddr), displayOrder: 10, displayFormat: "X2")]
        public ushort SkyOpacity {
            get => (ushort) Data.GetWord(_skyOpacityAddr);
            set => Data.SetWord(_skyOpacityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsAndTilesOpacityAddr), displayOrder: 11, displayFormat: "X2")]
        public ushort ModelsAndTilesOpacity {
            get => (ushort) Data.GetWord(_modelsAndTilesOpacityAddr);
            set => Data.SetWord(_modelsAndTilesOpacityAddr, value);
        }
    }
}
