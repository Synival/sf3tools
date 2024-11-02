using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X1 {
    public class Slot : IModel {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int enemyID;
        private readonly int x;
        private readonly int y;
        private readonly int itemOverride;
        private readonly int characterPlus0x0B;
        private readonly int unknown4;
        private readonly int joinID;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int facingIsBoss;
        private readonly int unknown8;
        private readonly int controlType;
        private readonly int unknown9;
        private readonly int unknown10;
        private readonly int unknown11;
        private readonly int unknown12;
        private readonly int unknown13;
        private readonly int unknown14;
        private readonly int unknown15;
        private readonly int unknown16;
        private readonly int unknown17;
        private readonly int unknown18;
        private readonly int unknown19;
        private readonly int unknown20;
        private readonly int unknown21;
        private readonly int unknown22;
        private readonly int unknown23;
        private readonly int unknown24;
        private readonly int unknown25;
        private readonly int unknown26;
        private readonly int unknown27;
        private readonly int unknown28;
        private readonly int unknown29;
        private readonly int unknown30;
        private readonly int unknown31;
        private readonly int unknown32;
        private readonly int unknown33;
        private readonly int unknown34;
        private readonly int unknown35;
        private readonly int unknown36;
        private readonly int unknown37;
        private readonly int unknown38;
        private readonly int unknown39;
        private readonly int unknown40;

        public Slot(IX1_FileEditor editor, int id, string name) {
            Editor = editor;
            Name   = name;
            ID     = id;
            Size   = 0x34;

            int offset = 0;
            int sub;

            if (editor.IsBTL99) {
                offset = 0x00000018; //BTL99 initial pointer
                sub = 0x06060000;
                offset = Editor.GetDouble(offset);
                offset -= sub; //first pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //second pointer
                offset = Editor.GetDouble(offset);
                offset -= sub; //third pointer

                offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
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

                    offset += 10;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            //int start = 0x354c + (id * 24);

            var start = offset + (id * 0x34);
            enemyID = start; //2 bytes  
            x = start + 2; //2 byte
            y = start + 4; //2 byte
            itemOverride = start + 6; //2 byte
            unknown1 = start + 8; //drop disabled
            unknown2 = start + 9; //probably droprate override
            joinID = start + 10; //2 byte
            characterPlus0x0B = start + 12; //character that shows up when enemy id is 5b
            unknown4 = start + 13;
            unknown5 = start + 14;
            unknown6 = start + 15;
            facingIsBoss = start + 16;
            controlType = start + 17;
            unknown8 = start + 18;
            unknown9 = start + 19;
            unknown10 = start + 20;
            unknown11 = start + 21;
            unknown12 = start + 22;
            unknown13 = start + 23;
            unknown14 = start + 24;
            unknown15 = start + 25;
            unknown16 = start + 26;
            unknown17 = start + 27;
            unknown18 = start + 28;
            unknown19 = start + 29;
            unknown20 = start + 30;
            unknown21 = start + 31; //turn not skipped?
            unknown22 = start + 32;
            unknown23 = start + 33;
            unknown24 = start + 34;
            unknown25 = start + 35;//aitag1?
            unknown26 = start + 36; //aitype1?
            unknown27 = start + 37;//aiaggression 1?
            unknown28 = start + 38;//aitag2?
            unknown29 = start + 39;//aitype4?
            unknown30 = start + 40;//aiaggression 2?
            unknown31 = start + 41; //aitag3?
            unknown32 = start + 42;//aitype4?
            unknown33 = start + 43;//aiaggression 3?
            unknown34 = start + 44; //aitag4?
            unknown35 = start + 45; //aitype4?
            unknown36 = start + 46; //aiaggression 4?
            unknown37 = start + 47;
            unknown38 = start + 48;
            unknown39 = start + 49;
            unknown40 = start + 50; //2 bytes
            //unknown42 = start + 52;
            Address = offset + (id * 0x34);
            //address = 0x0354c + (id * 0x18);
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Unknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int Unknown2 {
            get => Editor.GetByte(unknown2);
            set => Editor.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.MonsterForSlot)]
        public int EnemyID {
            get => Editor.GetWord(enemyID);
            set => Editor.SetWord(enemyID, value);
        }

        [BulkCopy]
        public int EnemyX {
            get => Editor.GetWord(x);
            set => Editor.SetWord(x, value);
        }

        [BulkCopy]
        public int EnemyY {
            get => Editor.GetWord(y);
            set => Editor.SetWord(y, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemOverride {
            get => Editor.GetWord(itemOverride);
            set => Editor.SetWord(itemOverride, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Character)]
        public int CharacterPlus0x0B {
            get => Editor.GetByte(characterPlus0x0B);
            set => Editor.SetByte(characterPlus0x0B, (byte) value);
        }

        [BulkCopy]
        public int Unknown4 {
            get => Editor.GetByte(unknown4);
            set => Editor.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int Unknown5 {
            get => Editor.GetByte(unknown5);
            set => Editor.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int JoinID {
            get => Editor.GetWord(joinID);
            set => Editor.SetWord(joinID, value);
        }

        [BulkCopy]
        public int Unknown6 {
            get => Editor.GetByte(unknown6);
            set => Editor.SetByte(unknown6, (byte) value);
        }

        public bool IsBoss {
            get => Editor.GetBit(facingIsBoss, 5);
            set => Editor.SetBit(facingIsBoss, 5, value);
        }

        [BulkCopy]
        public int FacingIsBoss {
            get => Editor.GetByte(facingIsBoss);
            set => Editor.SetByte(facingIsBoss, (byte) value);
        }

        [BulkCopy]
        public int Unknown8 {
            get => Editor.GetByte(unknown8);
            set => Editor.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int Unknown9 {
            get => Editor.GetByte(unknown9);
            set => Editor.SetByte(unknown9, (byte) value);
        }

        [BulkCopy]
        public int ControlType {
            get => Editor.GetByte(controlType);
            set => Editor.SetByte(controlType, (byte) value);
        }

        [BulkCopy]
        public int Unknown10 {
            get => Editor.GetByte(unknown10);
            set => Editor.SetByte(unknown10, (byte) value);
        }

        [BulkCopy]
        public int Unknown11 {
            get => Editor.GetByte(unknown11);
            set => Editor.SetByte(unknown11, (byte) value);
        }

        [BulkCopy]
        public int Unknown12 {
            get => Editor.GetByte(unknown12);
            set => Editor.SetByte(unknown12, (byte) value);
        }

        [BulkCopy]
        public int Unknown13 {
            get => Editor.GetByte(unknown13);
            set => Editor.SetByte(unknown13, (byte) value);
        }

        [BulkCopy]
        public int Unknown14 {
            get => Editor.GetByte(unknown14);
            set => Editor.SetByte(unknown14, (byte) value);
        }

        [BulkCopy]
        public int Unknown15 {
            get => Editor.GetByte(unknown15);
            set => Editor.SetByte(unknown15, (byte) value);
        }

        [BulkCopy]
        public int Unknown16 {
            get => Editor.GetByte(unknown16);
            set => Editor.SetByte(unknown16, (byte) value);
        }

        [BulkCopy]
        public int Unknown17 {
            get => Editor.GetByte(unknown17);
            set => Editor.SetByte(unknown17, (byte) value);
        }

        [BulkCopy]
        public int Unknown18 {
            get => Editor.GetByte(unknown18);
            set => Editor.SetByte(unknown18, (byte) value);
        }

        [BulkCopy]
        public int Unknown19 {
            get => Editor.GetByte(unknown19);
            set => Editor.SetByte(unknown19, (byte) value);
        }

        [BulkCopy]
        public int Unknown20 {
            get => Editor.GetByte(unknown20);
            set => Editor.SetByte(unknown20, (byte) value);
        }

        [BulkCopy]
        public int Unknown21 {
            get => Editor.GetByte(unknown21);
            set => Editor.SetByte(unknown21, (byte) value);
        }

        [BulkCopy]
        public int Unknown22 {
            get => Editor.GetByte(unknown22);
            set => Editor.SetByte(unknown22, (byte) value);
        }

        [BulkCopy]
        public int Unknown23 {
            get => Editor.GetByte(unknown23);
            set => Editor.SetByte(unknown23, (byte) value);
        }

        [BulkCopy]
        public int Unknown24 {
            get => Editor.GetByte(unknown24);
            set => Editor.SetByte(unknown24, (byte) value);
        }

        [BulkCopy]
        public int Unknown25 {
            get => Editor.GetByte(unknown25);
            set => Editor.SetByte(unknown25, (byte) value);
        }

        [BulkCopy]
        public int Unknown26 {
            get => Editor.GetByte(unknown26);
            set => Editor.SetByte(unknown26, (byte) value);
        }

        [BulkCopy]
        public int Unknown27 {
            get => Editor.GetByte(unknown27);
            set => Editor.SetByte(unknown27, (byte) value);
        }

        [BulkCopy]
        public int Unknown28 {
            get => Editor.GetByte(unknown28);
            set => Editor.SetByte(unknown28, (byte) value);
        }

        [BulkCopy]
        public int Unknown29 {
            get => Editor.GetByte(unknown29);
            set => Editor.SetByte(unknown29, (byte) value);
        }

        [BulkCopy]
        public int Unknown30 {
            get => Editor.GetByte(unknown30);
            set => Editor.SetByte(unknown30, (byte) value);
        }

        [BulkCopy]
        public int Unknown31 {
            get => Editor.GetByte(unknown31);
            set => Editor.SetByte(unknown31, (byte) value);
        }

        [BulkCopy]
        public int Unknown32 {
            get => Editor.GetByte(unknown32);
            set => Editor.SetByte(unknown32, (byte) value);
        }

        [BulkCopy]
        public int Unknown33 {
            get => Editor.GetByte(unknown33);
            set => Editor.SetByte(unknown33, (byte) value);
        }

        [BulkCopy]
        public int Unknown34 {
            get => Editor.GetByte(unknown34);
            set => Editor.SetByte(unknown34, (byte) value);
        }

        [BulkCopy]
        public int Unknown35 {
            get => Editor.GetByte(unknown35);
            set => Editor.SetByte(unknown35, (byte) value);
        }

        [BulkCopy]
        public int Unknown36 {
            get => Editor.GetByte(unknown36);
            set => Editor.SetByte(unknown36, (byte) value);
        }

        [BulkCopy]
        public int Unknown37 {
            get => Editor.GetByte(unknown37);
            set => Editor.SetByte(unknown37, (byte) value);
        }

        [BulkCopy]
        public int Unknown38 {
            get => Editor.GetByte(unknown38);
            set => Editor.SetByte(unknown38, (byte) value);
        }

        [BulkCopy]
        public int Unknown39 {
            get => Editor.GetByte(unknown39);
            set => Editor.SetByte(unknown39, (byte) value);
        }

        [BulkCopy]
        public int Unknown40 {
            get => Editor.GetWord(unknown40);
            set => Editor.SetWord(unknown40, value);
        }
    }
}
