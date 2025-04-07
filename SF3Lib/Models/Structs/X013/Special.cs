using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class Special : Struct {
        private readonly int _typeAddr;
        private readonly int _lowPowAddr;
        private readonly int _midPowAddr;
        private readonly int _highPowAddr;

        public Special(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            _typeAddr    = Address + 0x00; // 1 byte
            _lowPowAddr  = Address + 0x01; // 1 byte
            _midPowAddr  = Address + 0x02; // 1 byte
            _highPowAddr = Address + 0x03; // 1 byte
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X2", minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.SpecialType)]
        public int Type {
            get => Data.GetByte(_typeAddr);
            set => Data.SetByte(_typeAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1)]
        [BulkCopy]
        public int LowPow {
            get => Data.GetByte(_lowPowAddr);
            set => Data.SetByte(_lowPowAddr, (byte) value);
        }

        public NamedValueType? MidPowType
            => (LowPow == 100) ? NamedValueType.Element : (NamedValueType?) null;

        [TableViewModelColumn(displayOrder: 2, minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.ConditionalType, nameof(MidPowType))]
        public int MidPow {
            get => Data.GetByte(_midPowAddr);
            set => Data.SetByte(_midPowAddr, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3)]
        [BulkCopy]
        public int MaxPow {
            get => Data.GetByte(_highPowAddr);
            set => Data.SetByte(_highPowAddr, (byte) value);
        }

        // Function used for low RNG damage rolls
        private int lowerRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Data.GetByte(_midPowAddr);
            r1 -= Data.GetByte(_lowPowAddr);
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
            r1 += Data.GetByte(_lowPowAddr);

            return r1;
        }

        // Function used for high RNG damage rolls
        private int upperRngFunc(int randomNumber) {
            int r1, r2, r3, machh;

            r1 = Data.GetByte(_midPowAddr);
            r2 = Data.GetByte(_highPowAddr);
            r2 = Data.GetByte(_midPowAddr) - r2;
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
            r1 = Data.GetByte(_midPowAddr) + r1;

            return r1;
        }

        public int DamageRoll(int rng)
            => (LowPow == 100) ? MaxPow : ((rng < 8) ? lowerRngFunc(rng) : upperRngFunc(rng));

        [TableViewModelColumn(displayName: "RNG0",  displayOrder: 3.10f)] public int DamageRoll0 => DamageRoll(0);
        [TableViewModelColumn(displayName: "RNG1",  displayOrder: 3.11f)] public int DamageRoll1 => DamageRoll(1);
        [TableViewModelColumn(displayName: "RNG2",  displayOrder: 3.12f)] public int DamageRoll2 => DamageRoll(2);
        [TableViewModelColumn(displayName: "RNG3",  displayOrder: 3.13f)] public int DamageRoll3 => DamageRoll(3);
        [TableViewModelColumn(displayName: "RNG4",  displayOrder: 3.14f)] public int DamageRoll4 => DamageRoll(4);
        [TableViewModelColumn(displayName: "RNG5",  displayOrder: 3.15f)] public int DamageRoll5 => DamageRoll(5);
        [TableViewModelColumn(displayName: "RNG6",  displayOrder: 3.16f)] public int DamageRoll6 => DamageRoll(6);
        [TableViewModelColumn(displayName: "RNG7",  displayOrder: 3.17f)] public int DamageRoll7 => DamageRoll(7);
        [TableViewModelColumn(displayName: "RNG8",  displayOrder: 3.18f)] public int DamageRoll8 => DamageRoll(8);
        [TableViewModelColumn(displayName: "RNG9",  displayOrder: 3.19f)] public int DamageRoll9 => DamageRoll(9);
        [TableViewModelColumn(displayName: "RNG10", displayOrder: 3.20f)] public int DamageRoll10 => DamageRoll(10);
        [TableViewModelColumn(displayName: "RNG11", displayOrder: 3.21f)] public int DamageRoll11 => DamageRoll(11);
        [TableViewModelColumn(displayName: "RNG12", displayOrder: 3.22f)] public int DamageRoll12 => DamageRoll(12);
        [TableViewModelColumn(displayName: "RNG13", displayOrder: 3.23f)] public int DamageRoll13 => DamageRoll(13);
        [TableViewModelColumn(displayName: "RNG14", displayOrder: 3.24f)] public int DamageRoll14 => DamageRoll(14);
    }
}
