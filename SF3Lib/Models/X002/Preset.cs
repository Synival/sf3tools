using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class Preset : IModel {
        private readonly int spell;
        private readonly int weaponLv0;
        private readonly int weaponLv1;
        private readonly int weaponLv2;
        private readonly int weaponLv3;
        private readonly int offset;
        private readonly int checkVersion2;

        public Preset(ISF3FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x05;

            checkVersion2 = editor.GetByte(0x0000000B);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00004738; //scn1
                if (checkVersion2 == 0x10) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00004b60; //scn2
                if (checkVersion2 == 0x2C)
                    offset -= 0x44;
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00005734; //scn3
            }
            else {
                offset = 0x00005820; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            Name = name;

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 5);
            spell = start; //2 bytes
            weaponLv0 = start + 1; //1 byte
            weaponLv1 = start + 2; //1 byte
            weaponLv2 = start + 3; //1 byte
            weaponLv3 = start + 4; //1 byte
            Address = offset + (id * 0x05);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int SpellID2 {
            get => Editor.GetByte(spell);
            set => Editor.SetByte(spell, (byte) value);
        }

        [BulkCopy]
        public int Weapon0 {
            get => Editor.GetByte(weaponLv0);
            set => Editor.SetByte(weaponLv0, (byte) value);
        }

        [BulkCopy]
        public int Weapon1 {
            get => Editor.GetByte(weaponLv1);
            set => Editor.SetByte(weaponLv1, (byte) value);
        }

        [BulkCopy]
        public int Weapon2 {
            get => Editor.GetByte(weaponLv2);
            set => Editor.SetByte(weaponLv2, (byte) value);
        }

        [BulkCopy]
        public int Weapon3 {
            get => Editor.GetByte(weaponLv3);
            set => Editor.SetByte(weaponLv3, (byte) value);
        }
    }
}
