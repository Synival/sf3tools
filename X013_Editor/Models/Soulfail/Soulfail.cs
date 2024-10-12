using SF3.Types;

namespace SF3.X013_Editor.Models.Soulfail
{
    public class Soulfail
    {
        private IX013_FileEditor _fileEditor;

        private int expLost;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Soulfail(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00005e5f; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x0000650f; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
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

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SoulfailID => index;
        public string SoulfailName => name;

        public int ExpLost
        {
            get => _fileEditor.GetByte(expLost);
            set => _fileEditor.SetByte(expLost, (byte)value);
        }

        public int SoulfailAddress => (address);
    }
}
