using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Spell : Struct {
        private readonly int _spellTargetAddr;
        private readonly int _spellTypeAddr;
        private readonly int _elementAddr;
        private readonly int _iconHiddenAddr;
        private readonly int _lv1DistanceAddr;
        private readonly int _lv1TargetsAddr;
        private readonly int _lv1CostAddr;
        private readonly int _lv1DamageAddr;
        private readonly int _lv2DistanceAddr;
        private readonly int _lv2TargetsAddr;
        private readonly int _lv2CostAddr;
        private readonly int _lv2DamageAddr;
        private readonly int _lv3DistanceAddr;
        private readonly int _lv3TargetsAddr;
        private readonly int _lv3CostAddr;
        private readonly int _lv3DamageAddr;
        private readonly int _lv4DistanceAddr;
        private readonly int _lv4TargetsAddr;
        private readonly int _lv4CostAddr;
        private readonly int _lv4DamageAddr;

        public Spell(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            _spellTargetAddr = Address;
            _spellTypeAddr   = Address + 1;
            _elementAddr     = Address + 2;
            _iconHiddenAddr  = Address + 3;
            _lv1DistanceAddr = Address + 4;
            _lv1TargetsAddr  = Address + 5;
            _lv1CostAddr     = Address + 6;
            _lv1DamageAddr   = Address + 7;
            _lv2DistanceAddr = Address + 8;
            _lv2TargetsAddr  = Address + 9;
            _lv2CostAddr     = Address + 10;
            _lv2DamageAddr   = Address + 11;
            _lv3DistanceAddr = Address + 12;
            _lv3TargetsAddr  = Address + 13;
            _lv3CostAddr     = Address + 14;
            _lv3DamageAddr   = Address + 15;
            _lv4DistanceAddr = Address + 16;
            _lv4TargetsAddr  = Address + 17;
            _lv4CostAddr     = Address + 18;
            _lv4DamageAddr   = Address + 19;
        }

        [TableViewModelColumn(addressField: nameof(_spellTargetAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpellTarget)]
        public int SpellTarget {
            get => Data.GetByte(_spellTargetAddr);
            set => Data.SetByte(_spellTargetAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellTypeAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int SpellType {
            get => Data.GetByte(_spellTypeAddr);
            set => Data.SetByte(_spellTypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_elementAddr), displayOrder: 2, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Element)]
        public int Element {
            get => Data.GetByte(_elementAddr);
            set => Data.SetByte(_elementAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_iconHiddenAddr), displayOrder: 3)]
        [BulkCopy]
        public int IconHidden {
            get => Data.GetByte(_iconHiddenAddr);
            set => Data.SetByte(_iconHiddenAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1DistanceAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int Lv1Distance {
            get => Data.GetByte(_lv1DistanceAddr);
            set => Data.SetByte(_lv1DistanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1TargetsAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int Lv1Targets {
            get => Data.GetByte(_lv1TargetsAddr);
            set => Data.SetByte(_lv1TargetsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1CostAddr), displayOrder: 6)]
        [BulkCopy]
        public int Lv1Cost {
            get => Data.GetByte(_lv1CostAddr);
            set => Data.SetByte(_lv1CostAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1DamageAddr), displayOrder: 7)]
        [BulkCopy]
        public int Lv1Damage {
            get => Data.GetByte(_lv1DamageAddr);
            set => Data.SetByte(_lv1DamageAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2DistanceAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int Lv2Distance {
            get => Data.GetByte(_lv2DistanceAddr);
            set => Data.SetByte(_lv2DistanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2TargetsAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Lv2Targets {
            get => Data.GetByte(_lv2TargetsAddr);
            set => Data.SetByte(_lv2TargetsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2CostAddr), displayOrder: 10)]
        [BulkCopy]
        public int Lv2Cost {
            get => Data.GetByte(_lv2CostAddr);
            set => Data.SetByte(_lv2CostAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2DamageAddr), displayOrder: 11)]
        [BulkCopy]
        public int Lv2Damage {
            get => Data.GetByte(_lv2DamageAddr);
            set => Data.SetByte(_lv2DamageAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3DistanceAddr), displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public int Lv3Distance {
            get => Data.GetByte(_lv3DistanceAddr);
            set => Data.SetByte(_lv3DistanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3TargetsAddr), displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public int Lv3Targets {
            get => Data.GetByte(_lv3TargetsAddr);
            set => Data.SetByte(_lv3TargetsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3CostAddr), displayOrder: 14)]
        [BulkCopy]
        public int Lv3Cost {
            get => Data.GetByte(_lv3CostAddr);
            set => Data.SetByte(_lv3CostAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3DamageAddr), displayOrder: 15)]
        [BulkCopy]
        public int Lv3Damage {
            get => Data.GetByte(_lv3DamageAddr);
            set => Data.SetByte(_lv3DamageAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4DistanceAddr), displayOrder: 16, displayFormat: "X2")]
        [BulkCopy]
        public int Lv4Distance {
            get => Data.GetByte(_lv4DistanceAddr);
            set => Data.SetByte(_lv4DistanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4TargetsAddr), displayOrder: 17, displayFormat: "X2")]
        [BulkCopy]
        public int Lv4Targets {
            get => Data.GetByte(_lv4TargetsAddr);
            set => Data.SetByte(_lv4TargetsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4CostAddr), displayOrder: 18)]
        [BulkCopy]
        public int Lv4Cost {
            get => Data.GetByte(_lv4CostAddr);
            set => Data.SetByte(_lv4CostAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4DamageAddr), displayOrder: 19)]
        [BulkCopy]
        public int Lv4Damage {
            get => Data.GetByte(_lv4DamageAddr);
            set => Data.SetByte(_lv4DamageAddr, (byte) value);
        }
    }
}
