using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class BattlePointers : IModel {
        private readonly int battlePointer;

        public BattlePointers(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x04;

            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer
                               //offset = Editor.GetDouble(offset);
                /*
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xa90;
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer

                //offset = Editor.GetDouble(offset);
                //offset = offset - sub;

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
                offset -= sub; //second pointer

                //offset = Editor.GetDouble(offset);

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
                offset -= sub; //second pointer

                //offset = Editor.GetDouble(offset);
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer
                               //offset = Editor.GetDouble(offset);
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x4);
            battlePointer = start; //2 bytes 
            //battlePointer2 = start +2; //2 bytes
            //unknown42 = start + 52;
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
        public int BattlePointer {
            get => Editor.GetDouble(battlePointer);
            set => Editor.SetDouble(battlePointer, value);
        }
    }
}
