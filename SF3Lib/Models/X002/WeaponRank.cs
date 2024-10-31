using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class WeaponRank : IModel {
        private readonly int skill0;
        private readonly int skill1;
        private readonly int skill2;
        private readonly int skill3;
        private readonly int offset;
        private readonly int checkVersion2;

        public WeaponRank(ISF3FileEditor editor, int id, string text) {
            Editor = editor;
            Name   = text;
            ID     = id;
            Size   = 0x04;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000029f8; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00002d00; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x0000339c; //scn3
            }
            else {
                offset = 0x0000344c; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x04);
            skill0 = start; //1 byte
            skill1 = start + 1; //1 byte
            skill2 = start + 2; //1 byte
            skill3 = start + 3; //1 byte

            Address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Skill0 {
            get => Editor.GetByte(skill0);
            set => Editor.SetByte(skill0, (byte) value);
        }

        [BulkCopy]
        public int Skill1 {
            get => Editor.GetByte(skill1);
            set => Editor.SetByte(skill1, (byte) value);
        }

        [BulkCopy]
        public int Skill2 {
            get => Editor.GetByte(skill2);
            set => Editor.SetByte(skill2, (byte) value);
        }

        [BulkCopy]
        public int Skill3 {
            get => Editor.GetByte(skill3);
            set => Editor.SetByte(skill3, (byte) value);
        }
    }
}
