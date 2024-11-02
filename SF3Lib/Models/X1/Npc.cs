using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Npc : Model {
        private readonly int spriteID;
        private readonly int unknown1;
        private readonly int table;
        private readonly int xPos;
        private readonly int zPos;
        private readonly int direction;
        private readonly int unknownA;
        private readonly int unknownC;
        private readonly int unknownE;
        private readonly int unknown12;
        private readonly int unknown16;

        public Npc(IX1_FileEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x18) {
            int offset = 0;
            int sub;

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);

                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }/*
            else if (editor.Scenario == ScenarioType.BTL99)
            {
                offset = 0x00000024; //btl99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset = offset - sub;
            }*/

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x18);
            spriteID = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown1 = start + 0x02; //unknown+0x02
            table = start + 0x04;
            xPos = start + 0x08;
            unknownA = start + 0x0a;
            unknownC = start + 0x0c;
            unknownE = start + 0x0e;
            zPos = start + 0x10;
            unknown12 = start + 0x12;
            direction = start + 0x14;
            unknown16 = start + 0x16;

            //unknown42 = start + 52;
            Address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        public string NpcTieIn             /*get
            {
                return index + 0x3D;
            }*/

            => Editor.GetWord(spriteID) > 0x0f && Editor.GetWord(spriteID) != 0xffff ? (ID + 0x3D).ToString("X") : "";

        [BulkCopy]
        public int SpriteID {
            get => Editor.GetWord(spriteID);
            set => Editor.SetWord(spriteID, value);
        }

        [BulkCopy]
        public int NpcUnknown {
            get => Editor.GetWord(unknown1);
            set => Editor.SetWord(unknown1, value);
        }

        [BulkCopy]
        public int NpcTable {
            get => Editor.GetDouble(table);
            set => Editor.SetDouble(table, value);
        }

        [BulkCopy]
        public int NpcXPos {
            get => Editor.GetWord(xPos);
            set => Editor.SetWord(xPos, value);
        }

        [BulkCopy]
        public int NpcZPos {
            get => Editor.GetWord(zPos);
            set => Editor.SetWord(zPos, value);
        }

        [BulkCopy]
        public int NpcDirection {
            get => Editor.GetWord(direction);
            set => Editor.SetWord(direction, value);
        }

        [BulkCopy]
        public int NpcUnknownA {
            get => Editor.GetWord(unknownA);
            set => Editor.SetWord(unknownA, value);
        }

        [BulkCopy]
        public int NpcUnknownC {
            get => Editor.GetWord(unknownC);
            set => Editor.SetWord(unknownC, value);
        }

        [BulkCopy]
        public int NpcUnknownE {
            get => Editor.GetWord(unknownE);
            set => Editor.SetWord(unknownE, value);
        }

        [BulkCopy]
        public int NpcUnknown12 {
            get => Editor.GetWord(unknown12);
            set => Editor.SetWord(unknown12, value);
        }

        [BulkCopy]
        public int NpcUnknown16 {
            get => Editor.GetWord(unknown16);
            set => Editor.SetWord(unknown16, value);
        }
    }
}
