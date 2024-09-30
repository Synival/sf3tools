using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Spells
{
    public class Spell
    {
        private int supportA;
        private int supportB;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Spell(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00007484; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00007390; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00007278; //scn3
            }
            else
                offset = 0x00007154; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x02);
            supportA = start; //1 byte
            supportB = start + 1;

            address = offset + (id * 0x02);
            //address = 0x0354c + (id * 0x18);

        }

        public int SpellID => index;
        public string SpellName => name;

        public int SupportA
        {
            get
            {
                return FileEditor.getByte(supportA);
            }
            set
            {
                FileEditor.setByte(supportA, (byte)value);
            }
        }
        public int SupportB
        {
            get
            {
                return FileEditor.getByte(supportB);
            }
            set
            {
                FileEditor.setByte(supportB, (byte)value);
            }
        }

        public int SpellAddress => (address);
    }
}
