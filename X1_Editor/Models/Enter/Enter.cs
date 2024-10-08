using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Types;
using System;

namespace SF3.X1_Editor.Models.Enters
{
    public class Enter
    {
        private int enterID; //2 byte
        private int unknown2; //2 byte
        private int xPos; //2 byte
        private int unknown6; //2 byte
        private int zPos; //2 byte
        private int direction; //2 byte
        private int camera; //2 byte
        private int unknownE; //2 byte

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

        public Enter(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000024; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.GetDouble(offset);

                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000030; //scn2 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000030; //scn3 initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000030; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            /*
            else if (Scenario == ScenarioType.BTL99)
            {
                offset = 0x00000030; //btl99 initial pointer
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

            int start = offset + (id * 0x10);
            enterID = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown2 = start + 0x02; //unknown+0x02
            xPos = start + 0x04;
            unknown6 = start + 0x06;
            zPos = start + 0x08;
            direction = start + 0x0a;
            camera = start + 0x0c;
            unknownE = start + 0x0e;

            //unknown42 = start + 52;
            address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int EnterID => index;
        public string EnterName => name;

        public int Entered
        {
            get => FileEditor.GetWord(enterID);
            set => FileEditor.SetWord(enterID, value);
        }

        public int EnterUnknown2
        {
            get => FileEditor.GetWord(unknown2);
            set => FileEditor.SetWord(unknown2, value);
        }

        public int EnterXPos
        {
            get => FileEditor.GetWord(xPos);
            set => FileEditor.SetWord(xPos, value);
        }

        public int EnterUnknown6
        {
            get => FileEditor.GetWord(unknown6);
            set => FileEditor.SetWord(unknown6, value);
        }

        public int EnterZPos
        {
            get => FileEditor.GetWord(zPos);
            set => FileEditor.SetWord(zPos, value);
        }

        public int EnterDirection
        {
            get => FileEditor.GetWord(direction);
            set => FileEditor.SetWord(direction, value);
        }

        public int EnterCamera
        {
            get => FileEditor.GetWord(camera);
            set => FileEditor.SetWord(camera, value);
        }

        public int EnterUnknownE
        {
            get => FileEditor.GetWord(unknownE);
            set => FileEditor.SetWord(unknownE, value);
        }

        public int EnterAddress => (address);
    }
}
