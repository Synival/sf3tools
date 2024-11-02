using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class SupportStats : IModel {
        private readonly int sLvlStat1;
        private readonly int sLvlStat2;
        private readonly int sLvlStat3;
        private readonly int sLvlStat4;
        private readonly int offset;
        private readonly int checkVersion2;

        public SupportStats(IX013_FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x04;

            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000074b5; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00007409; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000072f1; //scn3
            }
            else {
                offset = 0x000071cd; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 4);
            sLvlStat1 = start; //1 bytes
            sLvlStat2 = start + 1; //1 byte
            sLvlStat3 = start + 2; //1 byte
            sLvlStat4 = start + 3; //1 byte
            Address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int SLvlStat1 {
            get => Editor.GetByte(sLvlStat1);
            set => Editor.SetByte(sLvlStat1, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat2 {
            get => Editor.GetByte(sLvlStat2);
            set => Editor.SetByte(sLvlStat2, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat3 {
            get => Editor.GetByte(sLvlStat3);
            set => Editor.SetByte(sLvlStat3, (byte) value);
        }

        [BulkCopy]
        public int SLvlStat4 {
            get => Editor.GetByte(sLvlStat4);
            set => Editor.SetByte(sLvlStat4, (byte) value);
        }
    }
}
