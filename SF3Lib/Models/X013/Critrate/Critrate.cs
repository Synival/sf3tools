using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X013.Critrate
{
    public class Critrate
    {
        private IX013_FileEditor _fileEditor;

        private int noSpecial;
        private int oneSpecial;
        private int twoSpecial;
        private int threeSpecial;
        private int fourSpecial;
        private int fiveSpecial;
        private int address;
        private int offset;

        private int index;
        private string name;
        private int checkVersion2;

        public Critrate(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000073f8; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00007304; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x000071dc; //scn3
            }
            else
                offset = 0x000070b8; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 8);
            noSpecial = start; //1 bytes
            oneSpecial = start + 1; //1 byte
            twoSpecial = start + 2; //1 byte
            threeSpecial = start + 3; //1 byte
            fourSpecial = start + 4;
            fiveSpecial = start + 5;
            address = offset + (id * 0x8);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int CritrateID => index;
        public string CritrateName => name;

        public int NoSpecial
        {
            get => _fileEditor.GetByte(noSpecial);
            set => _fileEditor.SetByte(noSpecial, (byte)value);
        }
        public int OneSpecial
        {
            get => _fileEditor.GetByte(oneSpecial);
            set => _fileEditor.SetByte(oneSpecial, (byte)value);
        }
        public int TwoSpecial
        {
            get => _fileEditor.GetByte(twoSpecial);
            set => _fileEditor.SetByte(twoSpecial, (byte)value);
        }
        public int ThreeSpecial
        {
            get => _fileEditor.GetByte(threeSpecial);
            set => _fileEditor.SetByte(threeSpecial, (byte)value);
        }
        public int FourSpecial
        {
            get => _fileEditor.GetByte(fourSpecial);
            set => _fileEditor.SetByte(fourSpecial, (byte)value);
        }

        public int FiveSpecial
        {
            get => _fileEditor.GetByte(fiveSpecial);
            set => _fileEditor.SetByte(fiveSpecial, (byte)value);
        }

        public int CritrateAddress => (address);
    }
}
