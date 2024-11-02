using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Soulmate : IModel {
        private readonly int chance;
        private readonly int offset;
        private readonly int checkVersion2;

        public Soulmate(IX013_FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x01;

            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00007530; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007484; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000736c; //scn3
            }
            else {
                offset = 0x00007248; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 1);
            chance = start; //2 bytes
            Address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Chance {
            get => Editor.GetByte(chance);
            set => Editor.SetByte(chance, (byte) value);
        }
    }
}
