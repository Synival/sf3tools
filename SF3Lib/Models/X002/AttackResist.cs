using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class AttackResist : IModel {
        private readonly int attack;
        private readonly int resist;
        private readonly int offset;
        private readonly int checkVersion2;

        public AttackResist(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0xD3;

            checkVersion2 = Editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000cb5; //scn1
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000d15; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x40;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000dcd; //scn3
            }
            else {
                offset = 0x00000dd9; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0xd3);
            attack = start; //1 byte
            resist = start + 0xd2; //1 byte

            Address = offset + (id * 0xd3);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Attack {
            get => Editor.GetByte(attack);
            set => Editor.SetByte(attack, (byte) value);
        }

        [BulkCopy]
        public int Resist {
            get => Editor.GetByte(resist);
            set => Editor.SetByte(resist, (byte) value);
        }
    }
}
