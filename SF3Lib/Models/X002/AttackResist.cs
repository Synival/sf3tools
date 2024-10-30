using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class AttackResist {
        private readonly IX002_FileEditor _fileEditor;

        private readonly int attack;
        private readonly int resist;
        private readonly int offset;
        private readonly int checkVersion2;

        public AttackResist(IX002_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00000cb5; //scn1
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000d15; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x40;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000dcd; //scn3
            }
            else {
                offset = 0x00000dd9; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            AttackResistID = id;
            AttackResistName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0xd3);
            attack = start; //1 byte
            resist = start + 0xd2; //1 byte

            AttackResistAddress = offset + (id * 0xd3);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;

        [BulkCopyRowName]
        public string AttackResistName { get; }
        public int AttackResistID { get; }
        public int AttackResistAddress { get; }

        [BulkCopy]
        public int Attack {
            get => _fileEditor.GetByte(attack);
            set => _fileEditor.SetByte(attack, (byte) value);
        }

        [BulkCopy]
        public int Resist {
            get => _fileEditor.GetByte(resist);
            set => _fileEditor.SetByte(resist, (byte) value);
        }
    }
}
