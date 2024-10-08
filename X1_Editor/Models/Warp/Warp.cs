using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Types;
using SF3.X1_Editor.Models.UnknownAI;
using System;

namespace SF3.X1_Editor.Models.Warps
{
    public class Warp
    {
        private IFileEditor _fileEditor;

        private int unknown1;
        private int unknown2;
        private int type;
        private int map;

        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int sub;

        private int index;
        private string name;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public Warp(IFileEditor fileEditor, ScenarioType scenario, int id, string text)
        {
            _fileEditor = fileEditor;
            Scenario = scenario;

            //no scn1 for this

            if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000018; //scn2 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000018; //scn3 initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000018; //pd initial pointer
                sub = 0x0605e000;
                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x04);
            unknown1 = start;
            unknown2 = start + 1;
            type = start + 2;
            map = start + 3;

            //unknown42 = start + 52;
            address = offset + (id * 0x04);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario { get; }
        public int WarpID => index;
        public string WarpName => name;

        public int WarpUnknown1
        {
            get => FileEditor.GetByte(unknown1);
            set => FileEditor.SetByte(unknown1, (byte)value);
        }

        public int WarpUnknown2
        {
            get => FileEditor.GetByte(unknown2);
            set => FileEditor.SetByte(unknown2, (byte)value);
        }

        public int WarpType
        {
            get => FileEditor.GetByte(type);
            set => FileEditor.SetByte(type, (byte)value);
        }

        public int WarpMap
        {
            get => FileEditor.GetByte(map);
            set => FileEditor.SetByte(map, (byte)value);
        }

        public int WarpAddress => (address);
    }
}
