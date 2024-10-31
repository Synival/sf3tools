using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Loading : IModel {
        private readonly int locationID;
        private readonly int x1;
        private readonly int chp;
        private readonly int x5;
        private readonly int music;
        private readonly int mpd;
        private readonly int unknown;
        private readonly int chr;
        private readonly int offset;
        private readonly int checkVersion2;

        public Loading(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x10;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x000047A4; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00004bd8; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x000057d0; //scn3
            }
            else {
                offset = 0x000058bc; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x10);
            locationID = start; //2 bytes
            x1 = start + 0x02; //2 byte
            chp = start + 0x04; //2 byte
            x5 = start + 0x06; //2 byte
            music = start + 0x08; //2 byte
            mpd = start + 0x0a; //2 bytes
            unknown = start + 0x0c; //2 bytes
            chr = start + 0x0e; //2 bytes
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
        public int LocationID {
            get => Editor.GetWord(locationID);
            set => Editor.SetWord(locationID, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X1 {
            get => Editor.GetWord(x1);
            set => Editor.SetWord(x1, value);
        }

        [BulkCopy]
        public int CHP {
            get => Editor.GetWord(chp);
            set => Editor.SetWord(chp, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X5 {
            get => Editor.GetWord(x5);
            set => Editor.SetWord(x5, value);
        }

        [BulkCopy]
        public int Music {
            get => Editor.GetWord(music);
            set => Editor.SetWord(music, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MPD {
            get => Editor.GetWord(mpd);
            set => Editor.SetWord(mpd, value);
        }

        [BulkCopy]
        public int LoadUnknown {
            get => Editor.GetWord(unknown);
            set => Editor.SetWord(unknown, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int CHR {
            get => Editor.GetWord(chr);
            set => Editor.SetWord(chr, value);
        }
    }
}
