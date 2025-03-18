using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class LoadedOverride : Struct {
        private readonly int mapID;
        private readonly int synMusic;
        private readonly int medMusic;
        private readonly int julMusic;
        private readonly int extraMusic;
        private readonly int synMpd;
        private readonly int medMpd;
        private readonly int julMpd;
        private readonly int extraMpd;
        private readonly int synChr;
        private readonly int medChr;
        private readonly int julChr;
        private readonly int extraChr;

        public LoadedOverride(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x28) {
            mapID      = Address;        // 2 bytes
            synMusic   = Address + 0x02; // 1 byte
            medMusic   = Address + 0x03; // 1 byte
            julMusic   = Address + 0x04; // 1 byte
            extraMusic = Address + 0x05; // 1 byte
            synMpd     = Address + 0x06; // 4 bytes mpd synbios?
            medMpd     = Address + 0x0a; // 4 bytes mpd medion
            julMpd     = Address + 0x0e; // 4 bytes mpd julian?
            extraMpd   = Address + 0x12; // 4 bytes mpd extra?
            synChr     = Address + 0x16; // 4 bytes chr synbios?
            medChr     = Address + 0x1a; // 4 bytes chr medion
            julChr     = Address + 0x1e; // 4 bytes chr julian?
            extraChr   = Address + 0x22; // 4 bytes chr extra?
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "MapID", displayFormat: "X2")]
        [BulkCopy]
        public int MOMapID {
            get => Data.GetWord(mapID);
            set => Data.SetWord(mapID, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int SynMusic {
            get => Data.GetByte(synMusic);
            set => Data.SetByte(synMusic, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int MedMusic {
            get => Data.GetByte(medMusic);
            set => Data.SetByte(medMusic, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int JulMusic {
            get => Data.GetByte(julMusic);
            set => Data.SetByte(julMusic, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int ExtraMusic {
            get => Data.GetByte(extraMusic);
            set => Data.SetByte(extraMusic, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "SynMPD", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMpd {
            get => Data.GetDouble(synMpd);
            set => Data.SetDouble(synMpd, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayName: "MedMPD", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMpd {
            get => Data.GetDouble(medMpd);
            set => Data.SetDouble(medMpd, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayName: "JulMPD", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMpd {
            get => Data.GetDouble(julMpd);
            set => Data.SetDouble(julMpd, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayName: "ExtraMPD", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMpd {
            get => Data.GetDouble(extraMpd);
            set => Data.SetDouble(extraMpd, value);
        }

        [TableViewModelColumn(displayOrder: 9, displayName: "SynCHR", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynChr {
            get => Data.GetDouble(synChr);
            set => Data.SetDouble(synChr, value);
        }

        [TableViewModelColumn(displayOrder: 10, displayName: "MedCHR", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedChr {
            get => Data.GetDouble(medChr);
            set => Data.SetDouble(medChr, value);
        }

        [TableViewModelColumn(displayOrder: 11, displayName: "JulCHR", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulChr {
            get => Data.GetDouble(julChr);
            set => Data.SetDouble(julChr, value);
        }

        [TableViewModelColumn(displayOrder: 12, displayName: "ExtraCHR", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraChr {
            get => Data.GetDouble(extraChr);
            set => Data.SetDouble(extraChr, value);
        }
    }
}
