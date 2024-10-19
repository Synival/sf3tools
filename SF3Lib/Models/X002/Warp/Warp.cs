using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X002.Warps
{
    public class Warp
    {
        private IX002_FileEditor _fileEditor;

        private int unknown1;
        private int unknown2;
        private int type;
        private int map;

        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int checkVersion2;
        private int sub;

        private int index;
        private string name;

        /*public int NPCTableAddress1
        {
            get => FileEditor.GetDouble(npcOffset);
            set => FileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Warp(IX002_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            //only scn1 for this
            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x000053cc; //scn1
                if (checkVersion2 == 0x10) //original jp
                {
                    offset -= 0x0C;
                }
            }

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x04);
            unknown1 = start;
            unknown2 = start + 1;
            type = start + 2;
            map = start + 3;

            //unknown42 = start + 52;
            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int WarpID => index;
        public string WarpName => name;

        public int WarpUnknown1
        {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte)value);
        }

        public int WarpUnknown2
        {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte)value);
        }

        public int WarpType
        {
            get => _fileEditor.GetByte(type);
            set => _fileEditor.SetByte(type, (byte)value);
        }

        public int WarpMap
        {
            get => _fileEditor.GetByte(map);
            set => _fileEditor.SetByte(map, (byte)value);
        }

        public int WarpAddress => (address);
    }
}
