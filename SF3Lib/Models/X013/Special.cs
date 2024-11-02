using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X013 {
    public class Special : IModel {
        private readonly int unknown1;
        private readonly int damageCalculation;
        private readonly int extraPow;
        private readonly int pow;
        private readonly int offset;
        private int r1;
        private int r2;
        private int r3;
        private int machh;
        private readonly int checkVersion2;

        public Special(IX013_FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x04;

            checkVersion2 = Editor.GetByte(0x0000000A);

            if (editor.Scenario == ScenarioType.Scenario1) {
                offset = 0x00007104; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    offset -= 0x0C;
            }
            else if (editor.Scenario == ScenarioType.Scenario2) {
                offset = 0x00006fdc; //scn2
            }
            else if (editor.Scenario == ScenarioType.Scenario3) {
                offset = 0x00006d18; //scn3
            }
            else {
                offset = 0x00006bf4; //pd
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd
            //int start = 0x354c + (id * 24);

            var start = offset + (id * 4);
            unknown1 = start; //2 bytes
            damageCalculation = start + 1; //1 byte
            extraPow = start + 2; //1 byte
            pow = start + 3; //1 byte
            Address = offset + (id * 0x4);
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
        public int DamageCalc {
            get => Editor.GetByte(damageCalculation);
            set => Editor.SetByte(damageCalculation, (byte) value);
        }

        [BulkCopy]
        public int ExtraPow {
            get => Editor.GetByte(extraPow);
            set => Editor.SetByte(extraPow, (byte) value);
        }

        [BulkCopy]
        public int Pow {
            get => Editor.GetByte(pow);
            set => Editor.SetByte(pow, (byte) value);
        }

        /*public int ranResult0
        {
            get
            {
                return Editor.GetByte(damageCalculation);
            }
            //set
            //{
            //    Editor.SetByte(pow, (byte)value);
            //}
        }*/

        public int ranResult0 {
            get {
                //x = extraPow;
                //y = damageCalculation;
                //ran is 1

                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 0; //random number

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
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult1 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 1; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult2 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 2; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult3 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 3; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult4 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 4; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult5 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 5; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult6 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 6; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult7 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    ;
                    r1 -= Editor.GetByte(damageCalculation);
                    ;
                    r3 = 7; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 += Editor.GetByte(damageCalculation);

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult8 {
            get {
                //x = extraPow;
                //y = damageCalculation;
                //ran is 1
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 8; //random number
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
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult9 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 9; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult10 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 10; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult11 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 11; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult12 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 12; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult13 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 13; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }

        public int ranResult14 {
            get {
                if (Editor.GetByte(damageCalculation) != 0x64) {
                    r1 = Editor.GetByte(extraPow);
                    r2 = Editor.GetByte(pow);
                    r2 = Editor.GetByte(extraPow) - r2;
                    r3 = 14; //random number
                    r1 = r3;
                    r1 -= 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 <<= 2;
                    r2 -= r2;
                    r1 -= r2;
                    r1 = Editor.GetByte(extraPow) + r1;

                    return r1;
                }
                else {
                    return Editor.GetByte(pow);
                }
            }
        }
    }
}
