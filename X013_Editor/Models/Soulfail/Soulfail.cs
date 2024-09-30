using SF3.Editor;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Soulfail
{
    public class Soulfail
    {
        private int expLost;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Soulfail(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00005e5f; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x0000650f; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00006077; //scn3
            }
            else
                offset = 0x00005f37; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 1);
            expLost = start; //1 bytes
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public int SoulfailID => index;
        public string SoulfailName => name;

        public int ExpLost
        {
            get => FileEditor.getByte(expLost);
            set => FileEditor.setByte(expLost, (byte)value);
        }

        public int SoulfailAddress => (address);
    }
}
