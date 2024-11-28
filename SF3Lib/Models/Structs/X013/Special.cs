using CommonLib.Attributes;
using SF3.Models.Structs;
using SF3.RawEditors;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class Special : Struct {
        private readonly int unknown1;
        private readonly int damageCalculation;
        private readonly int extraPow;
        private readonly int pow;

        public Special(IRawEditor editor, int id, string name, int address)
        : base(editor, id, name, address, 0x04) {
            unknown1          = Address;     // 1 byte
            damageCalculation = Address + 1; // 1 byte
            extraPow          = Address + 2; // 1 byte
            pow               = Address + 3; // 1 byte
        }

        [BulkCopy]
        public int Unknown1 {
            get => Editor.GetByte(unknown1);
            set => Editor.SetByte(unknown1, (byte) value);
        }

        [BulkCopy]
        public int DamageCalc {
            get => Editor.GetByte(damageCalculation);
            set => Editor.SetByte(damageCalculation, (byte) value);
        }

        [BulkCopy]
        [NameGetter(NamedValueType.SpecialElement, nameof(DamageCalc))]
        public int ExtraPow {
            get => Editor.GetByte(extraPow);
            set => Editor.SetByte(extraPow, (byte) value);
        }

        [BulkCopy]
        public int Pow {
            get => Editor.GetByte(pow);
            set => Editor.SetByte(pow, (byte) value);
        }

        private int lowerRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Editor.GetByte(extraPow);
            ;
            r1 -= Editor.GetByte(damageCalculation);
            ;
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
            r1 += Editor.GetByte(damageCalculation);

            return r1;
        }

        public int ranResult0 => DamageCalc == 100 ? Pow : lowerRngFunc(0);
        public int ranResult1 => DamageCalc == 100 ? Pow : lowerRngFunc(1);
        public int ranResult2 => DamageCalc == 100 ? Pow : lowerRngFunc(2);
        public int ranResult3 => DamageCalc == 100 ? Pow : lowerRngFunc(3);
        public int ranResult4 => DamageCalc == 100 ? Pow : lowerRngFunc(4);
        public int ranResult5 => DamageCalc == 100 ? Pow : lowerRngFunc(5);
        public int ranResult6 => DamageCalc == 100 ? Pow : lowerRngFunc(6);
        public int ranResult7 => DamageCalc == 100 ? Pow : lowerRngFunc(7);

        private int upperRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Editor.GetByte(extraPow);
            r2 = Editor.GetByte(pow);
            r2 = Editor.GetByte(extraPow) - r2;
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
            r1 = Editor.GetByte(extraPow) + r1;

            return r1;
        }

        public int ranResult8 => DamageCalc == 100 ? Pow : upperRngFunc(8);
        public int ranResult9 => DamageCalc == 100 ? Pow : upperRngFunc(9);
        public int ranResult10 => DamageCalc == 100 ? Pow : upperRngFunc(10);
        public int ranResult11 => DamageCalc == 100 ? Pow : upperRngFunc(11);
        public int ranResult12 => DamageCalc == 100 ? Pow : upperRngFunc(12);
        public int ranResult13 => DamageCalc == 100 ? Pow : upperRngFunc(13);
        public int ranResult14 => DamageCalc == 100 ? Pow : upperRngFunc(14);
    }
}
