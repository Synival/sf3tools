using SF3.Editor;
using SF3.Types;

namespace SF3.IconPointerEditor.Models.Spells
{
    public class Spell
    {
        private ISF3FileEditor _fileEditor;

        private int targetType;
        private int damageType;
        private int unknown1;
        private int unknown2;
        private int lv1Distance;
        private int lv1Targets;
        private int lv1Cost;
        private int lv1Damage;
        private int lv2Distance;
        private int lv2Targets;
        private int lv2Cost;
        private int lv2Damage;
        private int lv3Distance;
        private int lv3Targets;
        private int lv3Cost;
        private int lv3Damage;
        private int lv4Distance;
        private int lv4Targets;
        private int lv4Cost;
        private int lv4Damage;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Spell(ISF3FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004328; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000469c; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000516c; //scn3
            }
            else
                offset = 0x0000521c; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 20);
            targetType = start; //2 bytes
            damageType = start + 1;
            unknown1 = start + 2; //1 byte
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
            address = offset + (id * 0x14);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SpellID => index;
        public string SpellName => name;

        public int SpellTarget
        {
            get => _fileEditor.GetByte(targetType);
            set => _fileEditor.SetByte(targetType, (byte)value);
        }
        public int SpellType
        {
            get => _fileEditor.GetByte(damageType);
            set => _fileEditor.SetByte(damageType, (byte)value);
        }
        public int SpellUnknown1
        {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte)value);
        }
        public int SpellUnknown2
        {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte)value);
        }
        public int Lv1Distance
        {
            get => _fileEditor.GetByte(lv1Distance);
            set => _fileEditor.SetByte(lv1Distance, (byte)value);
        }
        public int Lv1Targets
        {
            get => _fileEditor.GetByte(lv1Targets);
            set => _fileEditor.SetByte(lv1Targets, (byte)value);
        }
        public int Lv1Cost
        {
            get => _fileEditor.GetByte(lv1Cost);
            set => _fileEditor.SetByte(lv1Cost, (byte)value);
        }
        public int Lv1Damage
        {
            get => _fileEditor.GetByte(lv1Damage);
            set => _fileEditor.SetByte(lv1Damage, (byte)value);
        }
        public int Lv2Distance
        {
            get => _fileEditor.GetByte(lv2Distance);
            set => _fileEditor.SetByte(lv2Distance, (byte)value);
        }
        public int Lv2Targets
        {
            get => _fileEditor.GetByte(lv2Targets);
            set => _fileEditor.SetByte(lv2Targets, (byte)value);
        }

        public int Lv2Cost
        {
            get => _fileEditor.GetByte(lv2Cost);
            set => _fileEditor.SetByte(lv2Cost, (byte)value);
        }
        public int Lv2Damage
        {
            get => _fileEditor.GetByte(lv2Damage);
            set => _fileEditor.SetByte(lv2Damage, (byte)value);
        }
        public int Lv3Distance
        {
            get => _fileEditor.GetByte(lv3Distance);
            set => _fileEditor.SetByte(lv3Distance, (byte)value);
        }
        public int Lv3Targets
        {
            get => _fileEditor.GetByte(lv3Targets);
            set => _fileEditor.SetByte(lv3Targets, (byte)value);
        }
        public int Lv3Cost
        {
            get => _fileEditor.GetByte(lv3Cost);
            set => _fileEditor.SetByte(lv3Cost, (byte)value);
        }
        public int Lv3Damage
        {
            get => _fileEditor.GetByte(lv3Damage);
            set => _fileEditor.SetByte(lv3Damage, (byte)value);
        }
        public int Lv4Distance
        {
            get => _fileEditor.GetByte(lv4Distance);
            set => _fileEditor.SetByte(lv4Distance, (byte)value);
        }
        public int Lv4Targets
        {
            get => _fileEditor.GetByte(lv4Targets);
            set => _fileEditor.SetByte(lv4Targets, (byte)value);
        }
        public int Lv4Cost
        {
            get => _fileEditor.GetByte(lv4Cost);
            set => _fileEditor.SetByte(lv4Cost, (byte)value);
        }
        public int Lv4Damage
        {
            get => _fileEditor.GetByte(lv4Damage);
            set => _fileEditor.SetByte(lv4Damage, (byte)value);
        }
        public int SpellAddress => (address);
    }
}
