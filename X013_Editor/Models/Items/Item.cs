using System;
using static SF3.X013_Editor.Forms.frmMain;

namespace SF3.X013_Editor.Models.Items
{
    public class Item
    {
        private int unknown1;
        private int damageCalculation;
        private int extraPow;
        private int pow;
        private int address;
        private int offset;
        private int r1;
        private int r2;
        private int r3;
        private int machh;

        private int index;
        private string name;

        public Item(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00007104; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00006fdc; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00006d18; //scn3
            }
            else
                offset = 0x00006bf4; //pd

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 4);
            unknown1 = start; //2 bytes
            damageCalculation = start + 1; //1 byte
            extraPow = start + 2; //1 byte
            pow = start + 3; //1 byte
            address = offset + (id * 0x4);
            //address = 0x0354c + (id * 0x18);

        }

        public int ID => index;
        public string Name => name;

        public int Unknown1
        {
            get
            {
                return FileEditor.getByte(unknown1);
            }
            set
            {
                FileEditor.setByte(unknown1, (byte)value);
            }
        }
        public int DamageCalc
        {
            get
            {
                return FileEditor.getByte(damageCalculation);
            }
            set
            {
                FileEditor.setByte(damageCalculation, (byte)value);
            }
        }
        public int ExtraPow
        {
            get
            {
                return FileEditor.getByte(extraPow);
            }
            set
            {
                FileEditor.setByte(extraPow, (byte)value);
            }
        }
        public int Pow
        {
            get
            {
                return FileEditor.getByte(pow);
            }
            set
            {
                FileEditor.setByte(pow, (byte)value);
            }
        }

        /*public int ranResult0
        {
            get
            {
                return FileEditor.getByte(damageCalculation);
            }
            //set
            //{
            //    FileEditor.setByte(pow, (byte)value);
            //}
        }*/

        public int ranResult0
        {
            get
            {
                //x = extraPow;
                //y = damageCalculation;
                //ran is 1

                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
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

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult1
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 1; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult2
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 2; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult3
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 3; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult4
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 4; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult5
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 5; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult6
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 6; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult7
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow); ;
                    r1 = r1 - FileEditor.getByte(damageCalculation); ;
                    r3 = 7; //random number

                    machh = r1 * r3;

                    r2 = machh;
                    r1 = r2 / 7;

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = r1 + FileEditor.getByte(damageCalculation);

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult8
        {
            get
            {
                //x = extraPow;
                //y = damageCalculation;
                //ran is 1
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 8; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    //r1 = 0x92492493;
                    //machh = r1 * r2;
                    //r1 = machh;
                    //Console.WriteLine("r1 = " + r1);
                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult9
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 9; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult10
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 10; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult11
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 11; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult12
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 12; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult13
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 13; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int ranResult14
        {
            get
            {
                if (FileEditor.getByte(damageCalculation) != 0x64)
                {
                    r1 = FileEditor.getByte(extraPow);
                    r2 = FileEditor.getByte(pow);
                    r2 = FileEditor.getByte(extraPow) - r2;
                    r3 = 14; //random number
                    r1 = r3;
                    r1 = r1 - 7;

                    machh = r1 * r2;

                    r2 = machh;

                    r1 = -(r2 / 7);

                    r2 = r2 << 2;
                    r2 = r2 - r2;
                    r1 = r1 - r2;
                    r1 = FileEditor.getByte(extraPow) + r1;

                    return r1;
                }
                else
                {
                    return FileEditor.getByte(pow);
                }

            }
        }

        public int Address => (address);
    }
}
