using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Warp {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        private readonly int offset;
        private readonly int checkVersion2;
        private readonly int sub;

        public Warp(ISF3FileEditor editor, int id, string text) {
            Editor = editor;
            Name   = text;
            ID     = id;
            Size   = 0x04;

            checkVersion2 = Editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000053cc; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000018; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000018; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000018; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x04);
            unknown1 = start;
            unknown2 = start + 1;
            type = start + 2;
            map = start + 3;

            //unknown42 = start + 52;
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
        public int WarpUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int WarpUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int WarpType {
            get => Editor.GetByte(type);
            set => Editor.SetByte(type, (byte) value);
        }

        [BulkCopy]
        public int WarpMap {
            get => Editor.GetByte(map);
            set => Editor.SetByte(map, (byte) value);
        }
    }
}
