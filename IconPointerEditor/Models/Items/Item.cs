﻿using System.Threading;
using System;
using System.Windows.Forms;
using SF3.Editor;
using SF3.Types;
using static SF3.IconPointerEditor.Forms.frmMain;

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

        public Item(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;

            if (Scenario == ScenarioType.Scenario1)
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
            else if (Scenario == ScenarioType.Scenario2)
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
            else if (Scenario == ScenarioType.Scenario3)
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
            else if (Scenario == ScenarioType.PremiumDisk)
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

            if (Globals.x026 == true && (Scenario == ScenarioType.Scenario1))
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

        public ScenarioType Scenario { get; }
        public int ID => index;
        public string Name => name;

        public int TheSpellIcon
        {
            get
            {
                if (Globals.x026 == true && (Scenario == ScenarioType.Scenario1))
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
                if (Globals.x026 == true && (Scenario == ScenarioType.Scenario1))
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
                if (Globals.x026 == true && (Scenario == ScenarioType.Scenario1))
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
                if (Globals.x026 == true && (Scenario == ScenarioType.Scenario1))
                {
                    FileEditor.setWord(theSpellIcon, value - realOffset);
                }
                else
                {
                    FileEditor.setDouble(theSpellIcon, value - realOffset);
                }
            }
        }

        public int Address => (address);
    }
}
