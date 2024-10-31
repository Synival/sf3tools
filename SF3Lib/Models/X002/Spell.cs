using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Spell : IModel {
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
        private readonly int offset;
        private readonly int checkVersion2;

        public Spell(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x14;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00004328; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x0000469c; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000516c; //scn3
            }
            else {
                offset = 0x0000521c; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = name;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 20);
            targetType = start; //2 bytes
            damageType = start + 1;
            element = start + 2; //1 byte
            unknown2 = start + 3; //1 byte
            lv1Distance = start + 4; //1 byte
            lv1Targets = start + 5; //1 byte
            lv1Cost = start + 6;
            lv1Damage = start + 7;
            lv2Distance = start + 8;
            lv2Targets = start + 9;
            lv2Cost = start + 10;
            lv2Damage = start + 11;
            lv3Distance = start + 12;
            lv3Targets = start + 13;
            lv3Cost = start + 14;
            lv3Damage = start + 15;
            lv4Distance = start + 16;
            lv4Targets = start + 17;
            lv4Cost = start + 18;
            lv4Damage = start + 19;
            Address = offset + (id * 0x14);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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
