using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Critrate : Model {
        private readonly int noSpecial;
        private readonly int oneSpecial;
        private readonly int twoSpecial;
        private readonly int threeSpecial;
        private readonly int fourSpecial;
        private readonly int fiveSpecial;
        private readonly int offset;
        private readonly int checkVersion2;

        public Critrate(IX013_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x08) {
            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000073f8; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007304; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000071dc; //scn3
            }
            else {
                offset = 0x000070b8; //pd
            }

            var start = offset + (id * 8);
            noSpecial = start; //1 bytes
            oneSpecial = start + 1; //1 byte
            twoSpecial = start + 2; //1 byte
            threeSpecial = start + 3; //1 byte
            fourSpecial = start + 4;
            fiveSpecial = start + 5;
            CritrateAddress = offset + (id * 0x8);
        }

        [BulkCopy]
        public int NoSpecial {
            get => Editor.GetByte(noSpecial);
            set => Editor.SetByte(noSpecial, (byte) value);
        }

        [BulkCopy]
        public int OneSpecial {
            get => Editor.GetByte(oneSpecial);
            set => Editor.SetByte(oneSpecial, (byte) value);
        }

        [BulkCopy]
        public int TwoSpecial {
            get => Editor.GetByte(twoSpecial);
            set => Editor.SetByte(twoSpecial, (byte) value);
        }

        [BulkCopy]
        public int ThreeSpecial {
            get => Editor.GetByte(threeSpecial);
            set => Editor.SetByte(threeSpecial, (byte) value);
        }

        [BulkCopy]
        public int FourSpecial {
            get => Editor.GetByte(fourSpecial);
            set => Editor.SetByte(fourSpecial, (byte) value);
        }

        [BulkCopy]
        public int FiveSpecial {
            get => Editor.GetByte(fiveSpecial);
            set => Editor.SetByte(fiveSpecial, (byte) value);
        }

        public int CritrateAddress { get; }
    }
}
