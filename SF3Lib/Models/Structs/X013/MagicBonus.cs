using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Structs.X013 {
    public class MagicBonus : Struct {
        private readonly int earthBonus;
        private readonly int fireBonus;
        private readonly int iceBonus;
        private readonly int sparkBonus;
        private readonly int windBonus;
        private readonly int lightBonus;
        private readonly int darkBonus;
        private readonly int unknownBonus;

        public MagicBonus(IRawData editor, int id, string name, int address, bool has32BitValues)
        : base(editor, id, name, address, has32BitValues ? 0x20 : 0x08) {
            Has32BitValues = has32BitValues;

            if (has32BitValues) {
                earthBonus   = Address + 0x00; // 4 bytes
                fireBonus    = Address + 0x04; // 4 bytes
                iceBonus     = Address + 0x08; // 4 bytes
                sparkBonus   = Address + 0x0C; // 4 bytes
                windBonus    = Address + 0x10; // 4 bytes
                lightBonus   = Address + 0x14; // 4 bytes
                darkBonus    = Address + 0x18; // 4 bytes
                unknownBonus = Address + 0x1C; // 4 bytes
            }
            else {
                earthBonus   = Address;     // 1 byte
                fireBonus    = Address + 1; // 1 byte
                iceBonus     = Address + 2; // 1 byte
                sparkBonus   = Address + 3; // 1 byte
                windBonus    = Address + 4; // 1 byte
                lightBonus   = Address + 5; // 1 byte
                darkBonus    = Address + 6; // 1 byte
                unknownBonus = Address + 7; // 1 byte
            }
        }

        public bool Has32BitValues { get; }

        [BulkCopy]
        public int EarthBonus {
            get => Has32BitValues ? Data.GetDouble(earthBonus) : (sbyte) Data.GetByte(earthBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(earthBonus, value);
                else
                    Data.SetByte(earthBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int FireBonus {
            get => Has32BitValues ? Data.GetDouble(fireBonus) : (sbyte) Data.GetByte(fireBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(fireBonus, value);
                else
                    Data.SetByte(fireBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int IceBonus {
            get => Has32BitValues ? Data.GetDouble(iceBonus) : (sbyte) Data.GetByte(iceBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(iceBonus, value);
                else
                    Data.SetByte(iceBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int SparkBonus {
            get => Has32BitValues ? Data.GetDouble(sparkBonus) : (sbyte) Data.GetByte(sparkBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(sparkBonus, value);
                else
                    Data.SetByte(sparkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int WindBonus {
            get => Has32BitValues ? Data.GetDouble(windBonus) : (sbyte) Data.GetByte(windBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(windBonus, value);
                else
                    Data.SetByte(windBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int LightBonus {
            get => Has32BitValues ? Data.GetDouble(lightBonus) : (sbyte) Data.GetByte(lightBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(lightBonus, value);
                else
                    Data.SetByte(lightBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int DarkBonus {
            get => Has32BitValues ? Data.GetDouble(darkBonus) : (sbyte) Data.GetByte(darkBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(darkBonus, value);
                else
                    Data.SetByte(darkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int UnknownBonus {
            get => Has32BitValues ? Data.GetDouble(unknownBonus) : (sbyte) Data.GetByte(unknownBonus);
            set {
                if (Has32BitValues)
                    Data.SetDouble(unknownBonus, value);
                else
                    Data.SetByte(unknownBonus, (byte) value);
            }
        }
    }
}
