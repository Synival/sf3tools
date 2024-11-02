using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class SpawnZone : Model {
        private readonly int unknown00;
        private readonly int unknown02;
        private readonly int unknown04;
        private readonly int unknown06;
        private readonly int unknown08;
        private readonly int unknown0A;
        private readonly int unknown0C;
        private readonly int unknown0E;
        private readonly int unknown10;

        public SpawnZone(IX1_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x12) {
            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //third pointer

                offset += 10;
                offset += 0xea0;
            }
            else if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = Editor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xEa0;
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xEa0;
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = Editor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = Editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = Editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }
                else {
                    editor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = Editor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = Editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = Editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }
                else {
                    editor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer
                offset = Editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa90;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd
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
            Address = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);
        }

        [BulkCopy]
        public int UnknownAI00 {
            get => Editor.GetWord(unknown00);
            set => Editor.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int UnknownAI02 {
            get => Editor.GetWord(unknown02);
            set => Editor.SetWord(unknown02, value);
        }

        [BulkCopy]
        public int UnknownAI04 {
            get => Editor.GetWord(unknown04);
            set => Editor.SetWord(unknown04, value);
        }

        [BulkCopy]
        public int UnknownAI06 {
            get => Editor.GetWord(unknown06);
            set => Editor.SetWord(unknown06, value);
        }

        [BulkCopy]
        public int UnknownAI08 {
            get => Editor.GetWord(unknown08);
            set => Editor.SetWord(unknown08, value);
        }

        [BulkCopy]
        public int UnknownAI0A {
            get => Editor.GetWord(unknown0A);
            set => Editor.SetWord(unknown0A, value);
        }

        [BulkCopy]
        public int UnknownAI0C {
            get => Editor.GetWord(unknown0C);
            set => Editor.SetWord(unknown0C, value);
        }

        [BulkCopy]
        public int UnknownAI0E {
            get => Editor.GetWord(unknown0E);
            set => Editor.SetWord(unknown0E, value);
        }

        [BulkCopy]
        public int UnknownAI10 {
            get => Editor.GetWord(unknown10);
            set => Editor.SetWord(unknown10, value);
        }
    }
}
