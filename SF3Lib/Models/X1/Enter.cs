using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Enter : IModel {
        private readonly int enterID; //2 byte
        private readonly int unknown2; //2 byte
        private readonly int xPos; //2 byte
        private readonly int unknown6; //2 byte
        private readonly int zPos; //2 byte
        private readonly int direction; //2 byte
        private readonly int camera; //2 byte
        private readonly int unknownE; //2 byte
                                       //private int npcOffset;

        public Enter(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x10;

            int offset = 0;
            int sub;

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000024; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);

                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000030; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000030; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub;
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000030; //pd initial pointer
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

            var start = offset + (id * 0x10);
            enterID = start; //2 bytes. how is searched. second by being 0x13 is a treasure. if this is 0xffff terminate 
            unknown2 = start + 0x02; //unknown+0x02
            xPos = start + 0x04;
            unknown6 = start + 0x06;
            zPos = start + 0x08;
            direction = start + 0x0a;
            camera = start + 0x0c;
            unknownE = start + 0x0e;

            //unknown42 = start + 52;
            Address = offset + (id * 0x10);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Entered {
            get => Editor.GetWord(enterID);
            set => Editor.SetWord(enterID, value);
        }

        [BulkCopy]
        public int EnterUnknown2 {
            get => Editor.GetWord(unknown2);
            set => Editor.SetWord(unknown2, value);
        }

        [BulkCopy]
        public int EnterXPos {
            get => Editor.GetWord(xPos);
            set => Editor.SetWord(xPos, value);
        }

        [BulkCopy]
        public int EnterUnknown6 {
            get => Editor.GetWord(unknown6);
            set => Editor.SetWord(unknown6, value);
        }

        [BulkCopy]
        public int EnterZPos {
            get => Editor.GetWord(zPos);
            set => Editor.SetWord(zPos, value);
        }

        [BulkCopy]
        public int EnterDirection {
            get => Editor.GetWord(direction);
            set => Editor.SetWord(direction, value);
        }

        [BulkCopy]
        public int EnterCamera {
            get => Editor.GetWord(camera);
            set => Editor.SetWord(camera, value);
        }

        [BulkCopy]
        public int EnterUnknownE {
            get => Editor.GetWord(unknownE);
            set => Editor.SetWord(unknownE, value);
        }
    }
}
