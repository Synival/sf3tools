using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class Special : Struct {
        private readonly int unknown1;
        private readonly int damageCalculation;
        private readonly int extraPow;
        private readonly int pow;

        public Special(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            unknown1          = Address;     // 1 byte
            damageCalculation = Address + 1; // 1 byte
            extraPow          = Address + 2; // 1 byte
            pow               = Address + 3; // 1 byte
        }

        [BulkCopy]
        public int Unknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int DamageCalc {
            get => Data.GetByte(damageCalculation);
            set => Data.SetByte(damageCalculation, (byte) value);
        }

        public NamedValueType? ExtraPowType
            => (DamageCalc == 100) ? NamedValueType.Element : (NamedValueType?) null;

        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(ExtraPowType))]
        public int ExtraPow {
            get => Data.GetByte(extraPow);
            set => Data.SetByte(extraPow, (byte) value);
        }

        [BulkCopy]
        public int Pow {
            get => Data.GetByte(pow);
            set => Data.SetByte(pow, (byte) value);
        }

        // Function used for low RNG damage rolls
        private int lowerRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Data.GetByte(extraPow);
            r1 -= Data.GetByte(damageCalculation);
            r3 = randomNumber;

            machh = r1 * r3;

            r2 = machh; //6d or da right now

            //r1 = 0x92492493;
            //machh = r1 * r2;
            //r1 = machh;

            r1 = r2 / 7;
            //simulates 92492493
            //Console.WriteLine("r1 = " + r1);
            //simulates this:
            //r1 = 0x92492493;
            //machh = r1 * r2;
            //r1 = machh;
            //r1 = r2 + r1;
            //r1 = r1 >> 1;
            //r1 = r1 >> 1;

            r2 <<= 2;
            r2 -= r2;
            r1 -= r2;
            r1 += Data.GetByte(damageCalculation);

            return r1;
        }

        // Function used for high RNG damage rolls
        private int upperRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Data.GetByte(extraPow);
            r2 = Data.GetByte(pow);
            r2 = Data.GetByte(extraPow) - r2;
            r3 = randomNumber;
            r1 = r3;
            r1 -= 7;

            machh = r1 * r2;

            r2 = machh;

            //r1 = 0x92492493;
            //machh = r1 * r2;
            //r1 = machh;
            //Console.WriteLine("r1 = " + r1);
            r1 = -(r2 / 7);

            r2 <<= 2;
            r2 -= r2;
            r1 -= r2;
            r1 = Data.GetByte(extraPow) + r1;

            return r1;
        }

        public int DamageRoll(int rng)
            => (DamageCalc == 100) ? Pow : ((rng < 8) ? lowerRngFunc(rng) : upperRngFunc(rng));

        public int DamageRoll0 => DamageRoll(0);
        public int DamageRoll1 => DamageRoll(1);
        public int DamageRoll2 => DamageRoll(2);
        public int DamageRoll3 => DamageRoll(3);
        public int DamageRoll4 => DamageRoll(4);
        public int DamageRoll5 => DamageRoll(5);
        public int DamageRoll6 => DamageRoll(6);
        public int DamageRoll7 => DamageRoll(7);
        public int DamageRoll8 => DamageRoll(8);
        public int DamageRoll9 => DamageRoll(9);
        public int DamageRoll10 => DamageRoll(10);
        public int DamageRoll11 => DamageRoll(11);
        public int DamageRoll12 => DamageRoll(12);
        public int DamageRoll13 => DamageRoll(13);
        public int DamageRoll14 => DamageRoll(14);
    }
}
