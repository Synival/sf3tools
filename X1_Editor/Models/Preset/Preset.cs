using SF3.Editor;
using SF3.Types;
using static SF3.X1_Editor.Forms.frmMain;

namespace SF3.X1_Editor.Models.Presets
{
    public class Preset
    {
        private int unknown1;
        private int tableSize;
        private int unknown2;
        private int unknown3;
        private int unknown4;
        private int unknown5;
        private int unknown6;
        private int unknown7;

        private int unknown8;
        private int unknown9;

        private int offset;
        private int address;

        private int index;
        private string name;
        private int sub;

        public Preset(ScenarioType scenario, int id, string text)
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
                }
            }
            else if (Scenario == ScenarioType.Other /* BTL99 */)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub + Globals.map; //second pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //third pointer
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x0A);
            unknown1 = start; //1 bytes
            tableSize = start + 1; //1 byte
            unknown2 = start + 2; //1 byte
            unknown3 = start + 3; //1 byte
            unknown4 = start + 4; //1 byte
            unknown5 = start + 5;
            unknown6 = start + 6;
            unknown7 = start + 7;

            unknown8 = start + 8;
            unknown9 = start + 9;

            address = offset + (id * 0x0A);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int SizeID => index;
        public string SizeName => name;

        public int SizeUnknown1
        {
            get => FileEditor.GetByte(unknown1);
            set => FileEditor.SetByte(unknown1, (byte)value);
        }
        public int TableSize
        {
            get => FileEditor.GetByte(tableSize);
            set => FileEditor.SetByte(tableSize, (byte)value);
        }
        public int SizeUnknown2
        {
            get => FileEditor.GetByte(unknown2);
            set => FileEditor.SetByte(unknown2, (byte)value);
        }
        public int SizeUnknown3
        {
            get => FileEditor.GetByte(unknown3);
            set => FileEditor.SetByte(unknown3, (byte)value);
        }
        public int SizeUnknown4
        {
            get => FileEditor.GetByte(unknown4);
            set => FileEditor.SetByte(unknown4, (byte)value);
        }

        public int SizeUnknown5
        {
            get => FileEditor.GetByte(unknown5);
            set => FileEditor.SetByte(unknown5, (byte)value);
        }

        public int SizeUnknown6
        {
            get => FileEditor.GetByte(unknown6);
            set => FileEditor.SetByte(unknown6, (byte)value);
        }

        public int SizeUnknown7
        {
            get => FileEditor.GetByte(unknown7);
            set => FileEditor.SetByte(unknown7, (byte)value);
        }

        public int SizeUnknown8
        {
            get => FileEditor.GetByte(unknown8);
            set => FileEditor.SetByte(unknown8, (byte)value);
        }

        public int SizeUnknown9
        {
            get => FileEditor.GetByte(unknown9);
            set => FileEditor.SetByte(unknown9, (byte)value);
        }

        public int Map => Globals.map;

        public int SizeAddress => (address);
    }
}
