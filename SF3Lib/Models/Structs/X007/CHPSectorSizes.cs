using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X007 {
    public class CHPSectorSizes : Struct {
        public readonly int _characterIdAddr;
        public readonly int _chpFileIdAddr;
        public readonly int _uIdleSectorAddr;
        public readonly int _uIdleSizeInIntsAddr;
        public readonly int _uActiveSectorAddr;
        public readonly int _uActiveSizeInIntsAddr;
        public readonly int _p1IdleSectorAddr;
        public readonly int _p1IdleSizeInIntsAddr;
        public readonly int _p1ActiveSectorAddr;
        public readonly int _p1ActiveSizeInIntsAddr;
        public readonly int _p2IdeSectorAddr;
        public readonly int _p2IdleSizeInIntsAddr;
        public readonly int _p2ActiveSectorAddr;
        public readonly int _p2ActiveSizeInIntsAddr;

        public CHPSectorSizes(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x1c) {
            _characterIdAddr        = address + 0x00;
            _chpFileIdAddr          = address + 0x02;

            _uIdleSectorAddr        = address + 0x04;
            _uIdleSizeInIntsAddr    = address + 0x06;
            _uActiveSectorAddr      = address + 0x08;
            _uActiveSizeInIntsAddr  = address + 0x0A;

            _p1IdleSectorAddr       = address + 0x0C;
            _p1IdleSizeInIntsAddr   = address + 0x0E;
            _p1ActiveSectorAddr     = address + 0x10;
            _p1ActiveSizeInIntsAddr = address + 0x12;

            _p2IdeSectorAddr        = address + 0x14;
            _p2IdleSizeInIntsAddr   = address + 0x16;
            _p2ActiveSectorAddr     = address + 0x18;
            _p2ActiveSizeInIntsAddr = address + 0x1A;
        }

        [TableViewModelColumn(addressField: nameof(_characterIdAddr), displayOrder: 0, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public short CharacterID {
            get => (short) Data.GetWord(_characterIdAddr);
            set => Data.SetWord(_characterIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_chpFileIdAddr), displayOrder: 1, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        [NameGetter(NamedValueType.FileIndex)]
        public short CHPFileId {
            get => (short) Data.GetWord(_chpFileIdAddr);
            set => Data.SetWord(_chpFileIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_uIdleSectorAddr), displayOrder: 2, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short U_Idle_Sector {
            get => (short) Data.GetWord(_uIdleSectorAddr);
            set => Data.SetWord(_uIdleSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_uIdleSizeInIntsAddr), displayOrder: 3, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short U_Idle_SizeInInts {
            get => (short) Data.GetWord(_uIdleSizeInIntsAddr);
            set => Data.SetWord(_uIdleSizeInIntsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_uActiveSectorAddr), displayOrder: 4, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short U_Active_Sector {
            get => (short) Data.GetWord(_uActiveSectorAddr);
            set => Data.SetWord(_uActiveSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_uActiveSizeInIntsAddr), displayOrder: 5, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short U_Active_SizeInInts {
            get => (short) Data.GetWord(_uActiveSizeInIntsAddr);
            set => Data.SetWord(_uActiveSizeInIntsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p1IdleSectorAddr), displayOrder: 6, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P1_Idle_Sector {
            get => (short) Data.GetWord(_p1IdleSectorAddr);
            set => Data.SetWord(_p1IdleSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p1IdleSizeInIntsAddr), displayOrder: 7, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P1_Idle_SizeInInts {
            get => (short) Data.GetWord(_p1IdleSizeInIntsAddr);
            set => Data.SetWord(_p1IdleSizeInIntsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p1ActiveSectorAddr), displayOrder: 8, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P1_Active_Sector {
            get => (short) Data.GetWord(_p1ActiveSectorAddr);
            set => Data.SetWord(_p1ActiveSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p1ActiveSizeInIntsAddr), displayOrder: 9, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P1_Active_SizeInInts {
            get => (short) Data.GetWord(_p1ActiveSizeInIntsAddr);
            set => Data.SetWord(_p1ActiveSizeInIntsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p2IdeSectorAddr), displayOrder: 10, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P2_Idle_Sector {
            get => (short) Data.GetWord(_p2IdeSectorAddr);
            set => Data.SetWord(_p2IdeSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p2IdleSizeInIntsAddr), displayOrder: 11, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P2_Idle_SizeInInts {
            get => (short) Data.GetWord(_p2IdleSizeInIntsAddr);
            set => Data.SetWord(_p2IdleSizeInIntsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p2ActiveSectorAddr), displayOrder: 12, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P2_Active_Sector {
            get => (short) Data.GetWord(_p2ActiveSectorAddr);
            set => Data.SetWord(_p2ActiveSectorAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_p2ActiveSizeInIntsAddr), displayOrder: 13, displayFormat: "X2", minWidth: 150)]
        [BulkCopy]
        public short P2_Active_SizeInInts {
            get => (short) Data.GetWord(_p2ActiveSizeInIntsAddr);
            set => Data.SetWord(_p2ActiveSizeInIntsAddr, value);
        }
    }
}
