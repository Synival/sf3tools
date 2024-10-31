using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Arrow {
        private readonly IX1_FileEditor _fileEditor;

        private readonly int unknown0; //2 byte
        private readonly int textID; //2 byte
        private readonly int unknown4; //2 byte
        private readonly int warpInMPD; //2 byte
        private readonly int unknown8; //2 byte
        private readonly int unknownA; //2 byte
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

        public Arrow(IX1_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000060; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000060; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000060; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset -= sub;
            }
            /*
            else if (Scenario == ScenarioType.BTL99)
            {
                offset = 0x00000030; //btl99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }*/

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ArrowID = id;
            ArrowName = text;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0c);
            unknown0 = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            textID = start + 0x02;
            unknown4 = start + 0x04;
            warpInMPD = start + 0x06;
            unknown8 = start + 0x08;
            unknownA = start + 0x0a;

            //unknown42 = start + 52;
            ArrowAddress = offset + (id * 0x0c);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int ArrowID { get; }

        [BulkCopyRowName]
        public string ArrowName { get; }

        [BulkCopy]
        public int ArrowUnknown0 {
            get => _fileEditor.GetWord(unknown0);
            set => _fileEditor.SetWord(unknown0, value);
        }

        [BulkCopy]
        public int ArrowText {
            get => _fileEditor.GetWord(textID);
            set => _fileEditor.SetWord(textID, value);
        }

        [BulkCopy]
        public int ArrowUnknown4 {
            get => _fileEditor.GetWord(unknown4);
            set => _fileEditor.SetWord(unknown4, value);
        }

        [BulkCopy]
        public int ArrowWarp {
            get => _fileEditor.GetWord(warpInMPD);
            set => _fileEditor.SetWord(warpInMPD, value);
        }

        [BulkCopy]
        public int ArrowUnknown8 {
            get => _fileEditor.GetWord(unknown8);
            set => _fileEditor.SetWord(unknown8, value);
        }

        [BulkCopy]
        public int ArrowUnknownA {
            get => _fileEditor.GetWord(unknownA);
            set => _fileEditor.SetWord(unknownA, value);
        }

        public int ArrowAddress { get; }
    }
}
