using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class Blacksmith : Struct {
        private readonly int _materialItemAddr;
        private readonly int _requestItemTypeAddr;
        private readonly int _result1ItemAddr;
        private readonly int _result1ChanceAddr;
        private readonly int _result2ItemAddr;
        private readonly int _result2ChanceAddr;
        private readonly int _result3ItemAddr;
        private readonly int _result3ChanceAddr;
        private readonly int _result4ItemAddr;
        private readonly int _result4ChanceAddr;
        private readonly int _result5ItemAddr;
        private readonly int _result5ChanceAddr;
        private readonly int _result6ItemAddr;
        private readonly int _result6ChanceAddr;
        private readonly int _result7ItemAddr;
        private readonly int _result7ChanceAddr;
        private readonly int _result8ItemAddr;
        private readonly int _result8ChanceAddr;
        private readonly int _costAddr;

        public Blacksmith(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x28) {
             _materialItemAddr    = Address + 0x00; // 2 bytes
             _requestItemTypeAddr = Address + 0x02; // 2 bytes
             _result1ItemAddr     = Address + 0x04; // 2 bytes
             _result1ChanceAddr   = Address + 0x06; // 2 bytes
             _result2ItemAddr     = Address + 0x08; // 2 bytes
             _result2ChanceAddr   = Address + 0x0A; // 2 bytes
             _result3ItemAddr     = Address + 0x0C; // 2 bytes
             _result3ChanceAddr   = Address + 0x0E; // 2 bytes
             _result4ItemAddr     = Address + 0x10; // 2 bytes
             _result4ChanceAddr   = Address + 0x12; // 2 bytes
             _result5ItemAddr     = Address + 0x14; // 2 bytes
             _result5ChanceAddr   = Address + 0x16; // 2 bytes
             _result6ItemAddr     = Address + 0x18; // 2 bytes
             _result6ChanceAddr   = Address + 0x1A; // 2 bytes
             _result7ItemAddr     = Address + 0x1C; // 2 bytes
             _result7ChanceAddr   = Address + 0x1E; // 2 bytes
             _result8ItemAddr     = Address + 0x20; // 2 bytes
             _result8ChanceAddr   = Address + 0x22; // 2 bytes
             _costAddr            = Address + 0x24; // 4 bytes
        }

        [TableViewModelColumn(addressField: nameof(_materialItemAddr), displayOrder: 0, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int MaterialItem {
            get => Data.GetWord(_materialItemAddr);
            set => Data.SetWord(_materialItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_requestItemTypeAddr), displayOrder: 1, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.WeaponType)]
        [BulkCopy]
        public int RequestItemType {
            get => Data.GetWord(_requestItemTypeAddr);
            set => Data.SetWord(_requestItemTypeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result1ItemAddr), displayOrder: 2, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result1Item {
            get => Data.GetWord(_result1ItemAddr);
            set => Data.SetWord(_result1ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result1ChanceAddr), displayOrder: 3, displayName: "Result1 %Chance")]
        [BulkCopy]
        public int Result1Chance {
            get => Data.GetWord(_result1ChanceAddr);
            set => Data.SetWord(_result1ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result2ItemAddr), displayOrder: 4, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result2Item {
            get => Data.GetWord(_result2ItemAddr);
            set => Data.SetWord(_result2ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result2ChanceAddr), displayOrder: 5, displayName: "Result2 %Chance")]
        [BulkCopy]
        public int Result2Chance {
            get => Data.GetWord(_result2ChanceAddr);
            set => Data.SetWord(_result2ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result3ItemAddr), displayOrder: 6, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result3Item {
            get => Data.GetWord(_result3ItemAddr);
            set => Data.SetWord(_result3ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result3ChanceAddr), displayOrder: 7, displayName: "Result3 %Chance")]
        [BulkCopy]
        public int Result3Chance {
            get => Data.GetWord(_result3ChanceAddr);
            set => Data.SetWord(_result3ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result4ItemAddr), displayOrder: 8, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result4Item {
            get => Data.GetWord(_result4ItemAddr);
            set => Data.SetWord(_result4ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result4ChanceAddr), displayOrder: 9, displayName: "Result4 %Chance")]
        [BulkCopy]
        public int Result4Chance {
            get => Data.GetWord(_result4ChanceAddr);
            set => Data.SetWord(_result4ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result5ItemAddr), displayOrder: 10, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result5Item {
            get => Data.GetWord(_result5ItemAddr);
            set => Data.SetWord(_result5ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result5ChanceAddr), displayOrder: 11, displayName: "Result5 %Chance")]
        [BulkCopy]
        public int Result5Chance {
            get => Data.GetWord(_result5ChanceAddr);
            set => Data.SetWord(_result5ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result6ItemAddr), displayOrder: 12, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result6Item {
            get => Data.GetWord(_result6ItemAddr);
            set => Data.SetWord(_result6ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result6ChanceAddr), displayOrder: 13, displayName: "Result6 %Chance")]
        [BulkCopy]
        public int Result6Chance {
            get => Data.GetWord(_result6ChanceAddr);
            set => Data.SetWord(_result6ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result7ItemAddr), displayOrder: 14, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result7Item {
            get => Data.GetWord(_result7ItemAddr);
            set => Data.SetWord(_result7ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result7ChanceAddr), displayOrder: 15, displayName: "Result7 %Chance")]
        [BulkCopy]
        public int Result7Chance {
            get => Data.GetWord(_result7ChanceAddr);
            set => Data.SetWord(_result7ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result8ItemAddr), displayOrder: 16, displayFormat: "X2", minWidth: 120)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Result8Item {
            get => Data.GetWord(_result8ItemAddr);
            set => Data.SetWord(_result8ItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_result8ChanceAddr), displayOrder: 17, displayName: "Result8 %Chance")]
        [BulkCopy]
        public int Result8Chance {
            get => Data.GetWord(_result8ChanceAddr);
            set => Data.SetWord(_result8ChanceAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_costAddr), displayOrder: 18)]
        [BulkCopy]
        public int Cost {
            get => Data.GetDouble(_costAddr);
            set => Data.SetDouble(_costAddr, value);
        }
    }
}
