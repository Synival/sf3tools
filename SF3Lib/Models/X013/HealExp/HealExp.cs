﻿using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X013.HealExp
{
    public class HealExp
    {
        private IX013_FileEditor _fileEditor;

        private int healExp;
        private int address;
        private int offset;

        private int index;
        private string name;
        private int checkVersion2;

        public HealExp(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00004c8b; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x64;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00004ebf; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00004aed; //scn3
            }
            else
                offset = 0x00004b01; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 1);
            healExp = start; //1 byte
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int HealExpID => index;
        public string HealExpName => name;

        public int HealBonus
        {
            get => _fileEditor.GetByte(healExp);
            set => _fileEditor.SetByte(healExp, (byte)value);
        }

        public int HealExpAddress => (address);
    }
}
