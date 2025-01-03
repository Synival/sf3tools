using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct {
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

        public Slot(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x34) {
            enemyID      = Address;      // 2 bytes  
            x            = Address +  2; // 2 bytes
            y            = Address +  4; // 2 bytes
            itemOverride = Address +  6; // 2 bytes
            unknown1     = Address +  8; // drop disabled
            unknown2     = Address +  9; // probably droprate override
            joinID       = Address + 10; // 2 bytes
            characterPlus0x0B = Address + 12; // character that shows up when enemy id is 5b
            unknown4     = Address + 13;
            unknown5     = Address + 14;
            unknown6     = Address + 15;
            facingIsBoss = Address + 16;
            controlType  = Address + 17;
            unknown8     = Address + 18;
            unknown9     = Address + 19;
            unknown10    = Address + 20;
            unknown11    = Address + 21;
            unknown12    = Address + 22;
            unknown13    = Address + 23;
            unknown14    = Address + 24;
            unknown15    = Address + 25;
            unknown16    = Address + 26;
            unknown17    = Address + 27;
            unknown18    = Address + 28;
            unknown19    = Address + 29;
            unknown20    = Address + 30;
            unknown21    = Address + 31; // turn not skipped?
            unknown22    = Address + 32;
            unknown23    = Address + 33;
            unknown24    = Address + 34;
            unknown25    = Address + 35; // aitag1?
            unknown26    = Address + 36; // aitype1?
            unknown27    = Address + 37; // aiaggression 1?
            unknown28    = Address + 38; // aitag2?
            unknown29    = Address + 39; // aitype4?
            unknown30    = Address + 40; // aiaggression 2?
            unknown31    = Address + 41; // aitag3?
            unknown32    = Address + 42; // aitype4?
            unknown33    = Address + 43; // aiaggression 3?
            unknown34    = Address + 44; // aitag4?
            unknown35    = Address + 45; // aitype4?
            unknown36    = Address + 46; // aiaggression 4?
            unknown37    = Address + 47;
            unknown38    = Address + 48;
            unknown39    = Address + 49;
            unknown40    = Address + 50; // 2 bytes
        }

        [BulkCopy]
        public int Unknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int Unknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.MonsterForSlot)]
        public int EnemyID {
            get => Data.GetWord(enemyID);
            set => Data.SetWord(enemyID, value);
        }

        [BulkCopy]
        public int EnemyX {
            get => Data.GetWord(x);
            set => Data.SetWord(x, value);
        }

        [BulkCopy]
        public int EnemyY {
            get => Data.GetWord(y);
            set => Data.SetWord(y, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemOverride {
            get => Data.GetWord(itemOverride);
            set => Data.SetWord(itemOverride, value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.CharacterPlus, nameof(EnemyID))]
        public int CharacterPlus0x0B {
            get => Data.GetByte(characterPlus0x0B);
            set => Data.SetByte(characterPlus0x0B, (byte) value);
        }

        [BulkCopy]
        public int Unknown4 {
            get => Data.GetByte(unknown4);
            set => Data.SetByte(unknown4, (byte) value);
        }

        [BulkCopy]
        public int Unknown5 {
            get => Data.GetByte(unknown5);
            set => Data.SetByte(unknown5, (byte) value);
        }

        [BulkCopy]
        public int JoinID {
            get => Data.GetWord(joinID);
            set => Data.SetWord(joinID, value);
        }

        [BulkCopy]
        public int Unknown6 {
            get => Data.GetByte(unknown6);
            set => Data.SetByte(unknown6, (byte) value);
        }

        public bool IsBoss {
            get => Data.GetBit(facingIsBoss, 5);
            set => Data.SetBit(facingIsBoss, 5, value);
        }

        [BulkCopy]
        public int FacingIsBoss {
            get => Data.GetByte(facingIsBoss);
            set => Data.SetByte(facingIsBoss, (byte) value);
        }

        [BulkCopy]
        public int Unknown8 {
            get => Data.GetByte(unknown8);
            set => Data.SetByte(unknown8, (byte) value);
        }

        [BulkCopy]
        public int Unknown9 {
            get => Data.GetByte(unknown9);
            set => Data.SetByte(unknown9, (byte) value);
        }

        [BulkCopy]
        public int ControlType {
            get => Data.GetByte(controlType);
            set => Data.SetByte(controlType, (byte) value);
        }

        [BulkCopy]
        public int Unknown10 {
            get => Data.GetByte(unknown10);
            set => Data.SetByte(unknown10, (byte) value);
        }

        [BulkCopy]
        public int Unknown11 {
            get => Data.GetByte(unknown11);
            set => Data.SetByte(unknown11, (byte) value);
        }

        [BulkCopy]
        public int Unknown12 {
            get => Data.GetByte(unknown12);
            set => Data.SetByte(unknown12, (byte) value);
        }

        [BulkCopy]
        public int Unknown13 {
            get => Data.GetByte(unknown13);
            set => Data.SetByte(unknown13, (byte) value);
        }

        [BulkCopy]
        public int Unknown14 {
            get => Data.GetByte(unknown14);
            set => Data.SetByte(unknown14, (byte) value);
        }

        [BulkCopy]
        public int Unknown15 {
            get => Data.GetByte(unknown15);
            set => Data.SetByte(unknown15, (byte) value);
        }

        [BulkCopy]
        public int Unknown16 {
            get => Data.GetByte(unknown16);
            set => Data.SetByte(unknown16, (byte) value);
        }

        [BulkCopy]
        public int Unknown17 {
            get => Data.GetByte(unknown17);
            set => Data.SetByte(unknown17, (byte) value);
        }

        [BulkCopy]
        public int Unknown18 {
            get => Data.GetByte(unknown18);
            set => Data.SetByte(unknown18, (byte) value);
        }

        [BulkCopy]
        public int Unknown19 {
            get => Data.GetByte(unknown19);
            set => Data.SetByte(unknown19, (byte) value);
        }

        [BulkCopy]
        public int Unknown20 {
            get => Data.GetByte(unknown20);
            set => Data.SetByte(unknown20, (byte) value);
        }

        [BulkCopy]
        public int Unknown21 {
            get => Data.GetByte(unknown21);
            set => Data.SetByte(unknown21, (byte) value);
        }

        [BulkCopy]
        public int Unknown22 {
            get => Data.GetByte(unknown22);
            set => Data.SetByte(unknown22, (byte) value);
        }

        [BulkCopy]
        public int Unknown23 {
            get => Data.GetByte(unknown23);
            set => Data.SetByte(unknown23, (byte) value);
        }

        [BulkCopy]
        public int Unknown24 {
            get => Data.GetByte(unknown24);
            set => Data.SetByte(unknown24, (byte) value);
        }

        [BulkCopy]
        public int Unknown25 {
            get => Data.GetByte(unknown25);
            set => Data.SetByte(unknown25, (byte) value);
        }

        [BulkCopy]
        public int Unknown26 {
            get => Data.GetByte(unknown26);
            set => Data.SetByte(unknown26, (byte) value);
        }

        [BulkCopy]
        public int Unknown27 {
            get => Data.GetByte(unknown27);
            set => Data.SetByte(unknown27, (byte) value);
        }

        [BulkCopy]
        public int Unknown28 {
            get => Data.GetByte(unknown28);
            set => Data.SetByte(unknown28, (byte) value);
        }

        [BulkCopy]
        public int Unknown29 {
            get => Data.GetByte(unknown29);
            set => Data.SetByte(unknown29, (byte) value);
        }

        [BulkCopy]
        public int Unknown30 {
            get => Data.GetByte(unknown30);
            set => Data.SetByte(unknown30, (byte) value);
        }

        [BulkCopy]
        public int Unknown31 {
            get => Data.GetByte(unknown31);
            set => Data.SetByte(unknown31, (byte) value);
        }

        [BulkCopy]
        public int Unknown32 {
            get => Data.GetByte(unknown32);
            set => Data.SetByte(unknown32, (byte) value);
        }

        [BulkCopy]
        public int Unknown33 {
            get => Data.GetByte(unknown33);
            set => Data.SetByte(unknown33, (byte) value);
        }

        [BulkCopy]
        public int Unknown34 {
            get => Data.GetByte(unknown34);
            set => Data.SetByte(unknown34, (byte) value);
        }

        [BulkCopy]
        public int Unknown35 {
            get => Data.GetByte(unknown35);
            set => Data.SetByte(unknown35, (byte) value);
        }

        [BulkCopy]
        public int Unknown36 {
            get => Data.GetByte(unknown36);
            set => Data.SetByte(unknown36, (byte) value);
        }

        [BulkCopy]
        public int Unknown37 {
            get => Data.GetByte(unknown37);
            set => Data.SetByte(unknown37, (byte) value);
        }

        [BulkCopy]
        public int Unknown38 {
            get => Data.GetByte(unknown38);
            set => Data.SetByte(unknown38, (byte) value);
        }

        [BulkCopy]
        public int Unknown39 {
            get => Data.GetByte(unknown39);
            set => Data.SetByte(unknown39, (byte) value);
        }

        [BulkCopy]
        public int Unknown40 {
            get => Data.GetWord(unknown40);
            set => Data.SetWord(unknown40, value);
        }
    }
}
