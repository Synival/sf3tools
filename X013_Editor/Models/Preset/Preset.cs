using SF3.Editor;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Presets
{
    public class Preset
    {
        private int sLvl0;
        private int sLvl1;
        private int sLvl2;
        private int sLvl3;
        private int sLvl4;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Preset(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x0000747c; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00007388; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00007270; //scn3
            }
            else
                offset = 0x0000714c; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            sLvl0 = start; //1 byte
            sLvl1 = start + 1; //1 byte
            sLvl2 = start + 2; //1 byte
            sLvl3 = start + 3; //1 byte
            sLvl4 = start + 4; //1 byte
            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);

        }

        public int PresetID => index;
        public string PresetName => name;

        public int SLvl0
        {
            get => FileEditor.getByte(sLvl0);
            set => FileEditor.setByte(sLvl0, (byte)value);
        }
        public int SLvl1
        {
            get => FileEditor.getByte(sLvl1);
            set => FileEditor.setByte(sLvl1, (byte)value);
        }
        public int SLvl2
        {
            get => FileEditor.getByte(sLvl2);
            set => FileEditor.setByte(sLvl2, (byte)value);
        }
        public int SLvl3
        {
            get => FileEditor.getByte(sLvl3);
            set => FileEditor.setByte(sLvl3, (byte)value);
        }
        public int SLvl4
        {
            get => FileEditor.getByte(sLvl4);
            set => FileEditor.setByte(sLvl4, (byte)value);
        }

        public int PresetAddress => (address);
    }
}
