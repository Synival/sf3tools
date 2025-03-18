using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Spell : Struct {
        private readonly int targetType;
        private readonly int damageType;
        private readonly int element;
        private readonly int unknown2; //actually, iconHidden
        private readonly int lv1Distance;
        private readonly int lv1Targets;
        private readonly int lv1Cost;
        private readonly int lv1Damage;
        private readonly int lv2Distance;
        private readonly int lv2Targets;
        private readonly int lv2Cost;
        private readonly int lv2Damage;
        private readonly int lv3Distance;
        private readonly int lv3Targets;
        private readonly int lv3Cost;
        private readonly int lv3Damage;
        private readonly int lv4Distance;
        private readonly int lv4Targets;
        private readonly int lv4Cost;
        private readonly int lv4Damage;

        public Spell(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x14) {
            targetType  = Address;
            damageType  = Address + 1;
            element     = Address + 2;
            unknown2    = Address + 3;
            lv1Distance = Address + 4;
            lv1Targets  = Address + 5;
            lv1Cost     = Address + 6;
            lv1Damage   = Address + 7;
            lv2Distance = Address + 8;
            lv2Targets  = Address + 9;
            lv2Cost     = Address + 10;
            lv2Damage   = Address + 11;
            lv3Distance = Address + 12;
            lv3Targets  = Address + 13;
            lv3Cost     = Address + 14;
            lv3Damage   = Address + 15;
            lv4Distance = Address + 16;
            lv4Targets  = Address + 17;
            lv4Cost     = Address + 18;
            lv4Damage   = Address + 19;
        }

        [TableViewModelColumn(displayOrder: 0, minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.SpellTarget)]
        public int SpellTarget {
            get => Data.GetByte(targetType);
            set => Data.SetByte(targetType, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int SpellType {
            get => Data.GetByte(damageType);
            set => Data.SetByte(damageType, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.Element)]
        public int Element {
            get => Data.GetByte(element);
            set => Data.SetByte(element, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "IconHidden")]
        [BulkCopy]
        public int SpellUnknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int Lv1Distance {
            get => Data.GetByte(lv1Distance);
            set => Data.SetByte(lv1Distance, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int Lv1Targets {
            get => Data.GetByte(lv1Targets);
            set => Data.SetByte(lv1Targets, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 6)]
        [BulkCopy]
        public int Lv1Cost {
            get => Data.GetByte(lv1Cost);
            set => Data.SetByte(lv1Cost, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 7)]
        [BulkCopy]
        public int Lv1Damage {
            get => Data.GetByte(lv1Damage);
            set => Data.SetByte(lv1Damage, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int Lv2Distance {
            get => Data.GetByte(lv2Distance);
            set => Data.SetByte(lv2Distance, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Lv2Targets {
            get => Data.GetByte(lv2Targets);
            set => Data.SetByte(lv2Targets, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 10)]
        [BulkCopy]
        public int Lv2Cost {
            get => Data.GetByte(lv2Cost);
            set => Data.SetByte(lv2Cost, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 11)]
        [BulkCopy]
        public int Lv2Damage {
            get => Data.GetByte(lv2Damage);
            set => Data.SetByte(lv2Damage, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public int Lv3Distance {
            get => Data.GetByte(lv3Distance);
            set => Data.SetByte(lv3Distance, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public int Lv3Targets {
            get => Data.GetByte(lv3Targets);
            set => Data.SetByte(lv3Targets, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 14)]
        [BulkCopy]
        public int Lv3Cost {
            get => Data.GetByte(lv3Cost);
            set => Data.SetByte(lv3Cost, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 15)]
        [BulkCopy]
        public int Lv3Damage {
            get => Data.GetByte(lv3Damage);
            set => Data.SetByte(lv3Damage, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 16, displayFormat: "X2")]
        [BulkCopy]
        public int Lv4Distance {
            get => Data.GetByte(lv4Distance);
            set => Data.SetByte(lv4Distance, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 17, displayFormat: "X2")]
        [BulkCopy]
        public int Lv4Targets {
            get => Data.GetByte(lv4Targets);
            set => Data.SetByte(lv4Targets, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 18)]
        [BulkCopy]
        public int Lv4Cost {
            get => Data.GetByte(lv4Cost);
            set => Data.SetByte(lv4Cost, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 19)]
        [BulkCopy]
        public int Lv4Damage {
            get => Data.GetByte(lv4Damage);
            set => Data.SetByte(lv4Damage, (byte) value);
        }
    }
}
