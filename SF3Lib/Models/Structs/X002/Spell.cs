using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Spell : Struct {
        private readonly int _targetTypeAddr;
        private readonly int _spellEffectAddr;
        private readonly int _elementAddr;
        private readonly int _categoryAddr;
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
            _targetTypeAddr  = Address + 0;
            _spellEffectAddr = Address + 1;
            _elementAddr     = Address + 2;
            _categoryAddr    = Address + 3;
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

        [TableViewModelColumn(addressField: nameof(_targetTypeAddr), displayOrder: 0, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpellTargetType)]
        public byte TargetType {
            get => Data.GetUInt8(_targetTypeAddr);
            set => Data.SetUInt8(_targetTypeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spellEffectAddr), displayOrder: 1, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.SpellEffectType)]
        public byte EffectType {
            get => (byte) (Data.GetUInt8(_spellEffectAddr) & 0x7F);
            set => Data.SetUInt8(_spellEffectAddr, (byte) (value & 0x7F | (IsFieldSpell ? 0x80 : 0)));
        }

        [TableViewModelColumn(addressField: nameof(_spellEffectAddr), displayOrder: 1.5f)]
        [BulkCopy]
        public bool IsFieldSpell {
            get => Data.GetBit(_spellEffectAddr, 8);
            set => Data.SetBit(_spellEffectAddr, 8, value);
        }

        [TableViewModelColumn(addressField: nameof(_elementAddr), displayOrder: 2, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Element)]
        public byte Element {
            get => Data.GetUInt8(_elementAddr);
            set => Data.SetUInt8(_elementAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_categoryAddr), displayOrder: 3, minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.SpellCategory)]
        public byte Category {
            get => Data.GetUInt8(_categoryAddr);
            set => Data.SetUInt8(_categoryAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1DistanceAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv1Distance {
            get => Data.GetUInt8(_lv1DistanceAddr);
            set => Data.SetUInt8(_lv1DistanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1TargetsAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv1Targets {
            get => Data.GetUInt8(_lv1TargetsAddr);
            set => Data.SetUInt8(_lv1TargetsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1CostAddr), displayOrder: 6)]
        [BulkCopy]
        public byte Lv1Cost {
            get => Data.GetUInt8(_lv1CostAddr);
            set => Data.SetUInt8(_lv1CostAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv1DamageAddr), displayOrder: 7)]
        [BulkCopy]
        public byte Lv1Damage {
            get => Data.GetUInt8(_lv1DamageAddr);
            set => Data.SetUInt8(_lv1DamageAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2DistanceAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv2Distance {
            get => Data.GetUInt8(_lv2DistanceAddr);
            set => Data.SetUInt8(_lv2DistanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2TargetsAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv2Targets {
            get => Data.GetUInt8(_lv2TargetsAddr);
            set => Data.SetUInt8(_lv2TargetsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2CostAddr), displayOrder: 10)]
        [BulkCopy]
        public byte Lv2Cost {
            get => Data.GetUInt8(_lv2CostAddr);
            set => Data.SetUInt8(_lv2CostAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv2DamageAddr), displayOrder: 11)]
        [BulkCopy]
        public byte Lv2Damage {
            get => Data.GetUInt8(_lv2DamageAddr);
            set => Data.SetUInt8(_lv2DamageAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3DistanceAddr), displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv3Distance {
            get => Data.GetUInt8(_lv3DistanceAddr);
            set => Data.SetUInt8(_lv3DistanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3TargetsAddr), displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv3Targets {
            get => Data.GetUInt8(_lv3TargetsAddr);
            set => Data.SetUInt8(_lv3TargetsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3CostAddr), displayOrder: 14)]
        [BulkCopy]
        public byte Lv3Cost {
            get => Data.GetUInt8(_lv3CostAddr);
            set => Data.SetUInt8(_lv3CostAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv3DamageAddr), displayOrder: 15)]
        [BulkCopy]
        public byte Lv3Damage {
            get => Data.GetUInt8(_lv3DamageAddr);
            set => Data.SetUInt8(_lv3DamageAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4DistanceAddr), displayOrder: 16, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv4Distance {
            get => Data.GetUInt8(_lv4DistanceAddr);
            set => Data.SetUInt8(_lv4DistanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4TargetsAddr), displayOrder: 17, displayFormat: "X2")]
        [BulkCopy]
        public byte Lv4Targets {
            get => Data.GetUInt8(_lv4TargetsAddr);
            set => Data.SetUInt8(_lv4TargetsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4CostAddr), displayOrder: 18)]
        [BulkCopy]
        public byte Lv4Cost {
            get => Data.GetUInt8(_lv4CostAddr);
            set => Data.SetUInt8(_lv4CostAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lv4DamageAddr), displayOrder: 19)]
        [BulkCopy]
        public byte Lv4Damage {
            get => Data.GetUInt8(_lv4DamageAddr);
            set => Data.SetUInt8(_lv4DamageAddr, value);
        }
    }
}
