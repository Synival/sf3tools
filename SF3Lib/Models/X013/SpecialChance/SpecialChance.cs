using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X013.SpecialChance
{
    public class SpecialChance
    {
        private IX013_FileEditor _fileEditor;

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
        private int checkVersion2;

        public SpecialChance(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000027ae; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x70;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x000029c6; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
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

            if (Scenario == ScenarioType.Scenario1)
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
            else if (Scenario == ScenarioType.Scenario2)
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
            else if (Scenario == ScenarioType.Scenario3)
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

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SpecialChanceID => index;
        public string SpecialChanceName => name;

        public int TwoSpecials2
        {
            get => _fileEditor.GetByte(twoSpecials2);
            set => _fileEditor.SetByte(twoSpecials2, (byte)value);
        }
        public int ThreeSpecials3
        {
            get => _fileEditor.GetByte(threeSpecials3);
            set => _fileEditor.SetByte(threeSpecials3, (byte)value);
        }
        public int ThreeSpecials2
        {
            get => _fileEditor.GetByte(threeSpecials2);
            set => _fileEditor.SetByte(threeSpecials2, (byte)value);
        }
        public int FourSpecials4
        {
            get => _fileEditor.GetByte(fourSpecials4);
            set => _fileEditor.SetByte(fourSpecials4, (byte)value);
        }

        public int FourSpecials3
        {
            get => _fileEditor.GetByte(fourSpecials3);
            set => _fileEditor.SetByte(fourSpecials3, (byte)value);
        }

        public int FourSpecials2
        {
            get => _fileEditor.GetByte(fourSpecials2);
            set => _fileEditor.SetByte(fourSpecials2, (byte)value);
        }

        public int SpecialChanceAddress => (address);
    }
}
