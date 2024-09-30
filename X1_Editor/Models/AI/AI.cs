using SF3.Editor;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.AI
{
    public class AI
    {
        private int targetX;
        private int targetY;

        private int address;
        private int offset;
        private int sub;

        private int index;
        private string name;

        public AI(int id, string text)
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
                    offset = offset + 0x126; //size of something else
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
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            targetX = start; //2 bytes
            targetY = start + 2; //2 bytes
            address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public int AIID => index;
        public string AIName => name;

        public int TargetX
        {
            get => FileEditor.getWord(targetX);
            set => FileEditor.setWord(targetX, value);
        }
        public int TargetY
        {
            get => FileEditor.getWord(targetY);
            set => FileEditor.setWord(targetY, value);
        }
        public int AIAddress => (address);
    }
}
