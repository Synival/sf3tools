using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class StatBoost : IModel {
        private readonly int stat;
        private readonly int offset;
        private readonly int checkVersion2;

        public StatBoost(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x01;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00004537; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x000048ab; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000537b; //scn3
            }
            else {
                offset = 0x0000542b; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = name;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x01);
            stat = start; //1 bytes
            Address = offset + (id * 0x01);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Stat {
            get => Editor.GetByte(stat);
            set => Editor.SetByte(stat, (byte) value);
        }
    }
}
