using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class CustomMovement : IModel {
        private readonly int unknown00;
        private readonly int xPos1;
        private readonly int zPos1;
        private readonly int xPos2;
        private readonly int zPos2;
        private readonly int xPos3;
        private readonly int zPos3;
        private readonly int xPos4;
        private readonly int zPos4;
        private readonly int ending;

        public CustomMovement(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x16;

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
                offset += 0xe9a;
                offset += 0x126;
                offset += 0x84; //size of AITargetPosition
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
                    offset += 0xe9a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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
                    offset += 0xe9a; //size of the enemy spawn table
                    offset += 0x126; //size of SpawnZones
                    offset += 0x84; //size of AITargetPosition
                    //we're now at our offset after adding these
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

                    offset += 0xa8a;//size of the enemy spawn table
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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

                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
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
                    offset += 0xa8a;
                    offset += 0x126;
                    offset += 0x84; //size of AITargetPosition
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x16);
            unknown00 = start; //2 bytes
            xPos1 = start + 2;
            zPos1 = start + 4;
            xPos2 = start + 6;
            zPos2 = start + 8;
            xPos3 = start + 10;
            zPos3 = start + 12;
            xPos4 = start + 14;
            zPos4 = start + 16;
            ending = start + 18; //4 bytes

            Address = offset + (id * 0x16);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int CustomMovementUnknown {
            get => Editor.GetWord(unknown00);
            set => Editor.SetWord(unknown00, value);
        }

        [BulkCopy]
        public int CustomMovementX1 {
            get => Editor.GetWord(xPos1);
            set => Editor.SetWord(xPos1, value);
        }

        [BulkCopy]
        public int CustomMovementZ1 {
            get => Editor.GetWord(zPos1);
            set => Editor.SetWord(zPos1, value);
        }

        [BulkCopy]
        public int CustomMovementX2 {
            get => Editor.GetWord(xPos2);
            set => Editor.SetWord(xPos2, value);
        }

        [BulkCopy]
        public int CustomMovementZ2 {
            get => Editor.GetWord(zPos2);
            set => Editor.SetWord(zPos2, value);
        }

        [BulkCopy]
        public int CustomMovementX3 {
            get => Editor.GetWord(xPos3);
            set => Editor.SetWord(xPos3, value);
        }

        [BulkCopy]
        public int CustomMovementZ3 {
            get => Editor.GetWord(zPos3);
            set => Editor.SetWord(zPos3, value);
        }

        [BulkCopy]
        public int CustomMovementX4 {
            get => Editor.GetWord(xPos4);
            set => Editor.SetWord(xPos4, value);
        }

        [BulkCopy]
        public int CustomMovementZ4 {
            get => Editor.GetWord(zPos4);
            set => Editor.SetWord(zPos4, value);
        }

        [BulkCopy]
        public int CustomMovementEnd {
            get => Editor.GetDouble(ending);
            set => Editor.SetDouble(ending, value);
        }
    }
}
