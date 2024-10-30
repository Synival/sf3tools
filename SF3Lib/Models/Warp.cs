using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Warp {
        private readonly ISF3FileEditor _fileEditor;

        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        private readonly int offset;
        private readonly int checkVersion2;
        private readonly int sub;

        public Warp(ISF3FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x000053cc; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
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

        [BulkCopyRowName]
        public string WarpName { get; }
        public int WarpID { get; }
        public int WarpAddress { get; }

        [BulkCopy]
        public int WarpUnknown1 {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int WarpUnknown2 {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int WarpType {
            get => _fileEditor.GetByte(type);
            set => _fileEditor.SetByte(type, (byte) value);
        }

        [BulkCopy]
        public int WarpMap {
            get => _fileEditor.GetByte(map);
            set => _fileEditor.SetByte(map, (byte) value);
        }
    }
}
