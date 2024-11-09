using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class LoadedOverride : Model {
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

        public LoadedOverride(IByteEditor editor, int id, string name, int address)
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
            get => Editor.GetWord(mapID);
            set => Editor.SetWord(mapID, value);
        }

        [BulkCopy]
        public int SynMusic {
            get => Editor.GetByte(synMusic);
            set => Editor.SetByte(synMusic, (byte) value);
        }

        [BulkCopy]
        public int MedMusic {
            get => Editor.GetByte(medMusic);
            set => Editor.SetByte(medMusic, (byte) value);
        }

        [BulkCopy]
        public int JulMusic {
            get => Editor.GetByte(julMusic);
            set => Editor.SetByte(julMusic, (byte) value);
        }

        [BulkCopy]
        public int ExtraMusic {
            get => Editor.GetByte(extraMusic);
            set => Editor.SetByte(extraMusic, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMpd {
            get => Editor.GetDouble(synMpd);
            set => Editor.SetDouble(synMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMpd {
            get => Editor.GetDouble(medMpd);
            set => Editor.SetDouble(medMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMpd {
            get => Editor.GetDouble(julMpd);
            set => Editor.SetDouble(julMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMpd {
            get => Editor.GetDouble(extraMpd);
            set => Editor.SetDouble(extraMpd, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynChr {
            get => Editor.GetDouble(synChr);
            set => Editor.SetDouble(synChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedChr {
            get => Editor.GetDouble(medChr);
            set => Editor.SetDouble(medChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulChr {
            get => Editor.GetDouble(julChr);
            set => Editor.SetDouble(julChr, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraChr {
            get => Editor.GetDouble(extraChr);
            set => Editor.SetDouble(extraChr, value);
        }
    }
}
