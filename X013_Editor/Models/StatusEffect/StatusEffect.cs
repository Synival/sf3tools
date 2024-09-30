﻿using BrightIdeasSoftware;
using System;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.StatusEffects
{
    public class StatusEffect
    {
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

        /*public int NPCTableAddress1
        {
            get => FileEditor.getDouble(npcOffset);
            set => FileEditor.setDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.getDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.getDouble(NPCTableAddress2 - 0x0605F000);*/

        public StatusEffect(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00007408; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00007314; //scn2
            }
            else if (Globals.scenario == 3)
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

        public int StatusEffectID
        {
            get
            {
                return index;
            }
        }
        public string StatusEffectName
        {
            get
            {
                return name;
            }
        }

        public int StatusLuck0
        {
            get
            {
                return FileEditor.getByte(luck0);
            }
            set
            {
                FileEditor.setByte(luck0, (byte)value);
            }
        }

        public int StatusLuck1
        {
            get
            {
                return FileEditor.getByte(luck1);
            }
            set
            {
                FileEditor.setByte(luck1, (byte)value);
            }
        }

        public int StatusLuck2
        {
            get
            {
                return FileEditor.getByte(luck2);
            }
            set
            {
                FileEditor.setByte(luck2, (byte)value);
            }
        }

        public int StatusLuck3
        {
            get
            {
                return FileEditor.getByte(luck3);
            }
            set
            {
                FileEditor.setByte(luck3, (byte)value);
            }
        }

        public int StatusLuck4
        {
            get
            {
                return FileEditor.getByte(luck4);
            }
            set
            {
                FileEditor.setByte(luck4, (byte)value);
            }
        }

        public int StatusLuck5
        {
            get
            {
                return FileEditor.getByte(luck5);
            }
            set
            {
                FileEditor.setByte(luck5, (byte)value);
            }
        }

        public int StatusLuck6
        {
            get
            {
                return FileEditor.getByte(luck6);
            }
            set
            {
                FileEditor.setByte(luck6, (byte)value);
            }
        }

        public int StatusLuck7
        {
            get
            {
                return FileEditor.getByte(luck7);
            }
            set
            {
                FileEditor.setByte(luck7, (byte)value);
            }
        }

        public int StatusLuck8
        {
            get
            {
                return FileEditor.getByte(luck8);
            }
            set
            {
                FileEditor.setByte(luck8, (byte)value);
            }
        }

        public int StatusLuck9
        {
            get
            {
                return FileEditor.getByte(luck9);
            }
            set
            {
                FileEditor.setByte(luck9, (byte)value);
            }
        }

        public int StatusEffectAddress
        {
            get
            {
                return (address);
            }
        }

    }
}
