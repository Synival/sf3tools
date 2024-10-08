using SF3.Editor;
using SF3.Types;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.UnknownAI
{
    public class UnknownAI
    {
        private int unknown00;
        private int unknown02;
        private int unknown04;
        private int unknown06;
        private int unknown08;
        private int unknown0A;
        private int unknown0C;
        private int unknown0E;
        private int unknown10;
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

        public UnknownAI(ScenarioType scenario, int id, string text)
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
                    offset = offset + 0xEa0;
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
                    offset = offset + 0xEa0;
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
                    offset = offset + 0xa90;
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
                    offset = offset + 0xa90;
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
                    offset = offset + 0xa90;
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
                    offset = offset + 0xa90;
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
                    offset = offset + 0xa90;
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
                    offset = offset + 0xa90;
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
                offset = offset + 0xea0;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x12);
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
            address = offset + (id * 0x12);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int UnknownAIID => index;
        public string UnknownAIName => name;

        public int UnknownAI00
        {
            get => FileEditor.GetWord(unknown00);
            set => FileEditor.SetWord(unknown00, value);
        }

        public int UnknownAI02
        {
            get => FileEditor.GetWord(unknown02);
            set => FileEditor.SetWord(unknown02, value);
        }

        public int UnknownAI04
        {
            get => FileEditor.GetWord(unknown04);
            set => FileEditor.SetWord(unknown04, value);
        }

        public int UnknownAI06
        {
            get => FileEditor.GetWord(unknown06);
            set => FileEditor.SetWord(unknown06, value);
        }

        public int UnknownAI08
        {
            get => FileEditor.GetWord(unknown08);
            set => FileEditor.SetWord(unknown08, value);
        }

        public int UnknownAI0A
        {
            get => FileEditor.GetWord(unknown0A);
            set => FileEditor.SetWord(unknown0A, value);
        }

        public int UnknownAI0C
        {
            get => FileEditor.GetWord(unknown0C);
            set => FileEditor.SetWord(unknown0C, value);
        }

        public int UnknownAI0E
        {
            get => FileEditor.GetWord(unknown0E);
            set => FileEditor.SetWord(unknown0E, value);
        }

        public int UnknownAI10
        {
            get => FileEditor.GetWord(unknown10);
            set => FileEditor.SetWord(unknown10, value);
        }

        public int UnknownAIAddress => (address);
    }
}
