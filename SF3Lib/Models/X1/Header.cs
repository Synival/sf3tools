using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Header : IModel {
        private readonly int unknown1;
        private readonly int tableSize;
        private readonly int unknown2;
        private readonly int unknown3;
        private readonly int unknown4;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int unknown7;

        private readonly int unknown8;
        private readonly int unknown9;

        public Header(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x0A;

            Map = editor.MapOffset;

            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //third pointer
            }
            else if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer
                offset = Editor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000018; //scn1 initial pointer
                    sub = 0x0605f000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer
                }

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = Editor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = Editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = Editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    editor.MapLeader = MapLeaderType.Medion;
                    offset = 0x00000024; //scn2 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer
                }

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = Editor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = Editor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer

                offset = Editor.GetDouble(offset);

                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    editor.MapLeader = MapLeaderType.Julian;
                    offset = 0x00000024; //scn3 initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer
                }
            }
            else if (editor.Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset = offset - sub + editor.MapOffset; //second pointer
                offset = Editor.GetDouble(offset);
                if (offset != 0) {
                    offset -= sub; //third pointer
                }
                else {
                    editor.MapLeader = MapLeaderType.Synbios;
                    offset = 0x00000024; //pd initial pointer
                    sub = 0x0605e000;
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //first pointer
                    offset = Editor.GetDouble(offset);
                    offset = offset - sub + editor.MapOffset; //second pointer
                    offset = Editor.GetDouble(offset);
                    offset -= sub; //third pointer
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x0A);
            unknown1 = start; //1 bytes
            tableSize = start + 1; //1 byte
            unknown2 = start + 2; //1 byte
            unknown3 = start + 3; //1 byte
            unknown4 = start + 4; //1 byte
            unknown5 = start + 5;
            unknown6 = start + 6;
            unknown7 = start + 7;

            unknown8 = start + 8;
            unknown9 = start + 9;

            SizeAddress = offset + (id * 0x0A);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int SizeUnknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int TableSize {
            get => Editor.GetByte(tableSize);
            set => Editor.SetByte(tableSize, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown3 {
            get => Editor.GetByte(unknown3);
            set => Editor.SetByte(unknown3, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown4 {
            get => Editor.GetByte(unknown4);
            set => Editor.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown5 {
            get => Editor.GetByte(unknown5);
            set => Editor.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown6 {
            get => Editor.GetByte(unknown6);
            set => Editor.SetByte(unknown6, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown7 {
            get => Editor.GetByte(unknown7);
            set => Editor.SetByte(unknown7, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown8 {
            get => Editor.GetByte(unknown8);
            set => Editor.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int SizeUnknown9 {
            get => Editor.GetByte(unknown9);
            set => Editor.SetByte(unknown9, (byte) value);
        }

        public int Map { get; }

        public int SizeAddress { get; }
    }
}
