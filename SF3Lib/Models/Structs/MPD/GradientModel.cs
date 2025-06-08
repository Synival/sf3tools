using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class GradientModel : Struct {
        private readonly int _startPositionAddr;
        private readonly int _stopPositionAddr;
        private readonly int _startRAddr;
        private readonly int _startGAddr;
        private readonly int _startBAddr;
        private readonly int _stopRAddr;
        private readonly int _stopGAddr;
        private readonly int _stopBAddr;
        private readonly int _partsAffectedBitsAddr;
        private readonly int _groundOpacityAddr;
        private readonly int _skyBoxOpacityAddr;
        private readonly int _modelsAndTilesOpacityAddr;

        public GradientModel(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _startPositionAddr         = Address + 0x00; // 2 bytes
            _stopPositionAddr          = Address + 0x02; // 2 bytes
            _startRAddr                = Address + 0x04; // 2 bytes
            _startGAddr                = Address + 0x06; // 2 bytes
            _startBAddr                = Address + 0x08; // 2 bytes
            _stopRAddr                 = Address + 0x0A; // 2 bytes
            _stopGAddr                 = Address + 0x0C; // 2 bytes
            _stopBAddr                 = Address + 0x0E; // 2 bytes
            _partsAffectedBitsAddr     = Address + 0x10; // 2 bytes
            _groundOpacityAddr         = Address + 0x12; // 2 bytes
            _skyBoxOpacityAddr         = Address + 0x14; // 2 bytes
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
        [TableViewModelColumn(addressField: nameof(_startRAddr), displayOrder: 2, displayFormat: "X2")]
        public ushort StartR {
            get => (ushort) Data.GetWord(_startRAddr);
            set => Data.SetWord(_startRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_startGAddr), displayOrder: 3, displayFormat: "X2")]
        public ushort StartG {
            get => (ushort) Data.GetWord(_startGAddr);
            set => Data.SetWord(_startGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_startBAddr), displayOrder: 4, displayFormat: "X2")]
        public ushort StartB {
            get => (ushort) Data.GetWord(_startBAddr);
            set => Data.SetWord(_startBAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_stopRAddr), displayOrder: 5, displayFormat: "X2")]
        public ushort StopR {
            get => (ushort) Data.GetWord(_stopRAddr);
            set => Data.SetWord(_stopRAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_stopGAddr), displayOrder: 6, displayFormat: "X2")]
        public ushort StopG {
            get => (ushort) Data.GetWord(_stopGAddr);
            set => Data.SetWord(_stopGAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_stopBAddr), displayOrder: 7, displayFormat: "X2")]
        public ushort StopB {
            get => (ushort) Data.GetWord(_stopBAddr);
            set => Data.SetWord(_stopBAddr, value);
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
            set => PartsAffectedBits = (ushort) ((PartsAffectedBits & ~0x01) | (value ? 0x01 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.2f)]
        public bool AffectsSkyBox {
            get => (PartsAffectedBits & 0x02) == 0x02;
            set => PartsAffectedBits = (ushort) ((PartsAffectedBits & ~0x02) | (value ? 0x02 : 0x00));
        }

        [TableViewModelColumn(addressField: null, displayOrder: 8.3f)]
        public bool AffectsModelsAndTiles {
            get => (PartsAffectedBits & 0x04) == 0x04;
            set => PartsAffectedBits = (ushort) ((PartsAffectedBits & ~0x04) | (value ? 0x04 : 0x00));
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_groundOpacityAddr), displayOrder: 9, displayFormat: "X2")]
        public ushort GroundOpacity {
            get => (ushort) Data.GetWord(_groundOpacityAddr);
            set => Data.SetWord(_groundOpacityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_skyBoxOpacityAddr), displayOrder: 10, displayFormat: "X2")]
        public ushort SkyBoxOpacity {
            get => (ushort) Data.GetWord(_skyBoxOpacityAddr);
            set => Data.SetWord(_skyBoxOpacityAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_modelsAndTilesOpacityAddr), displayOrder: 11, displayFormat: "X2")]
        public ushort ModelsAndTilesOpacity {
            get => (ushort) Data.GetWord(_modelsAndTilesOpacityAddr);
            set => Data.SetWord(_modelsAndTilesOpacityAddr, value);
        }
    }
}
