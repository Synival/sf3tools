using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Header {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int unknown1;
        private readonly int tableSize;
        private readonly int unknown2;
        private readonly int unknown3;
        private readonly int unknown4;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int unknown7;

        private readonly int unknown8;
        private readonly int unknown9;

        private readonly int offset;
        private readonly int sub;

        public Header(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //third pointer
            }
            else if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer
                offset = _fileEditor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer
                offset = _fileEditor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset -= sub; //third pointer
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            SizeID = id;
            SizeName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0A);
            unknown1 = start; //1 bytes
            tableSize = start + 1; //1 byte
            unknown2 = start + 2; //1 byte
            unknown3 = start + 3; //1 byte
            unknown4 = start + 4; //1 byte
            unknown5 = start + 5;
            unknown6 = start + 6;
            unknown7 = start + 7;

            unknown8 = start + 8;
            unknown9 = start + 9;

            SizeAddress = offset + (id * 0x0A);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SizeID { get; }

        [BulkCopyRowName]
        public string SizeName { get; }

        [BulkCopy]
        public int SizeUnknown1 {
            get => _fileEditor.GetByte(unknown1);
            set => _fileEditor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int TableSize {
            get => _fileEditor.GetByte(tableSize);
            set => _fileEditor.SetByte(tableSize, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown2 {
            get => _fileEditor.GetByte(unknown2);
            set => _fileEditor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown3 {
            get => _fileEditor.GetByte(unknown3);
            set => _fileEditor.SetByte(unknown3, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown4 {
            get => _fileEditor.GetByte(unknown4);
            set => _fileEditor.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown5 {
            get => _fileEditor.GetByte(unknown5);
            set => _fileEditor.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown6 {
            get => _fileEditor.GetByte(unknown6);
            set => _fileEditor.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown7 {
            get => _fileEditor.GetByte(unknown7);
            set => _fileEditor.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown8 {
            get => _fileEditor.GetByte(unknown8);
            set => _fileEditor.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown9 {
            get => _fileEditor.GetByte(unknown9);
            set => _fileEditor.SetByte(unknown9, (byte) value);
        }

        public int Map => _fileEditor.MapOffset;

        public int SizeAddress { get; }
    }
}
