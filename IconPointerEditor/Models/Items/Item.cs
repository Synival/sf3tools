//using SF3.IconPointerEditor.Forms;
//using SF3.IconPointerEditor.Models;
using System.Threading;
using System;
using System.Windows.Forms;
using static SF3.IconPointerEditor.Forms.frmMain;

//using SF3.IconPointerEditor.Models.StatTypes;


namespace SF3.IconPointerEditor.Models.Items
{
    public class Item
    {

        //SPELLS
        private int theSpellIcon;
        private int realOffset;


        //int pointerValue;

        private int address;
        //private int npcOffset;
        private int offset;
        private int sub;

        private int index;
        private string name;

        /*public int NPCTableAddress1
        {
            get => FileEditor.getDouble(npcOffset);
            set => FileEditor.setDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => FileEditor.getDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => FileEditor.getDouble(NPCTableAddress2 - 0x0605F000);*/







        public Item(int id, string text)
        {
            if (Globals.scenario == 1)
            {

                
                if (Globals.x026 == true)
                    {
                    offset = 0x0a30; //scn1 initial pointer
                    sub = 0x06078000;
                    //MessageBox.Show("Could not load Resources/itemList.xml.");
                    }
                else
                    {
                    offset = 0x00000030; //scn1 initial pointer
                    sub = 0x06068000;
                    }

                //MessageBox.Show("" + FileEditor.getDouble(offset));

                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer
               



                realOffset = 0xFF8E;

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.getDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = FileEditor.getDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Globals.scenario == 2)
            {

                

                if (Globals.x026 == true)
                {
                    offset = 0x0a1c; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //scn2 initial pointer
                    sub = 0x06068000;
                }


                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0xFC86;

            }
            else if (Globals.scenario == 3)
            {
                

                if (Globals.x026 == true)
                {
                    offset = 0x09cc; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //scn3 initial pointer
                    sub = 0x06068000;
                }


                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0x12A48;
            }
            else if (Globals.scenario == 4)
            {
                

                if (Globals.x026 == true)
                {
                    offset = 0x07a0; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //pd initial pointer
                    sub = 0x06068000;
                }


                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0x12A32;
            }

            

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;






            //int start = 0x354c + (id * 24);


            if (Globals.x026 == true && (Globals.scenario == 1))
            {
                int start = offset + (id * 0x02);
                theSpellIcon = start; //2 bytes  
                                      //unknown42 = start + 52;
                address = offset + (id * 0x02);
            }
            else
            {
                int start = offset + (id * 0x04);
                theSpellIcon = start; //2 bytes  
                                      //unknown42 = start + 52;
                address = offset + (id * 0x04);
            }


            /*int start = offset + (id * 0x04);
            theSpellIcon = start; //2 bytes  
            //unknown42 = start + 52;
            address = offset + (id * 0x04);*/
            //address = 0x0354c + (id * 0x18);
            //MessageBox.Show("" + offset);
            //MessageBox.Show("" + address);

        }

        public int ID
        {
            get
            {
                return index;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }

        public int TheSpellIcon
        {
            get
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    return FileEditor.getWord(theSpellIcon);
                }
                else
                {
                    return FileEditor.getDouble(theSpellIcon);
                }





                
            }
            set
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    FileEditor.setWord(theSpellIcon, value);
                }
                else
                {
                    FileEditor.setDouble(theSpellIcon, value);
                }

            }
        }

        public int RealOffset
        {
            get
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    return FileEditor.getWord(theSpellIcon) + realOffset;
                }
                else
                {
                    return FileEditor.getDouble(theSpellIcon) + realOffset;
                }



                




            }
            set
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    FileEditor.setWord(theSpellIcon, value - realOffset);
                }
                else
                {
                    FileEditor.setDouble(theSpellIcon, value - realOffset);
                }





                





            }
        }




        public int Address
        {
            get
            {
                return (address);
            }
        }



    }
}
