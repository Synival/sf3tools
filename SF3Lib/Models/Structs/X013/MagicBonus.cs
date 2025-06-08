using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class MagicBonus : Struct {
        private readonly int _earthBonusAddr;
        private readonly int _fireBonusAddr;
        private readonly int _iceBonusAddr;
        private readonly int _sparkBonusAddr;
        private readonly int _windBonusAddr;
        private readonly int _lightBonusAddr;
        private readonly int _darkBonusAddr;
        private readonly int _unknownBonusAddr;

        public MagicBonus(IByteData data, int id, string name, int address, bool has32BitValues)
        : base(data, id, name, address, has32BitValues ? 0x20 : 0x08) {
            Has32BitValues = has32BitValues;

            if (has32BitValues) {
                _earthBonusAddr   = Address + 0x00; // 4 bytes
                _fireBonusAddr    = Address + 0x04; // 4 bytes
                _iceBonusAddr     = Address + 0x08; // 4 bytes
                _sparkBonusAddr   = Address + 0x0C; // 4 bytes
                _windBonusAddr    = Address + 0x10; // 4 bytes
                _lightBonusAddr   = Address + 0x14; // 4 bytes
                _darkBonusAddr    = Address + 0x18; // 4 bytes
                _unknownBonusAddr = Address + 0x1C; // 4 bytes
            }
            else {
                _earthBonusAddr   = Address;     // 1 byte
                _fireBonusAddr    = Address + 1; // 1 byte
                _iceBonusAddr     = Address + 2; // 1 byte
                _sparkBonusAddr   = Address + 3; // 1 byte
                _windBonusAddr    = Address + 4; // 1 byte
                _lightBonusAddr   = Address + 5; // 1 byte
                _darkBonusAddr    = Address + 6; // 1 byte
                _unknownBonusAddr = Address + 7; // 1 byte
            }
        }

        public bool Has32BitValues { get; }

        [TableViewModelColumn(addressField: nameof(_earthBonusAddr), displayOrder: 0)]
        [BulkCopy]
        public int EarthBonus {
            get => Has32BitValues ? Data.GetDouble(_earthBonusAddr) : (sbyte) Data.GetByte(_earthBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_earthBonusAddr, value);
                else
                    Data.SetByte(_earthBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_fireBonusAddr), displayOrder: 1)]
        [BulkCopy]
        public int FireBonus {
            get => Has32BitValues ? Data.GetDouble(_fireBonusAddr) : (sbyte) Data.GetByte(_fireBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_fireBonusAddr, value);
                else
                    Data.SetByte(_fireBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_iceBonusAddr), displayOrder: 2)]
        [BulkCopy]
        public int IceBonus {
            get => Has32BitValues ? Data.GetDouble(_iceBonusAddr) : (sbyte) Data.GetByte(_iceBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_iceBonusAddr, value);
                else
                    Data.SetByte(_iceBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_sparkBonusAddr), displayOrder: 3)]
        [BulkCopy]
        public int SparkBonus {
            get => Has32BitValues ? Data.GetDouble(_sparkBonusAddr) : (sbyte) Data.GetByte(_sparkBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_sparkBonusAddr, value);
                else
                    Data.SetByte(_sparkBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_windBonusAddr), displayOrder: 4)]
        [BulkCopy]
        public int WindBonus {
            get => Has32BitValues ? Data.GetDouble(_windBonusAddr) : (sbyte) Data.GetByte(_windBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_windBonusAddr, value);
                else
                    Data.SetByte(_windBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_lightBonusAddr), displayOrder: 5)]
        [BulkCopy]
        public int LightBonus {
            get => Has32BitValues ? Data.GetDouble(_lightBonusAddr) : (sbyte) Data.GetByte(_lightBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_lightBonusAddr, value);
                else
                    Data.SetByte(_lightBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_darkBonusAddr), displayOrder: 6)]
        [BulkCopy]
        public int DarkBonus {
            get => Has32BitValues ? Data.GetDouble(_darkBonusAddr) : (sbyte) Data.GetByte(_darkBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_darkBonusAddr, value);
                else
                    Data.SetByte(_darkBonusAddr, (byte) value);
            }
        }

        [TableViewModelColumn(addressField: nameof(_unknownBonusAddr), displayOrder: 7)]
        [BulkCopy]
        public int UnknownBonus {
            get => Has32BitValues ? Data.GetDouble(_unknownBonusAddr) : (sbyte) Data.GetByte(_unknownBonusAddr);
            set {
                if (Has32BitValues)
                    Data.SetDouble(_unknownBonusAddr, value);
                else
                    Data.SetByte(_unknownBonusAddr, (byte) value);
            }
        }
    }
}
