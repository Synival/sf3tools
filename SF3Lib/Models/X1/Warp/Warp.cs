using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1.Warp {
    public class Warp {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        //private int npcOffset;
        private readonly int offset;
        private readonly int sub;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Warp(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            //no scn1 for this

            if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000018; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000018; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000018; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            WarpID = id;
            WarpName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x04);
            unknown1 = start;
            unknown2 = start + 1;
            type = start + 2;
            map = start + 3;

            //unknown42 = start + 52;
            WarpAddress = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int WarpID { get; }
        public string WarpName { get; }

        public int WarpUnknown1 {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte) value);
        }

        public int WarpUnknown2 {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte) value);
        }

        public int WarpType {
            get => _fileEditor.GetByte(type);
            set => _fileEditor.SetByte(type, (byte) value);
        }

        public int WarpMap {
            get => _fileEditor.GetByte(map);
            set => _fileEditor.SetByte(map, (byte) value);
        }

        public int WarpAddress { get; }
    }
}
