﻿using SF3.Types;
using SF3.Values;
using SF3.FileEditors;

namespace SF3.Models.X002.Presets
{
    public class Preset
    {
        private IX002_FileEditor _fileEditor;

        private int spell;
        private int weaponLv0;
        private int weaponLv1;
        private int weaponLv2;
        private int weaponLv3;
        private int address;
        private int offset;
        private int checkVersion2;

        private int index;
        private string name;

        public Preset(IX002_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004738; //scn1
                if (checkVersion2 == 0x10) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004b60; //scn2
                if (checkVersion2 == 0x2C)
                {
                    offset = offset - 0x44;
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00005734; //scn3
            }
            else
                offset = 0x00005820; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 5);
            spell = start; //2 bytes
            weaponLv0 = start + 1; //1 byte
            weaponLv1 = start + 2; //1 byte
            weaponLv2 = start + 3; //1 byte
            weaponLv3 = start + 4; //1 byte
            address = offset + (id * 0x05);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int PresetID => index;
        public string PresetName => name;

        public SpellValue SpellID2
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(spell));
            set => _fileEditor.SetByte(spell, (byte)value.Value);
        }
        public int Weapon0
        {
            get => _fileEditor.GetByte(weaponLv0);
            set => _fileEditor.SetByte(weaponLv0, (byte)value);
        }
        public int Weapon1
        {
            get => _fileEditor.GetByte(weaponLv1);
            set => _fileEditor.SetByte(weaponLv1, (byte)value);
        }
        public int Weapon2
        {
            get => _fileEditor.GetByte(weaponLv2);
            set => _fileEditor.SetByte(weaponLv2, (byte)value);
        }
        public int Weapon3
        {
            get => _fileEditor.GetByte(weaponLv3);
            set => _fileEditor.SetByte(weaponLv3, (byte)value);
        }

        public int PresetAddress => (address);
    }
}
