using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Loading : Model {
        private readonly int locationID;
        private readonly int x1;
        private readonly int chp;
        private readonly int x5;
        private readonly int music;
        private readonly int mpd;
        private readonly int unknown;
        private readonly int chr;

        public Loading(IByteEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x10) {
            locationID = Address;        // 2 bytes
            x1         = Address + 0x02; // 2 bytes
            chp        = Address + 0x04; // 2 bytes
            x5         = Address + 0x06; // 2 bytes
            music      = Address + 0x08; // 2 bytes
            mpd        = Address + 0x0a; // 2 bytes
            unknown    = Address + 0x0c; // 2 bytes
            chr        = Address + 0x0e; // 2 bytes
        }

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
