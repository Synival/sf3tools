using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;
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

        public LoadedOverride(IRawData editor, int id, string name, int address)
        : base(editor, id, name, address, 0x28) {
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

        [BulkCopy]
        public int MOMapID {
            get => Data.GetWord(mapID);
            set => Data.SetWord(mapID, value);
        }

        [BulkCopy]
        public int SynMusic {
            get => Data.GetByte(synMusic);
            set => Data.SetByte(synMusic, (byte) value);
        }

        [BulkCopy]
        public int MedMusic {
            get => Data.GetByte(medMusic);
            set => Data.SetByte(medMusic, (byte) value);
        }

        [BulkCopy]
        public int JulMusic {
            get => Data.GetByte(julMusic);
            set => Data.SetByte(julMusic, (byte) value);
        }

        [BulkCopy]
        public int ExtraMusic {
            get => Data.GetByte(extraMusic);
            set => Data.SetByte(extraMusic, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMpd {
            get => Data.GetDouble(synMpd);
            set => Data.SetDouble(synMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMpd {
            get => Data.GetDouble(medMpd);
            set => Data.SetDouble(medMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMpd {
            get => Data.GetDouble(julMpd);
            set => Data.SetDouble(julMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMpd {
            get => Data.GetDouble(extraMpd);
            set => Data.SetDouble(extraMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynChr {
            get => Data.GetDouble(synChr);
            set => Data.SetDouble(synChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedChr {
            get => Data.GetDouble(medChr);
            set => Data.SetDouble(medChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulChr {
            get => Data.GetDouble(julChr);
            set => Data.SetDouble(julChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraChr {
            get => Data.GetDouble(extraChr);
            set => Data.SetDouble(extraChr, value);
        }
    }
}
