//using STHAEditor.Forms;
//using STHAEditor.Models;
using static STHAEditor.Forms.frmMain;

//using STHAEditor.Models.StatTypes;


namespace STHAEditor.Models.HealExp
{
    public class HealExp
    {
        private int healExp;
        private int address;
        private int offset;

        private int index;
        private string name;

        public HealExp(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                offset = 0x00004c8b; //scn1
            }
            else if (Globals.scenario == 2)
            {
                offset = 0x00004ebf; //scn2
            }
            else if (Globals.scenario == 3)
            {
                offset = 0x00004aed; //scn3
            }
            else
                offset = 0x00004b01; //pd
            


            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);
            
            int start = offset + (id * 1);
            healExp = start; //1 byte
            address = offset + (id * 0x1);
            //address = 0x0354c + (id * 0x18);

        }

        public int HealExpID
        {
            get
            {
                return index;
            }
        }
        public string HealExpName
        {
            get
            {
                return name;
            }
        }

        public int HealBonus
        {
            get
            {
                return FileEditor.getByte(healExp);
            }
            set
            {
                FileEditor.setByte(healExp, (byte)value);
            }
        }
       
        

        public int HealExpAddress
        {
            get
            {
                return (address);
            }
        }



    }
}
