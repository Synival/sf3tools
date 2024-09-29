//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.MagicBonus
{
    public class MagicBonus
    {
        private int earthBonus;
        private int fireBonus;
        private int iceBonus;
        private int sparkBonus;
        private int windBonus;
        private int lightBonus;
        private int darkBonus;
        private int unknownBonus;


        private int address;
        private int offset;

        private int index;
        private string name;

        public MagicBonus(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00006e70; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00006ec8; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00006a40; //scn3
            }
            else
                offset = 0x00006914; //pd
            


            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);

            if (Globals.scenario == 1)
            {
                int start = offset + (id * 0x20);
                earthBonus = start +0x03; //1 bytes
                fireBonus = start + 0x07; //1 byte
                iceBonus = start + 0x0B; //1 byte
                sparkBonus = start + 0x0F; //1 byte
                windBonus = start + 0x13; //1 byte
                lightBonus = start + 0x17; //1 byte
                darkBonus = start + 0x1B; //1 byte
                unknownBonus = start + 0x1F; //1 byte

                address = offset + (id * 0x20);
            }
            else
            {
                int start = offset + (id * 8);
                earthBonus = start; //1 bytes
                fireBonus = start + 1; //1 byte
                iceBonus = start + 2; //1 byte
                sparkBonus = start + 3; //1 byte
                windBonus = start + 4; //1 byte
                lightBonus = start + 5; //1 byte
                darkBonus = start + 6; //1 byte
                unknownBonus = start + 7; //1 byte

                address = offset + (id * 0x8);
            }


            
            //address = 0x0354c + (id * 0x18);

        }

        public int MagicID
        {
            get
            {
                return index;
            }
        }
        public string MagicName
        {
            get
            {
                return name;
            }
        }

        public int EarthBonus
        {
            get
            {
                return FileEditor.getByte(earthBonus);
            }
            set
            {
                FileEditor.setByte(earthBonus, (byte)value);
            }
        }
        public int FireBonus
        {
            get
            {
                return FileEditor.getByte(fireBonus);
            }
            set
            {
                FileEditor.setByte(fireBonus, (byte)value);
            }
        }
        public int IceBonus
        {
            get
            {
                return FileEditor.getByte(iceBonus);
            }
            set
            {
                FileEditor.setByte(iceBonus, (byte)value);
            }
        }
        public int SparkBonus
        {
            get
            {
                return FileEditor.getByte(sparkBonus);
            }
            set
            {
                FileEditor.setByte(sparkBonus, (byte)value);
            }
        }

        public int WindBonus
        {
            get
            {
                return FileEditor.getByte(windBonus);
            }
            set
            {
                FileEditor.setByte(windBonus, (byte)value);
            }
        }

        public int LightBonus
        {
            get
            {
                return FileEditor.getByte(lightBonus);
            }
            set
            {
                FileEditor.setByte(lightBonus, (byte)value);
            }
        }

        public int DarkBonus
        {
            get
            {
                return FileEditor.getByte(darkBonus);
            }
            set
            {
                FileEditor.setByte(darkBonus, (byte)value);
            }
        }

        public int UnknownBonus
        {
            get
            {
                return FileEditor.getByte(unknownBonus);
            }
            set
            {
                FileEditor.setByte(unknownBonus, (byte)value);
            }
        }


        public int MagicAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
