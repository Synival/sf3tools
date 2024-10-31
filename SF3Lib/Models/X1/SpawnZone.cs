using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class SpawnZone {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int unknown00;
        private readonly int unknown02;
        private readonly int unknown04;
        private readonly int unknown06;
        private readonly int unknown08;
        private readonly int unknown0A;
        private readonly int unknown0C;
        private readonly int unknown0E;
        private readonly int unknown10;

        //private int npcOffset;
        private readonly int offset;
        private readonly int sub;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public SpawnZone(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //third pointer

                offset += 10;
                offset += 0xea0;
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

                    offset += 10;
                    offset += 0xEa0;
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

                    offset += 10;
                    offset += 0xEa0;
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

                    offset += 10;
                    offset += 0xa90;
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

                    offset += 10;
                    offset += 0xa90;
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

                    offset += 10;
                    offset += 0xa90;
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

                    offset += 10;
                    offset += 0xa90;
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

                    offset += 10;
                    offset += 0xa90;
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

                    offset += 10;
                    offset += 0xa90;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            UnknownAIID = id;
            UnknownAIName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x12);
            unknown00 = start; //2 bytes  
            unknown02 = start + 2; //2 byte
            unknown04 = start + 4; //2 byte
            unknown06 = start + 6; //2 byte
            unknown08 = start + 8;
            unknown0A = start + 0x0a;
            unknown0C = start + 0x0c; //2 byte
            unknown0E = start + 0x0e;
            unknown10 = start + 0x10;
            //unknown42 = start + 52;
            UnknownAIAddress = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int UnknownAIID { get; }

        [BulkCopyRowName]
        public string UnknownAIName { get; }

        [BulkCopy]
        public int UnknownAI00 {
            get => _fileEditor.GetWord(unknown00);
            set => _fileEditor.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int UnknownAI02 {
            get => _fileEditor.GetWord(unknown02);
            set => _fileEditor.SetWord(unknown02, value);
        }

        [BulkCopy]
        public int UnknownAI04 {
            get => _fileEditor.GetWord(unknown04);
            set => _fileEditor.SetWord(unknown04, value);
        }

        [BulkCopy]
        public int UnknownAI06 {
            get => _fileEditor.GetWord(unknown06);
            set => _fileEditor.SetWord(unknown06, value);
        }

        [BulkCopy]
        public int UnknownAI08 {
            get => _fileEditor.GetWord(unknown08);
            set => _fileEditor.SetWord(unknown08, value);
        }

        [BulkCopy]
        public int UnknownAI0A {
            get => _fileEditor.GetWord(unknown0A);
            set => _fileEditor.SetWord(unknown0A, value);
        }

        [BulkCopy]
        public int UnknownAI0C {
            get => _fileEditor.GetWord(unknown0C);
            set => _fileEditor.SetWord(unknown0C, value);
        }

        [BulkCopy]
        public int UnknownAI0E {
            get => _fileEditor.GetWord(unknown0E);
            set => _fileEditor.SetWord(unknown0E, value);
        }

        [BulkCopy]
        public int UnknownAI10 {
            get => _fileEditor.GetWord(unknown10);
            set => _fileEditor.SetWord(unknown10, value);
        }

        public int UnknownAIAddress { get; }
    }
}
