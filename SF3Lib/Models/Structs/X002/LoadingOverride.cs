using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X002 {
    public class LoadingOverride : Struct {
        private readonly int _mapIDAddr;
        private readonly int _synMusicAddr;
        private readonly int _medMusicAddr;
        private readonly int _julMusicAddr;
        private readonly int _extraMusicAddr;
        private readonly int _synMpdAddr;
        private readonly int _medMPDAddr;
        private readonly int _julMPDAddr;
        private readonly int _extraMPDAddr;
        private readonly int _synCHRAddr;
        private readonly int _medCHRAddr;
        private readonly int _julCHRAddr;
        private readonly int _extraCHRAddr;

        public LoadingOverride(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x28) {
            _mapIDAddr      = Address + 0x00; // 2 bytes
            _synMusicAddr   = Address + 0x02; // 1 byte
            _medMusicAddr   = Address + 0x03; // 1 byte
            _julMusicAddr   = Address + 0x04; // 1 byte
            _extraMusicAddr = Address + 0x05; // 1 byte
            _synMpdAddr     = Address + 0x06; // 4 bytes mpd synbios?
            _medMPDAddr     = Address + 0x0a; // 4 bytes mpd medion
            _julMPDAddr     = Address + 0x0e; // 4 bytes mpd julian?
            _extraMPDAddr   = Address + 0x12; // 4 bytes mpd extra?
            _synCHRAddr     = Address + 0x16; // 4 bytes chr synbios?
            _medCHRAddr     = Address + 0x1a; // 4 bytes chr medion
            _julCHRAddr     = Address + 0x1e; // 4 bytes chr julian?
            _extraCHRAddr   = Address + 0x22; // 4 bytes chr extra?
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.Load)]
        public int MapID {
            get => Data.GetWord(_mapIDAddr);
            set => Data.SetWord(_mapIDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public int SynMusic {
            get => Data.GetByte(_synMusicAddr);
            set => Data.SetByte(_synMusicAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public int MedMusic {
            get => Data.GetByte(_medMusicAddr);
            set => Data.SetByte(_medMusicAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public int JulMusic {
            get => Data.GetByte(_julMusicAddr);
            set => Data.SetByte(_julMusicAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int ExtraMusic {
            get => Data.GetByte(_extraMusicAddr);
            set => Data.SetByte(_extraMusicAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 5, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMPD {
            get => Data.GetDouble(_synMpdAddr);
            set => Data.SetDouble(_synMpdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMPD {
            get => Data.GetDouble(_medMPDAddr);
            set => Data.SetDouble(_medMPDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMPD {
            get => Data.GetDouble(_julMPDAddr);
            set => Data.SetDouble(_julMPDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 8, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMPD {
            get => Data.GetDouble(_extraMPDAddr);
            set => Data.SetDouble(_extraMPDAddr, value);
        }

        [TableViewModelColumn(displayOrder: 9, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynCHR {
            get => Data.GetDouble(_synCHRAddr);
            set => Data.SetDouble(_synCHRAddr, value);
        }

        [TableViewModelColumn(displayOrder: 10, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedCHR {
            get => Data.GetDouble(_medCHRAddr);
            set => Data.SetDouble(_medCHRAddr, value);
        }

        [TableViewModelColumn(displayOrder: 11, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulCHR {
            get => Data.GetDouble(_julCHRAddr);
            set => Data.SetDouble(_julCHRAddr, value);
        }

        [TableViewModelColumn(displayOrder: 12, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraCHR {
            get => Data.GetDouble(_extraCHRAddr);
            set => Data.SetDouble(_extraCHRAddr, value);
        }
    }
}
