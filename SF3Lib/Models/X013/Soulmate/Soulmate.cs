﻿using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X013.Soulmate
{
    public class Soulmate
    {
        private IX013_FileEditor _fileEditor;

        private int chance;
        private int address;
        private int offset;

        private int index;
        private string name;
        private int checkVersion2;

        public Soulmate(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00007530; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00007484; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x0000736c; //scn3
            }
            else
                offset = 0x00007248; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 1);
            chance = start; //2 bytes
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SoulmateID => index;
        public string SoulmateName => name;

        public int Chance
        {
            get => _fileEditor.GetByte(chance);
            set => _fileEditor.SetByte(chance, (byte)value);
        }

        public int SoulmateAddress => (address);
    }
}
