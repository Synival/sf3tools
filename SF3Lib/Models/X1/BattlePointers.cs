using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class BattlePointers {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int battlePointer;

        //private int npcOffset;
        private readonly int offset;
        private readonly int sub;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public BattlePointers(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer
                               //offset = _fileEditor.GetDouble(offset);
                /*
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xa90;
                */
            }
            else if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer

                //offset = _fileEditor.GetDouble(offset);
                //offset = offset - sub;

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer

                //offset = _fileEditor.GetDouble(offset);

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer

                //offset = _fileEditor.GetDouble(offset);
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //second pointer
                               //offset = _fileEditor.GetDouble(offset);
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            BattleID = id;
            BattleName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x4);
            battlePointer = start; //2 bytes 
            //battlePointer2 = start +2; //2 bytes
            //unknown42 = start + 52;
            BattleAddress = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int BattleID { get; }
        public string BattleName { get; }

        /*public int BattlePointer
        {
            get => _fileEditor.GetWord(battlePointer);
            set => _fileEditor.SetWord(battlePointer, value);
        }
        */

        public int BattlePointer {
            get => _fileEditor.GetDouble(battlePointer);
            set => _fileEditor.SetDouble(battlePointer, value);
        }

        /*public int BattlePointer2
        {
            get => _fileEditor.GetWord(battlePointer2);
            set => _fileEditor.SetWord(battlePointer2, value);
        }
        */

        public int BattleAddress { get; }
    }
}
