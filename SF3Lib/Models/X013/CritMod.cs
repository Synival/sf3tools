using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class CritMod : Model {
        private readonly int advantage;
        private readonly int disadvantage;
        private readonly int offset;
        private readonly int checkVersion2;

        public CritMod(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x12) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x12;

            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00002e74; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x70;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00003050; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00002d58; //scn3
            }
            else {
                offset = 0x00002d78; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0b);
            advantage = start + 0x01; //1 bytes
            disadvantage = start + 0x11; //1 byte
            CritModAddress = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int Advantage {
            get => Editor.GetByte(advantage);
            set => Editor.SetByte(advantage, (byte) value);
        }

        [BulkCopy]
        public int Disadvantage {
            get => Editor.GetByte(disadvantage);
            set => Editor.SetByte(disadvantage, (byte) value);
        }

        public int CritModAddress { get; }
    }
}
