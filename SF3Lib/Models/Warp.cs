using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class Warp {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        public Warp(ISF3FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x04;

            // X002 editor has Scenario1 WarpTable, and provides the address itself.
            // TODO: the X1 editor can do this soon too!!
            if (editor.Scenario != ScenarioType.Scenario1) {
                int offset;
                int sub;

                if (editor.Scenario == ScenarioType.Scenario2) {
                    offset = 0x00000018; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub;
                }
                else if (editor.Scenario == ScenarioType.Scenario3) {
                    offset = 0x00000018; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub;
                }
                else if (editor.Scenario == ScenarioType.PremiumDisk) {
                    offset = 0x00000018; //pd initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub;
                }
                else
                    throw new ArgumentException(nameof(editor) + "." + nameof(editor.Scenario));

                Address = offset + (id * 0x04);
            }

            unknown1 = Address;
            unknown2 = Address + 1;
            type     = Address + 2;
            map      = Address + 3;
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int WarpUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int WarpUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int WarpType {
            get => Editor.GetByte(type);
            set => Editor.SetByte(type, (byte) value);
        }

        [BulkCopy]
        public int WarpMap {
            get => Editor.GetByte(map);
            set => Editor.SetByte(map, (byte) value);
        }
    }
}
