using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class AI : IModel {
        private readonly int targetX;
        private readonly int targetY;

        public AI(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x04;

            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = editor.GetDouble(offset);
                offset -= sub; //second pointer
                offset = editor.GetDouble(offset);
                offset -= sub; //third pointer

                offset += 10;
                offset += 0xe9a;
                offset += 0x126;
            }
            else if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = editor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xe9a;
                    offset += 0x126;
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xe9a; //size of the enemy spawn table
                    offset += 0x126; //size of something else
                    //we're now at our offset after adding these
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = editor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;

                    offset += 0xa8a;//size of the enemy spawn table
                    offset += 0x126;
                }
                else {
                    editor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;

                    offset += 0xa8a;
                    offset += 0x126;
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = editor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                }
                else {
                    editor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                }
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer
                offset = editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = editor.GetDouble(offset);
                    offset -= sub; //third pointer

                    offset += 10;
                    offset += 0xa8a;
                    offset += 0x126;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 4);
            targetX = start; //2 bytes
            targetY = start + 2; //2 bytes
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
        public int TargetX {
            get => Editor.GetWord(targetX);
            set => Editor.SetWord(targetX, value);
        }

        [BulkCopy]
        public int TargetY {
            get => Editor.GetWord(targetY);
            set => Editor.SetWord(targetY, value);
        }
    }
}
