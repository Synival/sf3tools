using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Slot : Struct {
        private readonly int dropDisableAddr;
        private readonly int unknown2;
        private readonly int enemyID;
        private readonly int x;
        private readonly int y;
        private readonly int itemOverride;
        private readonly int characterPlus0x0B;
        private readonly int unknown4;
        private readonly int eventCallAddr;
        private readonly int unknown5;
        private readonly int unknown6;
        private readonly int facingIsBoss;
        private readonly int unknown8;
        private readonly int controlType;
        private readonly int unknown9;
        private readonly int unknown10;
        private readonly int unknown11;
        private readonly int unknown12;
        private readonly int strictAIAddr;
        private readonly int unknown14;
        private readonly int unknown15;
        private readonly int unknown16;
        private readonly int unknown17;
        private readonly int unknown18;
        private readonly int unknown19;
        private readonly int unknown20;
        private readonly int turnSkipAddr;
        private readonly int unknown22;
        private readonly int unknown23;
        private readonly int unknown24;
        private readonly int aiTag1Addr;
        private readonly int aiType1Addr;
        private readonly int aiAggr1Addr;
        private readonly int aiTag2Addr;
        private readonly int aiType2Addr;
        private readonly int aiAggr2Addr;
        private readonly int aiTag3Addr;
        private readonly int aiType3Addr;
        private readonly int aiAggr3Addr;
        private readonly int aiTag4Addr;
        private readonly int aiType4Addr;
        private readonly int aiAggr4Addr;
        private readonly int unknown37;
        private readonly int unknown38;
        private readonly int unknown39;
        private readonly int flagsAddr;

        public Slot(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x34) {
            enemyID      = Address;      // 2 bytes  
            x            = Address +  2; // 2 bytes
            y            = Address +  4; // 2 bytes
            itemOverride = Address +  6; // 2 bytes
            dropDisableAddr = Address +  8; // drop disabled
            unknown2     = Address +  9; // probably droprate override
            eventCallAddr = Address + 10; // 2 bytes
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
            strictAIAddr = Address + 23;
            unknown14    = Address + 24;
            unknown15    = Address + 25;
            unknown16    = Address + 26;
            unknown17    = Address + 27;
            unknown18    = Address + 28;
            unknown19    = Address + 29;
            unknown20    = Address + 30;
            turnSkipAddr = Address + 31; // turn not skipped?
            unknown22    = Address + 32;
            unknown23    = Address + 33;
            unknown24    = Address + 34;
            aiTag1Addr   = Address + 35; // aitag1?
            aiType1Addr  = Address + 36; // aitype1?
            aiAggr1Addr  = Address + 37; // aiaggression 1?
            aiTag2Addr   = Address + 38; // aitag2?
            aiType2Addr  = Address + 39; // aitype4?
            aiAggr2Addr  = Address + 40; // aiaggression 2?
            aiTag3Addr   = Address + 41; // aitag3?
            aiType3Addr  = Address + 42; // aitype4?
            aiAggr3Addr  = Address + 43; // aiaggression 3?
            aiTag4Addr   = Address + 44; // aitag4?
            aiType4Addr  = Address + 45; // aitype4?
            aiAggr4Addr  = Address + 46; // aiaggression 4?
            unknown37    = Address + 47;
            unknown38    = Address + 48;
            unknown39    = Address + 49;
            flagsAddr    = Address + 50; // 2 bytes
        }

        [BulkCopy]
        public int DropDisable {
            get => Data.GetByte(dropDisableAddr);
            set => Data.SetByte(dropDisableAddr, (byte) value);
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
        public int EventCall {
            get => Data.GetWord(eventCallAddr);
            set => Data.SetWord(eventCallAddr, value);
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
        public int StrictAI {
            get => Data.GetByte(strictAIAddr);
            set => Data.SetByte(strictAIAddr, (byte) value);
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
        public int TurnSkip {
            get => Data.GetByte(turnSkipAddr);
            set => Data.SetByte(turnSkipAddr, (byte) value);
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
        public int AITag1 {
            get => Data.GetByte(aiTag1Addr);
            set => Data.SetByte(aiTag1Addr, (byte) value);
        }

        [BulkCopy]
        public int AIType1 {
            get => Data.GetByte(aiType1Addr);
            set => Data.SetByte(aiType1Addr, (byte) value);
        }

        [BulkCopy]
        public int AIAggr1 {
            get => Data.GetByte(aiAggr1Addr);
            set => Data.SetByte(aiAggr1Addr, (byte) value);
        }

        [BulkCopy]
        public int AITag2 {
            get => Data.GetByte(aiTag2Addr);
            set => Data.SetByte(aiTag2Addr, (byte) value);
        }

        [BulkCopy]
        public int AIType2 {
            get => Data.GetByte(aiType2Addr);
            set => Data.SetByte(aiType2Addr, (byte) value);
        }

        [BulkCopy]
        public int AIAggr2 {
            get => Data.GetByte(aiAggr2Addr);
            set => Data.SetByte(aiAggr2Addr, (byte) value);
        }

        [BulkCopy]
        public int AITag3 {
            get => Data.GetByte(aiTag3Addr);
            set => Data.SetByte(aiTag3Addr, (byte) value);
        }

        [BulkCopy]
        public int AIType3 {
            get => Data.GetByte(aiType3Addr);
            set => Data.SetByte(aiType3Addr, (byte) value);
        }

        [BulkCopy]
        public int AIAggr3 {
            get => Data.GetByte(aiAggr3Addr);
            set => Data.SetByte(aiAggr3Addr, (byte) value);
        }

        [BulkCopy]
        public int AITag4 {
            get => Data.GetByte(aiTag4Addr);
            set => Data.SetByte(aiTag4Addr, (byte) value);
        }

        [BulkCopy]
        public int AIType4 {
            get => Data.GetByte(aiType4Addr);
            set => Data.SetByte(aiType4Addr, (byte) value);
        }

        [BulkCopy]
        public int AIAggr4 {
            get => Data.GetByte(aiAggr4Addr);
            set => Data.SetByte(aiAggr4Addr, (byte) value);
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
        public int Flags {
            get => Data.GetWord(flagsAddr);
            set => Data.SetWord(flagsAddr, value);
        }
    }
}
