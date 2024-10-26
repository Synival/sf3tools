using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tables.X013.MagicBonus {
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
                {
                    offset -= 0x0C;
                }
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
                earthBonus = start + 0x03; //1 bytes
                fireBonus = start + 0x07; //1 byte
                iceBonus = start + 0x0B; //1 byte
                sparkBonus = start + 0x0F; //1 byte
                windBonus = start + 0x13; //1 byte
                lightBonus = start + 0x17; //1 byte
                darkBonus = start + 0x1B; //1 byte
                unknownBonus = start + 0x1F; //1 byte

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
        public int MagicID { get; }
        public string MagicName { get; }

        public int EarthBonus {
            get => _fileEditor.GetByte(earthBonus);
            set => _fileEditor.SetByte(earthBonus, (byte) value);
        }

        public int FireBonus {
            get => _fileEditor.GetByte(fireBonus);
            set => _fileEditor.SetByte(fireBonus, (byte) value);
        }

        public int IceBonus {
            get => _fileEditor.GetByte(iceBonus);
            set => _fileEditor.SetByte(iceBonus, (byte) value);
        }

        public int SparkBonus {
            get => _fileEditor.GetByte(sparkBonus);
            set => _fileEditor.SetByte(sparkBonus, (byte) value);
        }

        public int WindBonus {
            get => _fileEditor.GetByte(windBonus);
            set => _fileEditor.SetByte(windBonus, (byte) value);
        }

        public int LightBonus {
            get => _fileEditor.GetByte(lightBonus);
            set => _fileEditor.SetByte(lightBonus, (byte) value);
        }

        public int DarkBonus {
            get => _fileEditor.GetByte(darkBonus);
            set => _fileEditor.SetByte(darkBonus, (byte) value);
        }

        public int UnknownBonus {
            get => _fileEditor.GetByte(unknownBonus);
            set => _fileEditor.SetByte(unknownBonus, (byte) value);
        }

        public int MagicAddress { get; }
    }
}
