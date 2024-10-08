using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Types;
using System;

namespace SF3.X1_Editor.Models.Npcs
{
    public class Npc
    {
        private int spriteID;
        private int unknown1;
        private int table;
        private int xPos;
        private int zPos;
        private int direction;
        private int unknownA;
        private int unknownC;
        private int unknownE;
        private int unknown12;
        private int unknown16;

        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int sub;

        private int index;
        private string name;

        /*public int NPCTableAddress1
        {
            get => FileEditor.GetDouble(npcOffset);
            set => FileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Npc(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.GetDouble(offset);

                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }/*
            else if (Scenario == ScenarioType.BTL99)
            {
                offset = 0x00000024; //btl99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }*/

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x18);
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
            address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int NpcID => index;
        public string NpcName => name;

        public string NpcTieIn
        {
            /*get
            {
                return index + 0x3D;
            }*/

            get
            {
                if ((FileEditor.GetWord(spriteID) > 0x0f) && (FileEditor.GetWord(spriteID) != 0xffff))
                {
                    return (index + 0x3D).ToString("X");
                }
                else
                {
                    return "";
                }
            }
        }

        public int SpriteID
        {
            get => FileEditor.GetWord(spriteID);
            set => FileEditor.SetWord(spriteID, value);
        }

        public int NpcUnknown
        {
            get => FileEditor.GetWord(unknown1);
            set => FileEditor.SetWord(unknown1, value);
        }

        public int NpcTable
        {
            get => FileEditor.GetDouble(table);
            set => FileEditor.SetDouble(table, value);
        }

        public int NpcXPos
        {
            get => FileEditor.GetWord(xPos);
            set => FileEditor.SetWord(xPos, value);
        }

        public int NpcZPos
        {
            get => FileEditor.GetWord(zPos);
            set => FileEditor.SetWord(zPos, value);
        }

        public int NpcDirection
        {
            get => FileEditor.GetWord(direction);
            set => FileEditor.SetWord(direction, value);
        }

        public int NpcUnknownA
        {
            get => FileEditor.GetWord(unknownA);
            set => FileEditor.SetWord(unknownA, value);
        }

        public int NpcUnknownC
        {
            get => FileEditor.GetWord(unknownC);
            set => FileEditor.SetWord(unknownC, value);
        }

        public int NpcUnknownE
        {
            get => FileEditor.GetWord(unknownE);
            set => FileEditor.SetWord(unknownE, value);
        }

        public int NpcUnknown12
        {
            get => FileEditor.GetWord(unknown12);
            set => FileEditor.SetWord(unknown12, value);
        }

        public int NpcUnknown16
        {
            get => FileEditor.GetWord(unknown16);
            set => FileEditor.SetWord(unknown16, value);
        }

        public int NpcAddress => (address);
    }
}
