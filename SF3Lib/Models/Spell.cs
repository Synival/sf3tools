using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Spell : Model {
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

        public Spell(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x14) {
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

        [BulkCopy]
        [NameGetter(NamedValueType.SpellTarget)]
        public int SpellTarget {
            get => Editor.GetByte(targetType);
            set => Editor.SetByte(targetType, (byte) value);
        }

        [BulkCopy]
        public int SpellType {
            get => Editor.GetByte(damageType);
            set => Editor.SetByte(damageType, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Element)]
        public int Element {
            get => Editor.GetByte(element);
            set => Editor.SetByte(element, (byte) value);
        }

        [BulkCopy]
        public int SpellUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int Lv1Distance {
            get => Editor.GetByte(lv1Distance);
            set => Editor.SetByte(lv1Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv1Targets {
            get => Editor.GetByte(lv1Targets);
            set => Editor.SetByte(lv1Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv1Cost {
            get => Editor.GetByte(lv1Cost);
            set => Editor.SetByte(lv1Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv1Damage {
            get => Editor.GetByte(lv1Damage);
            set => Editor.SetByte(lv1Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv2Distance {
            get => Editor.GetByte(lv2Distance);
            set => Editor.SetByte(lv2Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv2Targets {
            get => Editor.GetByte(lv2Targets);
            set => Editor.SetByte(lv2Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv2Cost {
            get => Editor.GetByte(lv2Cost);
            set => Editor.SetByte(lv2Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv2Damage {
            get => Editor.GetByte(lv2Damage);
            set => Editor.SetByte(lv2Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv3Distance {
            get => Editor.GetByte(lv3Distance);
            set => Editor.SetByte(lv3Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv3Targets {
            get => Editor.GetByte(lv3Targets);
            set => Editor.SetByte(lv3Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv3Cost {
            get => Editor.GetByte(lv3Cost);
            set => Editor.SetByte(lv3Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv3Damage {
            get => Editor.GetByte(lv3Damage);
            set => Editor.SetByte(lv3Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv4Distance {
            get => Editor.GetByte(lv4Distance);
            set => Editor.SetByte(lv4Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv4Targets {
            get => Editor.GetByte(lv4Targets);
            set => Editor.SetByte(lv4Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv4Cost {
            get => Editor.GetByte(lv4Cost);
            set => Editor.SetByte(lv4Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv4Damage {
            get => Editor.GetByte(lv4Damage);
            set => Editor.SetByte(lv4Damage, (byte) value);
        }
    }
}
