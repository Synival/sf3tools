using SF3.Editor;
using SF3.Types;

namespace SF3.X1_Editor.Models.BattlePointers
{
    public class BattlePointers
    {
        private int battlePointer;
        //private readonly int battlePointer2;
        //private int unknown42;

        //int pointerValue;

        private readonly int address;
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

        public BattlePointers(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer

                //offset = FileEditor.GetDouble(offset);
                //offset = offset - sub;

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
                offset = offset - sub; //second pointer

                //offset = FileEditor.GetDouble(offset);

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
                offset = offset - sub; //second pointer

                //offset = FileEditor.GetDouble(offset);
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer
                                       //offset = FileEditor.GetDouble(offset);
            }
            else if (Scenario == ScenarioType.Other)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = FileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer
                //offset = FileEditor.GetDouble(offset);
                /*
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xa90;
                */
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x4);
            battlePointer = start; //2 bytes 
            //battlePointer2 = start +2; //2 bytes
            //unknown42 = start + 52;
            address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int BattleID => index;
        public string BattleName => name;

        /*public int BattlePointer
        {
            get => FileEditor.GetWord(battlePointer);
            set => FileEditor.SetWord(battlePointer, value);
        }
        */

        public int BattlePointer
        {
            get => FileEditor.GetDouble(battlePointer);
            set => FileEditor.SetDouble(battlePointer, value);
        }

        /*public int BattlePointer2
        {
            get => FileEditor.GetWord(battlePointer2);
            set => FileEditor.SetWord(battlePointer2, value);
        }
        */

        public int BattleAddress => (address);
    }
}
