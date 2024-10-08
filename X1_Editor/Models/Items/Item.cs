using SF3.Editor;
using SF3.Types;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Items
{
    public class Item
    {
        private int unknown1;
        private int unknown2;
        private int enemyID;
        private int x;
        private int y;
        private int itemOverride;
        private int unknown3;
        private int unknown4;
        private int joinID;
        private int unknown5;
        private int unknown6;
        private int unknown7;
        private int unknown8;
        private int controlType;
        private int unknown9;
        private int unknown10;
        private int unknown11;
        private int unknown12;
        private int unknown13;
        private int unknown14;
        private int unknown15;
        private int unknown16;
        private int unknown17;
        private int unknown18;
        private int unknown19;
        private int unknown20;
        private int unknown21;
        private int unknown22;
        private int unknown23;
        private int unknown24;
        private int unknown25;
        private int unknown26;
        private int unknown27;
        private int unknown28;
        private int unknown29;
        private int unknown30;
        private int unknown31;
        private int unknown32;
        private int unknown33;
        private int unknown34;
        private int unknown35;
        private int unknown36;
        private int unknown37;
        private int unknown38;
        private int unknown39;
        private int unknown40;
        //private int unknown42;

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

        public Item(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.GetDouble(offset);

                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = FileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
                else
                {
                    Globals.map = 4;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = FileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
                else
                {
                    Globals.map = 8;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub + Globals.map; //second pointer
                offset = FileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                }
            }
            else if (Scenario == ScenarioType.Other)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //third pointer

                offset = offset + 10;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x34);
            enemyID = start; //2 bytes  
            x = start + 2; //2 byte
            y = start + 4; //2 byte
            itemOverride = start + 6; //2 byte
            unknown1 = start + 8; //drop disabled
            unknown2 = start + 9; //probably droprate override
            joinID = start + 10; //2 byte
            unknown3 = start + 12; //character that shows up when enemy id is 5b
            unknown4 = start + 13;
            unknown5 = start + 14;
            unknown6 = start + 15;
            unknown7 = start + 16;
            controlType = start + 17;
            unknown8 = start + 18;
            unknown9 = start + 19;
            unknown10 = start + 20;
            unknown11 = start + 21;
            unknown12 = start + 22;
            unknown13 = start + 23;
            unknown14 = start + 24;
            unknown15 = start + 25;
            unknown16 = start + 26;
            unknown17 = start + 27;
            unknown18 = start + 28;
            unknown19 = start + 29;
            unknown20 = start + 30;
            unknown21 = start + 31; //turn not skipped?
            unknown22 = start + 32;
            unknown23 = start + 33;
            unknown24 = start + 34;
            unknown25 = start + 35;//aitag1?
            unknown26 = start + 36; //aitype1?
            unknown27 = start + 37;//aiaggression 1?
            unknown28 = start + 38;//aitag2?
            unknown29 = start + 39;//aitype4?
            unknown30 = start + 40;//aiaggression 2?
            unknown31 = start + 41; //aitag3?
            unknown32 = start + 42;//aitype4?
            unknown33 = start + 43;//aiaggression 3?
            unknown34 = start + 44; //aitag4?
            unknown35 = start + 45; //aitype4?
            unknown36 = start + 46; //aiaggression 4?
            unknown37 = start + 47;
            unknown38 = start + 48;
            unknown39 = start + 49;
            unknown40 = start + 50; //2 bytes
            //unknown42 = start + 52;
            address = offset + (id * 0x34);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int ID => index;
        public string Name => name;

        public int Unknown1
        {
            get => FileEditor.GetByte(unknown1);
            set => FileEditor.SetByte(unknown1, (byte)value);
        }

        public int Unknown2
        {
            get => FileEditor.GetByte(unknown2);
            set => FileEditor.SetByte(unknown2, (byte)value);
        }

        public int EnemyID
        {
            get => FileEditor.GetWord(enemyID);
            set => FileEditor.SetWord(enemyID, value);
        }

        public int EnemyX
        {
            get => FileEditor.GetWord(x);
            set => FileEditor.SetWord(x, value);
        }

        public int EnemyY
        {
            get => FileEditor.GetWord(y);
            set => FileEditor.SetWord(y, value);
        }

        public int ItemOverride
        {
            get => FileEditor.GetWord(itemOverride);
            set => FileEditor.SetWord(itemOverride, value);
        }

        public int Unknown3
        {
            get => FileEditor.GetByte(unknown3);
            set => FileEditor.SetByte(unknown3, (byte)value);
        }
        public int Unknown4
        {
            get => FileEditor.GetByte(unknown4);
            set => FileEditor.SetByte(unknown4, (byte)value);
        }

        public int Unknown5
        {
            get => FileEditor.GetByte(unknown5);
            set => FileEditor.SetByte(unknown5, (byte)value);
        }

        public int JoinID
        {
            get => FileEditor.GetWord(joinID);
            set => FileEditor.SetWord(joinID, value);
        }

        public int Unknown6
        {
            get => FileEditor.GetByte(unknown6);
            set => FileEditor.SetByte(unknown6, (byte)value);
        }

        public int Unknown7
        {
            get => FileEditor.GetByte(unknown7);
            set => FileEditor.SetByte(unknown7, (byte)value);
        }
        public int Unknown8
        {
            get => FileEditor.GetByte(unknown8);
            set => FileEditor.SetByte(unknown8, (byte)value);
        }
        public int Unknown9
        {
            get => FileEditor.GetByte(unknown9);
            set => FileEditor.SetByte(unknown9, (byte)value);
        }

        public int ControlType
        {
            get => FileEditor.GetByte(controlType);
            set => FileEditor.SetByte(controlType, (byte)value);
        }

        public int Unknown10
        {
            get => FileEditor.GetByte(unknown10);
            set => FileEditor.SetByte(unknown10, (byte)value);
        }

        public int Unknown11
        {
            get => FileEditor.GetByte(unknown11);
            set => FileEditor.SetByte(unknown11, (byte)value);
        }

        public int Unknown12
        {
            get => FileEditor.GetByte(unknown12);
            set => FileEditor.SetByte(unknown12, (byte)value);
        }

        public int Unknown13
        {
            get => FileEditor.GetByte(unknown13);
            set => FileEditor.SetByte(unknown13, (byte)value);
        }

        public int Unknown14
        {
            get => FileEditor.GetByte(unknown14);
            set => FileEditor.SetByte(unknown14, (byte)value);
        }

        public int Unknown15
        {
            get => FileEditor.GetByte(unknown15);
            set => FileEditor.SetByte(unknown15, (byte)value);
        }

        public int Unknown16
        {
            get => FileEditor.GetByte(unknown16);
            set => FileEditor.SetByte(unknown16, (byte)value);
        }

        public int Unknown17
        {
            get => FileEditor.GetByte(unknown17);
            set => FileEditor.SetByte(unknown17, (byte)value);
        }

        public int Unknown18
        {
            get => FileEditor.GetByte(unknown18);
            set => FileEditor.SetByte(unknown18, (byte)value);
        }

        public int Unknown19
        {
            get => FileEditor.GetByte(unknown19);
            set => FileEditor.SetByte(unknown19, (byte)value);
        }

        public int Unknown20
        {
            get => FileEditor.GetByte(unknown20);
            set => FileEditor.SetByte(unknown20, (byte)value);
        }

        public int Unknown21
        {
            get => FileEditor.GetByte(unknown21);
            set => FileEditor.SetByte(unknown21, (byte)value);
        }

        public int Unknown22
        {
            get => FileEditor.GetByte(unknown22);
            set => FileEditor.SetByte(unknown22, (byte)value);
        }

        public int Unknown23
        {
            get => FileEditor.GetByte(unknown23);
            set => FileEditor.SetByte(unknown23, (byte)value);
        }

        public int Unknown24
        {
            get => FileEditor.GetByte(unknown24);
            set => FileEditor.SetByte(unknown24, (byte)value);
        }

        public int Unknown25
        {
            get => FileEditor.GetByte(unknown25);
            set => FileEditor.SetByte(unknown25, (byte)value);
        }

        public int Unknown26
        {
            get => FileEditor.GetByte(unknown26);
            set => FileEditor.SetByte(unknown26, (byte)value);
        }

        public int Unknown27
        {
            get => FileEditor.GetByte(unknown27);
            set => FileEditor.SetByte(unknown27, (byte)value);
        }

        public int Unknown28
        {
            get => FileEditor.GetByte(unknown28);
            set => FileEditor.SetByte(unknown28, (byte)value);
        }

        public int Unknown29
        {
            get => FileEditor.GetByte(unknown29);
            set => FileEditor.SetByte(unknown29, (byte)value);
        }

        public int Unknown30
        {
            get => FileEditor.GetByte(unknown30);
            set => FileEditor.SetByte(unknown30, (byte)value);
        }

        public int Unknown31
        {
            get => FileEditor.GetByte(unknown31);
            set => FileEditor.SetByte(unknown31, (byte)value);
        }

        public int Unknown32
        {
            get => FileEditor.GetByte(unknown32);
            set => FileEditor.SetByte(unknown32, (byte)value);
        }

        public int Unknown33
        {
            get => FileEditor.GetByte(unknown33);
            set => FileEditor.SetByte(unknown33, (byte)value);
        }

        public int Unknown34
        {
            get => FileEditor.GetByte(unknown34);
            set => FileEditor.SetByte(unknown34, (byte)value);
        }

        public int Unknown35
        {
            get => FileEditor.GetByte(unknown35);
            set => FileEditor.SetByte(unknown35, (byte)value);
        }

        public int Unknown36
        {
            get => FileEditor.GetByte(unknown36);
            set => FileEditor.SetByte(unknown36, (byte)value);
        }

        public int Unknown37
        {
            get => FileEditor.GetByte(unknown37);
            set => FileEditor.SetByte(unknown37, (byte)value);
        }

        public int Unknown38
        {
            get => FileEditor.GetByte(unknown38);
            set => FileEditor.SetByte(unknown38, (byte)value);
        }

        public int Unknown39
        {
            get => FileEditor.GetByte(unknown39);
            set => FileEditor.SetByte(unknown39, (byte)value);
        }

        public int Unknown40
        {
            get => FileEditor.GetWord(unknown40);
            set => FileEditor.SetWord(unknown40, value);
        }

        public int Address => (address);
    }
}
