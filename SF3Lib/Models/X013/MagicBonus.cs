using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class MagicBonus : IModel {
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

        public MagicBonus(IX013_FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;

            Has32BitValues = editor.Scenario == ScenarioType.Scenario1;

            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00006e70; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00006ec8; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00006a40; //scn3
            }
            else {
                offset = 0x00006914; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            if (editor.Scenario == ScenarioType.Scenario1) {
                var start = offset + (id * 0x20);
                earthBonus = start + 0x00; //4 bytes
                fireBonus = start + 0x04; //4 bytes
                iceBonus = start + 0x08; //4 bytes
                sparkBonus = start + 0x0C; //4 bytes
                windBonus = start + 0x10; //4 bytes
                lightBonus = start + 0x14; //4 bytes
                darkBonus = start + 0x18; //4 bytes
                unknownBonus = start + 0x1C; //4 byte

                Address = offset + (id * 0x20);
                Size    = 0x20;
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

                Address = offset + (id * 0x8);
                Size    = 0x08;
            }

            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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
