using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.FileEditors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Spell {
        private readonly IX002_FileEditor _fileEditor;

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

        public Spell(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00004328; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x0000469c; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x0000516c; //scn3
            }
            else {
                offset = 0x0000521c; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            SpellID = id;
            SpellName = text;

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
            SpellAddress = offset + (id * 0x14);
            //address = 0x0354c + (id * 0x18);
        }

        private NameAndInfo GetElementName(int value) => ValueNames.GetElementName(value);
        private NameAndInfo GetSpellTargetName(int value) => ValueNames.GetSpellTargetName(value);

        public ScenarioType Scenario => _fileEditor.Scenario;

        [BulkCopyRowName]
        public string SpellName { get; }
        public int SpellID { get; }
        public int SpellAddress { get; }

        [BulkCopy]
        [NameGetter(nameof(GetSpellTargetName))]
        public int SpellTarget {
            get => _fileEditor.GetByte(targetType);
            set => _fileEditor.SetByte(targetType, (byte) value);
        }

        [BulkCopy]
        public int SpellType {
            get => _fileEditor.GetByte(damageType);
            set => _fileEditor.SetByte(damageType, (byte) value);
        }

        [BulkCopy]
        [NameGetter(nameof(GetElementName))]
        public int Element {
            get => _fileEditor.GetByte(element);
            set => _fileEditor.SetByte(element, (byte) value);
        }

        [BulkCopy]
        public int SpellUnknown2 {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int Lv1Distance {
            get => _fileEditor.GetByte(lv1Distance);
            set => _fileEditor.SetByte(lv1Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv1Targets {
            get => _fileEditor.GetByte(lv1Targets);
            set => _fileEditor.SetByte(lv1Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv1Cost {
            get => _fileEditor.GetByte(lv1Cost);
            set => _fileEditor.SetByte(lv1Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv1Damage {
            get => _fileEditor.GetByte(lv1Damage);
            set => _fileEditor.SetByte(lv1Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv2Distance {
            get => _fileEditor.GetByte(lv2Distance);
            set => _fileEditor.SetByte(lv2Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv2Targets {
            get => _fileEditor.GetByte(lv2Targets);
            set => _fileEditor.SetByte(lv2Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv2Cost {
            get => _fileEditor.GetByte(lv2Cost);
            set => _fileEditor.SetByte(lv2Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv2Damage {
            get => _fileEditor.GetByte(lv2Damage);
            set => _fileEditor.SetByte(lv2Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv3Distance {
            get => _fileEditor.GetByte(lv3Distance);
            set => _fileEditor.SetByte(lv3Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv3Targets {
            get => _fileEditor.GetByte(lv3Targets);
            set => _fileEditor.SetByte(lv3Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv3Cost {
            get => _fileEditor.GetByte(lv3Cost);
            set => _fileEditor.SetByte(lv3Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv3Damage {
            get => _fileEditor.GetByte(lv3Damage);
            set => _fileEditor.SetByte(lv3Damage, (byte) value);
        }

        [BulkCopy]
        public int Lv4Distance {
            get => _fileEditor.GetByte(lv4Distance);
            set => _fileEditor.SetByte(lv4Distance, (byte) value);
        }

        [BulkCopy]
        public int Lv4Targets {
            get => _fileEditor.GetByte(lv4Targets);
            set => _fileEditor.SetByte(lv4Targets, (byte) value);
        }

        [BulkCopy]
        public int Lv4Cost {
            get => _fileEditor.GetByte(lv4Cost);
            set => _fileEditor.SetByte(lv4Cost, (byte) value);
        }

        [BulkCopy]
        public int Lv4Damage {
            get => _fileEditor.GetByte(lv4Damage);
            set => _fileEditor.SetByte(lv4Damage, (byte) value);
        }
    }
}
