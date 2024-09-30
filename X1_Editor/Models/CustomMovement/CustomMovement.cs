using SF3.Editor;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.CustomMovement
{
    public class CustomMovement
    {
        private int unknown00;
        private int xPos1;
        private int zPos1;
        private int xPos2;
        private int zPos2;
        private int xPos3;
        private int zPos3;
        private int xPos4;
        private int zPos4;
        private int ending;

        private int address;
        private int offset;
        private int sub;

        private int index;
        private string name;

        public CustomMovement(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.getDouble(offset);

                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xe9a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xe9a; //size of the enemy spawn table
                    offset = offset + 0x126; //size of SpawnZones
                    offset = offset + 0x84; //size of AITargetPosition
                    //we're now at our offset after adding these
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.getDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = FileEditor.getDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;

                    offset = offset + 0xa8a;//size of the enemy spawn table
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
                else
                {
                    Globals.map = 4;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;

                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.getDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = FileEditor.getDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */

            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer

                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
                else
                {
                    Globals.map = 8;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
            }
            else if (Globals.scenario == 4)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub + Globals.map; //second pointer
                offset = FileEditor.getDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
                else
                {
                    Globals.map = 0;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub + Globals.map; //second pointer
                    offset = FileEditor.getDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                    offset = offset + 0x84; //size of AITargetPosition
                }
            }
            else if (Globals.scenario == 5)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //second pointer
                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xe9a;
                offset = offset + 0x126;
                offset = offset + 0x84; //size of AITargetPosition

            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x16);
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

            address = offset + (id * 0x16);
            //address = 0x0354c + (id * 0x18);

        }

        public int CustomMovementID => index;
        public string CustomMovementName => name;

        public int CustomMovementUnknown
        {
            get => FileEditor.getWord(unknown00);
            set => FileEditor.setWord(unknown00, value);
        }

        public int CustomMovementX1
        {
            get => FileEditor.getWord(xPos1);
            set => FileEditor.setWord(xPos1, value);
        }
        public int CustomMovementZ1
        {
            get => FileEditor.getWord(zPos1);
            set => FileEditor.setWord(zPos1, value);
        }

        public int CustomMovementX2
        {
            get => FileEditor.getWord(xPos2);
            set => FileEditor.setWord(xPos2, value);
        }
        public int CustomMovementZ2
        {
            get => FileEditor.getWord(zPos2);
            set => FileEditor.setWord(zPos2, value);
        }

        public int CustomMovementX3
        {
            get => FileEditor.getWord(xPos3);
            set => FileEditor.setWord(xPos3, value);
        }
        public int CustomMovementZ3
        {
            get => FileEditor.getWord(zPos3);
            set => FileEditor.setWord(zPos3, value);
        }

        public int CustomMovementX4
        {
            get => FileEditor.getWord(xPos4);
            set => FileEditor.setWord(xPos4, value);
        }
        public int CustomMovementZ4
        {
            get => FileEditor.getWord(zPos4);
            set => FileEditor.setWord(zPos4, value);
        }

        public int CustomMovementEnd
        {
            get => FileEditor.getDouble(ending);
            set => FileEditor.setDouble(ending, value);
        }

        public int CustomMovementAddress => (address);
    }
}
