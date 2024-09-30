using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.SpecialChance
{
    public class SpecialChance
    {
        private int twoSpecials2;
        private int threeSpecials3;
        private int threeSpecials2;
        private int fourSpecials4;
        private int fourSpecials3;
        private int fourSpecials2;

        private int address;
        private int offset;

        private int index;
        private string name;

        public SpecialChance(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x000027ae; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x000029c6; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x000027a2; //scn3
            }
            else
                offset = 0x000027c2; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            if (Globals.scenario == 1)
            {
                int start = offset + (id * 0x4a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x15; //1 byte
                threeSpecials2 = start + 0x1d; //1 byte
                fourSpecials4 = start + 0x31; //1 byte
                fourSpecials3 = start + 0x3d; //1 byte
                fourSpecials2 = start + 0x49; //1 byte

                address = offset + (id * 0x4a);
            }
            else if (Globals.scenario == 2)
            {
                int start = offset + (id * 0x4a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x15; //1 byte
                threeSpecials2 = start + 0x1d; //1 byte
                fourSpecials4 = start + 0x31; //1 byte
                fourSpecials3 = start + 0x3d; //1 byte
                fourSpecials2 = start + 0x49; //1 byte

                address = offset + (id * 0x4a);
            }
            else if (Globals.scenario == 3)
            {
                int start = offset + (id * 0x3a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x0f; //1 byte
                threeSpecials2 = start + 0x19; //1 byte
                fourSpecials4 = start + 0x21; //1 byte
                fourSpecials3 = start + 0x2d; //1 byte
                fourSpecials2 = start + 0x39; //1 byte

                address = offset + (id * 0x3a);
            }
            else
            {
                int start = offset + (id * 0x3a);
                twoSpecials2 = start + 0x01; //1 bytes
                threeSpecials3 = start + 0x0f; //1 byte
                threeSpecials2 = start + 0x19; //1 byte
                fourSpecials4 = start + 0x21; //1 byte
                fourSpecials3 = start + 0x2d; //1 byte
                fourSpecials2 = start + 0x39; //1 byte

                address = offset + (id * 0x3a);
            }

            //address = 0x0354c + (id * 0x18);

        }

        public int SpecialChanceID => index;
        public string SpecialChanceName => name;

        public int TwoSpecials2
        {
            get => FileEditor.getByte(twoSpecials2);
            set => FileEditor.setByte(twoSpecials2, (byte)value);
        }
        public int ThreeSpecials3
        {
            get => FileEditor.getByte(threeSpecials3);
            set => FileEditor.setByte(threeSpecials3, (byte)value);
        }
        public int ThreeSpecials2
        {
            get => FileEditor.getByte(threeSpecials2);
            set => FileEditor.setByte(threeSpecials2, (byte)value);
        }
        public int FourSpecials4
        {
            get => FileEditor.getByte(fourSpecials4);
            set => FileEditor.setByte(fourSpecials4, (byte)value);
        }

        public int FourSpecials3
        {
            get => FileEditor.getByte(fourSpecials3);
            set => FileEditor.setByte(fourSpecials3, (byte)value);
        }

        public int FourSpecials2
        {
            get => FileEditor.getByte(fourSpecials2);
            set => FileEditor.setByte(fourSpecials2, (byte)value);
        }

        public int SpecialChanceAddress => (address);
    }
}
