using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class Loading : Struct {
        private readonly int locationID;
        private readonly int x1;
        private readonly int chp;
        private readonly int x5;
        private readonly int music;
        private readonly int mpd;
        private readonly int unknown;
        private readonly int chr;

        public Loading(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            locationID = Address;        // 2 bytes
            x1         = Address + 0x02; // 2 bytes
            chp        = Address + 0x04; // 2 bytes
            x5         = Address + 0x06; // 2 bytes
            music      = Address + 0x08; // 2 bytes
            mpd        = Address + 0x0a; // 2 bytes
            unknown    = Address + 0x0c; // 2 bytes
            chr        = Address + 0x0e; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "Location", displayFormat: "X2")]
        [BulkCopy]
        public int LocationID {
            get => Data.GetWord(locationID);
            set => Data.SetWord(locationID, value);
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X1 {
            get => Data.GetWord(x1);
            set => Data.SetWord(x1, value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "CHP? IsBattle?")]
        [BulkCopy]
        public int CHP {
            get => Data.GetWord(chp);
            set => Data.SetWord(chp, value);
        }

        [TableViewModelColumn(displayOrder: 3, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int X5 {
            get => Data.GetWord(x5);
            set => Data.SetWord(x5, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int Music {
            get => Data.GetWord(music);
            set => Data.SetWord(music, value);
        }

        [TableViewModelColumn(displayOrder: 5, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MPD {
            get => Data.GetWord(mpd);
            set => Data.SetWord(mpd, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "# of Maps?")]
        [BulkCopy]
        public int LoadUnknown {
            get => Data.GetWord(unknown);
            set => Data.SetWord(unknown, value);
        }

        [TableViewModelColumn(displayOrder: 7, minWidth: 140, displayFormat: "X3")]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int CHR {
            get => Data.GetWord(chr);
            set => Data.SetWord(chr, value);
        }
    }
}
