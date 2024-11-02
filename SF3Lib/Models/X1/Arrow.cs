using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Arrow : IModel {
        private readonly int unknown0; //2 byte
        private readonly int textID; //2 byte
        private readonly int unknown4; //2 byte
        private readonly int warpInMPD; //2 byte
        private readonly int unknown8; //2 byte
        private readonly int unknownA; //2 byte

        public Arrow(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x0c;

            int offset = 0;
            int sub;

            if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000060; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000060; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000060; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            /*
            else if (Scenario == ScenarioType.BTL99)
            {
                offset = 0x00000030; //btl99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset = offset - sub;
            }*/

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0c);
            unknown0 = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            textID = start + 0x02;
            unknown4 = start + 0x04;
            warpInMPD = start + 0x06;
            unknown8 = start + 0x08;
            unknownA = start + 0x0a;

            //unknown42 = start + 52;
            Address = offset + (id * 0x0c);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int ArrowUnknown0 {
            get => Editor.GetWord(unknown0);
            set => Editor.SetWord(unknown0, value);
        }

        [BulkCopy]
        public int ArrowText {
            get => Editor.GetWord(textID);
            set => Editor.SetWord(textID, value);
        }

        [BulkCopy]
        public int ArrowUnknown4 {
            get => Editor.GetWord(unknown4);
            set => Editor.SetWord(unknown4, value);
        }

        [BulkCopy]
        public int ArrowWarp {
            get => Editor.GetWord(warpInMPD);
            set => Editor.SetWord(warpInMPD, value);
        }

        [BulkCopy]
        public int ArrowUnknown8 {
            get => Editor.GetWord(unknown8);
            set => Editor.SetWord(unknown8, value);
        }

        [BulkCopy]
        public int ArrowUnknownA {
            get => Editor.GetWord(unknownA);
            set => Editor.SetWord(unknownA, value);
        }
    }
}
