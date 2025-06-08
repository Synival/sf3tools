using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class StatusEffect : Struct {
        private readonly int _luck0ChanceAddr;
        private readonly int _luck0UnknownAddr;
        private readonly int _luck1ChanceAddr;
        private readonly int _luck1UnknownAddr;
        private readonly int _luck2ChanceAddr;
        private readonly int _luck2UnknownAddr;
        private readonly int _luck3ChanceAddr;
        private readonly int _luck3UnknownAddr;
        private readonly int _luck4ChanceAddr;
        private readonly int _luck4UnknownAddr;
        private readonly int _luck5ChanceAddr;
        private readonly int _luck5UnknownAddr;
        private readonly int _luck6ChanceAddr;
        private readonly int _luck6UnknownAddr;
        private readonly int _luck7ChanceAddr;
        private readonly int _luck7UnknownAddr;
        private readonly int _luck8ChanceAddr;
        private readonly int _luck8UnknownAddr;
        private readonly int _luck9ChanceAddr;
        private readonly int _luck9UnknownAddr;

        public StatusEffect(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x18) {
            _luck0ChanceAddr  = Address;
            _luck0UnknownAddr = Address + 0x01;
            _luck1ChanceAddr  = Address + 0x02;
            _luck1UnknownAddr = Address + 0x03;
            _luck2ChanceAddr  = Address + 0x04;
            _luck2UnknownAddr = Address + 0x05;
            _luck3ChanceAddr  = Address + 0x06;
            _luck3UnknownAddr = Address + 0x07;
            _luck4ChanceAddr  = Address + 0x08;
            _luck4UnknownAddr = Address + 0x09;
            _luck5ChanceAddr  = Address + 0x0A;
            _luck5UnknownAddr = Address + 0x0B;
            _luck6ChanceAddr  = Address + 0x0C;
            _luck6UnknownAddr = Address + 0x0D;
            _luck7ChanceAddr  = Address + 0x0E;
            _luck7UnknownAddr = Address + 0x0F;
            _luck8ChanceAddr  = Address + 0x10;
            _luck8UnknownAddr = Address + 0x11;
            _luck9ChanceAddr  = Address + 0x12;
            _luck9UnknownAddr = Address + 0x13;
        }

        [TableViewModelColumn(addressField: nameof(_luck0ChanceAddr), displayOrder: 0, displayName: "Luck 0 Chance%")]
        [BulkCopy]
        public int Luck0Chance {
            get => Data.GetByte(_luck0ChanceAddr);
            set => Data.SetByte(_luck0ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck0UnknownAddr), displayOrder: 0.5f, displayName: "Luck 0 Unknown")]
        [BulkCopy]
        public int Luck0Unknown {
            get => Data.GetByte(_luck0UnknownAddr);
            set => Data.SetByte(_luck0UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck1ChanceAddr), displayOrder: 1, displayName: "Luck 1 Chance%")]
        [BulkCopy]
        public int Luck1Chance {
            get => Data.GetByte(_luck1ChanceAddr);
            set => Data.SetByte(_luck1ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck1UnknownAddr), displayOrder: 1.5f, displayName: "Luck 1 Unknown")]
        [BulkCopy]
        public int Luck1Unknown {
            get => Data.GetByte(_luck1UnknownAddr);
            set => Data.SetByte(_luck1UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck2ChanceAddr), displayOrder: 2, displayName: "Luck 2 Chance%")]
        [BulkCopy]
        public int Luck2Chance {
            get => Data.GetByte(_luck2ChanceAddr);
            set => Data.SetByte(_luck2ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck2UnknownAddr), displayOrder: 2.5f, displayName: "Luck 2 Unknown")]
        [BulkCopy]
        public int Luck2Unknown {
            get => Data.GetByte(_luck2UnknownAddr);
            set => Data.SetByte(_luck2UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck3ChanceAddr), displayOrder: 3, displayName: "Luck 3 Chance%")]
        [BulkCopy]
        public int Luck3Chance {
            get => Data.GetByte(_luck3ChanceAddr);
            set => Data.SetByte(_luck3ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck3UnknownAddr), displayOrder: 3.5f, displayName: "Luck 3 Unknown")]
        [BulkCopy]
        public int Luck3Unknown {
            get => Data.GetByte(_luck3UnknownAddr);
            set => Data.SetByte(_luck3UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck4ChanceAddr), displayOrder: 4, displayName: "Luck 4 Chance%")]
        [BulkCopy]
        public int Luck4Chance {
            get => Data.GetByte(_luck4ChanceAddr);
            set => Data.SetByte(_luck4ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck4UnknownAddr), displayOrder: 4.5f, displayName: "Luck 4 Unknown")]
        [BulkCopy]
        public int Luck4Unknown {
            get => Data.GetByte(_luck4UnknownAddr);
            set => Data.SetByte(_luck4UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck5ChanceAddr), displayOrder: 5, displayName: "Luck 5 Chance%")]
        [BulkCopy]
        public int Luck5Chance {
            get => Data.GetByte(_luck5ChanceAddr);
            set => Data.SetByte(_luck5ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck5UnknownAddr), displayOrder: 5.5f, displayName: "Luck 5 Unknown")]
        [BulkCopy]
        public int Luck5Unknown {
            get => Data.GetByte(_luck5UnknownAddr);
            set => Data.SetByte(_luck5UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck6ChanceAddr), displayOrder: 6, displayName: "Luck 6 Chance%")]
        [BulkCopy]
        public int Luck6Chance {
            get => Data.GetByte(_luck6ChanceAddr);
            set => Data.SetByte(_luck6ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck6UnknownAddr), displayOrder: 6.5f, displayName: "Luck 6 Unknown")]
        [BulkCopy]
        public int Luck6Unknown {
            get => Data.GetByte(_luck6UnknownAddr);
            set => Data.SetByte(_luck6UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck7ChanceAddr), displayOrder: 7, displayName: "Luck 7 Chance%")]
        [BulkCopy]
        public int Luck7Chance {
            get => Data.GetByte(_luck7ChanceAddr);
            set => Data.SetByte(_luck7ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck7UnknownAddr), displayOrder: 7.5f, displayName: "Luck 7 Unknown")]
        [BulkCopy]
        public int Luck7Unknown {
            get => Data.GetByte(_luck7UnknownAddr);
            set => Data.SetByte(_luck7UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck8ChanceAddr), displayOrder: 8, displayName: "Luck 8 Chance%")]
        [BulkCopy]
        public int Luck8Chance {
            get => Data.GetByte(_luck8ChanceAddr);
            set => Data.SetByte(_luck8ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck8UnknownAddr), displayOrder: 8.5f, displayName: "Luck 8 Unknown")]
        [BulkCopy]
        public int Luck8Unknown {
            get => Data.GetByte(_luck8UnknownAddr);
            set => Data.SetByte(_luck8UnknownAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck9ChanceAddr), displayOrder: 9, displayName: "Luck 9 Chance%")]
        [BulkCopy]
        public int Luck9Chance {
            get => Data.GetByte(_luck9ChanceAddr);
            set => Data.SetByte(_luck9ChanceAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luck9UnknownAddr), displayOrder: 9.5f, displayName: "Luck 9 Unknown")]
        [BulkCopy]
        public int Luck9Unknown {
            get => Data.GetByte(_luck9UnknownAddr);
            set => Data.SetByte(_luck9UnknownAddr, (byte) value);
        }
    }
}
