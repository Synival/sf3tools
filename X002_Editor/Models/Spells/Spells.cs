using SF3.Editor;
using SF3.Types;
using static SF3.X002_Editor.Forms.frmMain;

namespace SF3.X002_Editor.Models.Spells
{
    public class Spell
    {
        private int targetType;
        private int damageType;
        private int unknown1; //actually element
        private int unknown2; //actually, iconHidden
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
        private int checkVersion2;

        private int index;
        private string name;

        public Spell(int id, string text)
        {
            checkVersion2 = FileEditor.getByte(0x0000000B);

            if (Globals.scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004328; //scn1
            }
            else if (Globals.scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000469c; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Globals.scenario == ScenarioType.Scenario3)
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

        public int SpellID => index;
        public string SpellName => name;

        public int SpellTarget
        {
            get => FileEditor.getByte(targetType);
            set => FileEditor.setByte(targetType, (byte)value);
        }
        public int SpellType
        {
            get => FileEditor.getByte(damageType);
            set => FileEditor.setByte(damageType, (byte)value);
        }
        public int SpellUnknown1
        {
            get => FileEditor.getByte(unknown1);
            set => FileEditor.setByte(unknown1, (byte)value);
        }
        public int SpellUnknown2
        {
            get => FileEditor.getByte(unknown2);
            set => FileEditor.setByte(unknown2, (byte)value);
        }
        public int Lv1Distance
        {
            get => FileEditor.getByte(lv1Distance);
            set => FileEditor.setByte(lv1Distance, (byte)value);
        }
        public int Lv1Targets
        {
            get => FileEditor.getByte(lv1Targets);
            set => FileEditor.setByte(lv1Targets, (byte)value);
        }
        public int Lv1Cost
        {
            get => FileEditor.getByte(lv1Cost);
            set => FileEditor.setByte(lv1Cost, (byte)value);
        }
        public int Lv1Damage
        {
            get => FileEditor.getByte(lv1Damage);
            set => FileEditor.setByte(lv1Damage, (byte)value);
        }
        public int Lv2Distance
        {
            get => FileEditor.getByte(lv2Distance);
            set => FileEditor.setByte(lv2Distance, (byte)value);
        }
        public int Lv2Targets
        {
            get => FileEditor.getByte(lv2Targets);
            set => FileEditor.setByte(lv2Targets, (byte)value);
        }

        public int Lv2Cost
        {
            get => FileEditor.getByte(lv2Cost);
            set => FileEditor.setByte(lv2Cost, (byte)value);
        }
        public int Lv2Damage
        {
            get => FileEditor.getByte(lv2Damage);
            set => FileEditor.setByte(lv2Damage, (byte)value);
        }
        public int Lv3Distance
        {
            get => FileEditor.getByte(lv3Distance);
            set => FileEditor.setByte(lv3Distance, (byte)value);
        }
        public int Lv3Targets
        {
            get => FileEditor.getByte(lv3Targets);
            set => FileEditor.setByte(lv3Targets, (byte)value);
        }
        public int Lv3Cost
        {
            get => FileEditor.getByte(lv3Cost);
            set => FileEditor.setByte(lv3Cost, (byte)value);
        }
        public int Lv3Damage
        {
            get => FileEditor.getByte(lv3Damage);
            set => FileEditor.setByte(lv3Damage, (byte)value);
        }
        public int Lv4Distance
        {
            get => FileEditor.getByte(lv4Distance);
            set => FileEditor.setByte(lv4Distance, (byte)value);
        }
        public int Lv4Targets
        {
            get => FileEditor.getByte(lv4Targets);
            set => FileEditor.setByte(lv4Targets, (byte)value);
        }
        public int Lv4Cost
        {
            get => FileEditor.getByte(lv4Cost);
            set => FileEditor.setByte(lv4Cost, (byte)value);
        }
        public int Lv4Damage
        {
            get => FileEditor.getByte(lv4Damage);
            set => FileEditor.setByte(lv4Damage, (byte)value);
        }
        public int SpellAddress => (address);
    }
}
