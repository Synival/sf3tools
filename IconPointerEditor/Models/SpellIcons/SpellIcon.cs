using System.Threading;
using System;
using System.Windows.Forms;
using SF3.Editor;
using SF3.Types;
using static SF3.IconPointerEditor.Forms.frmIconPointerEditor;

namespace SF3.IconPointerEditor.Models.SpellIcons
{
    public class SpellIcon
    {
        private IIconPointerFileEditor _fileEditor;

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
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public SpellIcon(IIconPointerFileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1)
            {
                if (_fileEditor.X026 == true)
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

                //MessageBox.Show("" + _fileEditor.GetDouble(offset));

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0xFF8E;

                /*
                offset = 0x00000018; //scn1 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605f000;
                offset = npcOffset - sub; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                if (_fileEditor.X026 == true)
                {
                    offset = 0x0a1c; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0xFC86;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                if (_fileEditor.X026 == true)
                {
                    offset = 0x09cc; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer

                realOffset = 0x12A48;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                if (_fileEditor.X026 == true)
                {
                    offset = 0x07a0; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x00000030; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
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

            if (_fileEditor.X026 == true && (Scenario == ScenarioType.Scenario1))
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

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int ID => index;
        public string Name => name;

        public int TheSpellIcon
        {
            get
            {
                if (_fileEditor.X026 == true && (Scenario == ScenarioType.Scenario1))
                {
                    return _fileEditor.GetWord(theSpellIcon);
                }
                else
                {
                    return _fileEditor.GetDouble(theSpellIcon);
                }
            }
            set
            {
                if (_fileEditor.X026 == true && (Scenario == ScenarioType.Scenario1))
                {
                    _fileEditor.SetWord(theSpellIcon, value);
                }
                else
                {
                    _fileEditor.SetDouble(theSpellIcon, value);
                }
            }
        }

        public int RealOffset
        {
            get
            {
                if (_fileEditor.X026 == true && (Scenario == ScenarioType.Scenario1))
                {
                    return _fileEditor.GetWord(theSpellIcon) + realOffset;
                }
                else
                {
                    return _fileEditor.GetDouble(theSpellIcon) + realOffset;
                }
            }
            set
            {
                if (_fileEditor.X026 == true && (Scenario == ScenarioType.Scenario1))
                {
                    _fileEditor.SetWord(theSpellIcon, value - realOffset);
                }
                else
                {
                    _fileEditor.SetDouble(theSpellIcon, value - realOffset);
                }
            }
        }

        public int Address => (address);
    }
}
