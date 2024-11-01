using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class MagicBonus {
        private readonly IX013_FileEditor _fileEditor;

        private readonly int earthBonus;
        private readonly int fireBonus;
        private readonly int iceBonus;
        private readonly int sparkBonus;
        private readonly int windBonus;
        private readonly int lightBonus;
        private readonly int darkBonus;
        private readonly int unknownBonus;
        private readonly int offset;
        private readonly int checkVersion2;

        public MagicBonus(IX013_FileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00006e70; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00006ec8; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00006a40; //scn3
            }
            else {
                offset = 0x00006914; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            MagicID = id;
            MagicName = text;

            //int start = 0x354c + (id * 24);

            if (Scenario == ScenarioType.Scenario1) {
                var start = offset + (id * 0x20);
                earthBonus = start + 0x00; //4 bytes
                fireBonus = start + 0x04; //4 bytes
                iceBonus = start + 0x08; //4 bytes
                sparkBonus = start + 0x0C; //4 bytes
                windBonus = start + 0x10; //4 bytes
                lightBonus = start + 0x14; //4 bytes
                darkBonus = start + 0x18; //4 bytes
                unknownBonus = start + 0x1C; //4 byte

                MagicAddress = offset + (id * 0x20);
            }
            else {
                var start = offset + (id * 8);
                earthBonus = start; //1 bytes
                fireBonus = start + 1; //1 byte
                iceBonus = start + 2; //1 byte
                sparkBonus = start + 3; //1 byte
                windBonus = start + 4; //1 byte
                lightBonus = start + 5; //1 byte
                darkBonus = start + 6; //1 byte
                unknownBonus = start + 7; //1 byte

                MagicAddress = offset + (id * 0x8);
            }

            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public bool Has32BitValues => _fileEditor.Scenario == ScenarioType.Scenario1;
        public int MagicID { get; }

        [BulkCopyRowName]
        public string MagicName { get; }

        [BulkCopy]
        public int EarthBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(earthBonus) : (sbyte) _fileEditor.GetByte(earthBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(earthBonus, value);
                else
                    _fileEditor.SetByte(earthBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int FireBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(fireBonus) : (sbyte) _fileEditor.GetByte(fireBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(fireBonus, value);
                else
                    _fileEditor.SetByte(fireBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int IceBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(iceBonus) : (sbyte) _fileEditor.GetByte(iceBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(iceBonus, value);
                else
                    _fileEditor.SetByte(iceBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int SparkBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(sparkBonus) : (sbyte) _fileEditor.GetByte(sparkBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(sparkBonus, value);
                else
                    _fileEditor.SetByte(sparkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int WindBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(windBonus) : (sbyte) _fileEditor.GetByte(windBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(windBonus, value);
                else
                    _fileEditor.SetByte(windBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int LightBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(lightBonus) : (sbyte) _fileEditor.GetByte(lightBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(lightBonus, value);
                else
                    _fileEditor.SetByte(lightBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int DarkBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(darkBonus) : (sbyte) _fileEditor.GetByte(darkBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(darkBonus, value);
                else
                    _fileEditor.SetByte(darkBonus, (byte) value);
            }
        }

        [BulkCopy]
        public int UnknownBonus {
            get => Has32BitValues ? _fileEditor.GetDouble(unknownBonus) : (sbyte) _fileEditor.GetByte(unknownBonus);
            set {
                if (Has32BitValues)
                    _fileEditor.SetDouble(unknownBonus, value);
                else
                    _fileEditor.SetByte(unknownBonus, (byte) value);
            }
        }

        public int MagicAddress { get; }
    }
}
