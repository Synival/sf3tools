﻿using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X013.StatusEffects
{
    public class StatusEffect
    {
        private IX013_FileEditor _fileEditor;

        private int luck0;
        private int luck1;
        private int luck2;
        private int luck3;
        private int luck4;
        private int luck5;
        private int luck6;
        private int luck7;
        private int luck8;
        private int luck9;

        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int sub;

        private int index;
        private string name;
        private int checkVersion2;

        /*public int NPCTableAddress1
        {
            get => FileEditor.GetDouble(npcOffset);
            set => FileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public StatusEffect(IX013_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00007408; //scn1
                if (checkVersion2 == 0x0A) //original jp
                {
                    offset -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00007314; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x000071fc; //scn3
            }
            else
                offset = 0x000070d8; //pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x18);

            luck0 = start;
            luck1 = start + 2;
            luck2 = start + 4;
            luck3 = start + 6;
            luck4 = start + 8;
            luck5 = start + 10;
            luck6 = start + 12;
            luck7 = start + 14;
            luck8 = start + 16;
            luck9 = start + 18;

            //unknown42 = start + 52;
            address = offset + (id * 0x18);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int StatusEffectID => index;
        public string StatusEffectName => name;

        public int StatusLuck0
        {
            get => _fileEditor.GetByte(luck0);
            set => _fileEditor.SetByte(luck0, (byte)value);
        }

        public int StatusLuck1
        {
            get => _fileEditor.GetByte(luck1);
            set => _fileEditor.SetByte(luck1, (byte)value);
        }

        public int StatusLuck2
        {
            get => _fileEditor.GetByte(luck2);
            set => _fileEditor.SetByte(luck2, (byte)value);
        }

        public int StatusLuck3
        {
            get => _fileEditor.GetByte(luck3);
            set => _fileEditor.SetByte(luck3, (byte)value);
        }

        public int StatusLuck4
        {
            get => _fileEditor.GetByte(luck4);
            set => _fileEditor.SetByte(luck4, (byte)value);
        }

        public int StatusLuck5
        {
            get => _fileEditor.GetByte(luck5);
            set => _fileEditor.SetByte(luck5, (byte)value);
        }

        public int StatusLuck6
        {
            get => _fileEditor.GetByte(luck6);
            set => _fileEditor.SetByte(luck6, (byte)value);
        }

        public int StatusLuck7
        {
            get => _fileEditor.GetByte(luck7);
            set => _fileEditor.SetByte(luck7, (byte)value);
        }

        public int StatusLuck8
        {
            get => _fileEditor.GetByte(luck8);
            set => _fileEditor.SetByte(luck8, (byte)value);
        }

        public int StatusLuck9
        {
            get => _fileEditor.GetByte(luck9);
            set => _fileEditor.SetByte(luck9, (byte)value);
        }

        public int StatusEffectAddress => (address);
    }
}
