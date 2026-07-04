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

        [TableViewModelColumn(addressField: nameof(_mapIDAddr), displayOrder: 0, displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.Load)]
        public ushort MapID {
            get => Data.GetUInt16(_mapIDAddr);
            set => Data.SetUInt16(_mapIDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_synMusicAddr), displayOrder: 1, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public byte SynMusic {
            get => Data.GetUInt8(_synMusicAddr);
            set => Data.SetUInt8(_synMusicAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_medMusicAddr), displayOrder: 2, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public byte MedMusic {
            get => Data.GetUInt8(_medMusicAddr);
            set => Data.SetUInt8(_medMusicAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_julMusicAddr), displayOrder: 3, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public byte JulMusic {
            get => Data.GetUInt8(_julMusicAddr);
            set => Data.SetUInt8(_julMusicAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_extraMusicAddr), displayOrder: 4, displayFormat: "X2", minWidth: 210)]
        [BulkCopy]
        [NameGetter(NamedValueType.Music)]
        public byte ExtraMusic {
            get => Data.GetUInt8(_extraMusicAddr);
            set => Data.SetUInt8(_extraMusicAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_synMpdAddr), displayOrder: 5, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynMPD {
            get => Data.GetInt32(_synMpdAddr);
            set => Data.SetInt32(_synMpdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_medMPDAddr), displayOrder: 6, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedMPD {
            get => Data.GetInt32(_medMPDAddr);
            set => Data.SetInt32(_medMPDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_julMPDAddr), displayOrder: 7, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulMPD {
            get => Data.GetInt32(_julMPDAddr);
            set => Data.SetInt32(_julMPDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_extraMPDAddr), displayOrder: 8, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraMPD {
            get => Data.GetInt32(_extraMPDAddr);
            set => Data.SetInt32(_extraMPDAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_synCHRAddr), displayOrder: 9, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int SynCHR {
            get => Data.GetInt32(_synCHRAddr);
            set => Data.SetInt32(_synCHRAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_medCHRAddr), displayOrder: 10, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int MedCHR {
            get => Data.GetInt32(_medCHRAddr);
            set => Data.SetInt32(_medCHRAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_julCHRAddr), displayOrder: 11, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int JulCHR {
            get => Data.GetInt32(_julCHRAddr);
            set => Data.SetInt32(_julCHRAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_extraCHRAddr), displayOrder: 12, displayFormat: "X3", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public int ExtraCHR {
            get => Data.GetInt32(_extraCHRAddr);
            set => Data.SetInt32(_extraCHRAddr, value);
        }
    }
}
