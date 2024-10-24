﻿using SF3.Types;
using SF3.FileEditors;

namespace SF3.Models.X1.AI
{
    public class AI
    {
        private IX1_FileEditor _fileEditor;

        private int targetX;
        private int targetY;

        private int address;
        private int offset;
        private int sub;

        private int index;
        private string name;

        public AI(IX1_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            if (_fileEditor.IsBTL99)
            {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //second pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //third pointer

                offset = offset + 10;
                offset = offset + 0xe9a;
                offset = offset + 0x126;
            }
            else if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);

                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xe9a;
                    offset = offset + 0x126;
                }
                else
                {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xe9a; //size of the enemy spawn table
                    offset = offset + 0x126; //size of something else
                    //we're now at our offset after adding these
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;

                    offset = offset + 0xa8a;//size of the enemy spawn table
                    offset = offset + 0x126;
                }
                else
                {
                    _fileEditor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;

                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer

                offset = _fileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                }
                else
                {
                    _fileEditor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //first pointer
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub + _fileEditor.MapOffset; //second pointer
                offset = _fileEditor.GetDouble(offset);
                if (offset != 0)
                {
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                }
                else
                {
                    _fileEditor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //first pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub + _fileEditor.MapOffset; //second pointer
                    offset = _fileEditor.GetDouble(offset);
                    offset = offset - sub; //third pointer

                    offset = offset + 10;
                    offset = offset + 0xa8a;
                    offset = offset + 0x126;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            targetX = start; //2 bytes
            targetY = start + 2; //2 bytes
            address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int AIID => index;
        public string AIName => name;

        public int TargetX
        {
            get => _fileEditor.GetWord(targetX);
            set => _fileEditor.SetWord(targetX, value);
        }
        public int TargetY
        {
            get => _fileEditor.GetWord(targetY);
            set => _fileEditor.SetWord(targetY, value);
        }
        public int AIAddress => (address);
    }
}
