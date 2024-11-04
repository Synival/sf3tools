using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class MagicBonus : Model {
        private readonly int earthBonus;
        private readonly int fireBonus;
        private readonly int iceBonus;
        private readonly int sparkBonus;
        private readonly int windBonus;
        private readonly int lightBonus;
        private readonly int darkBonus;
        private readonly int unknownBonus;

        public MagicBonus(IByteEditor editor, int id, string name, int address, bool has32BitValues)
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
            get => Has32BitValues ? Editor.GetDouble(earthBonus) : (sbyte) Editor.GetByte(earthBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(earthBonus, value);
                else
                    Editor.SetByte(earthBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int FireBonus {
            get => Has32BitValues ? Editor.GetDouble(fireBonus) : (sbyte) Editor.GetByte(fireBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(fireBonus, value);
                else
                    Editor.SetByte(fireBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int IceBonus {
            get => Has32BitValues ? Editor.GetDouble(iceBonus) : (sbyte) Editor.GetByte(iceBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(iceBonus, value);
                else
                    Editor.SetByte(iceBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int SparkBonus {
            get => Has32BitValues ? Editor.GetDouble(sparkBonus) : (sbyte) Editor.GetByte(sparkBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(sparkBonus, value);
                else
                    Editor.SetByte(sparkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int WindBonus {
            get => Has32BitValues ? Editor.GetDouble(windBonus) : (sbyte) Editor.GetByte(windBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(windBonus, value);
                else
                    Editor.SetByte(windBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int LightBonus {
            get => Has32BitValues ? Editor.GetDouble(lightBonus) : (sbyte) Editor.GetByte(lightBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(lightBonus, value);
                else
                    Editor.SetByte(lightBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int DarkBonus {
            get => Has32BitValues ? Editor.GetDouble(darkBonus) : (sbyte) Editor.GetByte(darkBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(darkBonus, value);
                else
                    Editor.SetByte(darkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int UnknownBonus {
            get => Has32BitValues ? Editor.GetDouble(unknownBonus) : (sbyte) Editor.GetByte(unknownBonus);
            set {
                if (Has32BitValues)
                    Editor.SetDouble(unknownBonus, value);
                else
                    Editor.SetByte(unknownBonus, (byte) value);
            }
        }
    }
}
