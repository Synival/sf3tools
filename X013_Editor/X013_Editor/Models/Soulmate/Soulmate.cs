//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.Soulmate
{
    public class Soulmate
    {
        private int chance;
        private int address;
        private int offset;

        private int index;
        private string name;

        public Soulmate(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00007530; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00007484; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x0000736c; //scn3
            }
            else
                offset = 0x00007248; //pd
            


            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);
            
            int start = offset + (id * 1);
            chance = start; //2 bytes
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);

        }

        public int SoulmateID
        {
            get
            {
                return index;
            }
        }
        public string SoulmateName
        {
            get
            {
                return name;
            }
        }

        public int Chance
        {
            get
            {
                return FileEditor.getByte(chance);
            }
            set
            {
                FileEditor.setByte(chance, (byte)value);
            }
        }
        
        public int SoulmateAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
