using SF3.Editor;
using static SF3.IconPointerEditor.Forms.frmMain;

namespace SF3.IconPointerEditor.Models.Presets
{
    public class Preset
    {
        //ITEMS
        private int theItemIcon;

        private int offset;
        private int address;

        private int index;
        private string name;
        private int sub;

        public Preset(int id, string text)
        {
            if (Globals.scenario == 1)
            {
                if (Globals.x026 == true)
                {
                    offset = 0x08f0; //scn1 initial pointer
                    sub = 0x06078000;
                    //MessageBox.Show("Could not load Resources/itemList.xml.");
                }
                else
                {
                    offset = 0x0000003C; //scn1 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer
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
                    offset = 0x0a08; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x0000003C; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = FileEditor.getDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = FileEditor.getDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */

            }
            else if (Globals.scenario == 3)
            {
                if (Globals.x026 == true)
                {
                    offset = 0x09b4; //scn3 x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x0000003C; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer
            }
            else if (Globals.scenario == 4)
            {
                if (Globals.x026 == true)
                {
                    offset = 0x072c; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else
                {
                    offset = 0x0000003C; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.getDouble(offset);
                offset = offset - sub; //pointer

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
                theItemIcon = start; //1 bytes

                address = offset + (id * 0x02);
            }
            else
            {
                int start = offset + (id * 0x04);
                theItemIcon = start; //1 bytes

                address = offset + (id * 0x04);
            }

            //address = 0x0354c + (id * 0x18);

        }

        public int SizeID => index;
        public string SizeName => name;

        public int TheItemIcon
        {
            get
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    return FileEditor.getWord(theItemIcon);
                }
                else
                {
                    return FileEditor.getDouble(theItemIcon);
                }

            }
            set
            {
                if (Globals.x026 == true && (Globals.scenario == 1))
                {
                    FileEditor.setWord(theItemIcon, value);
                }
                else
                {
                    FileEditor.setDouble(theItemIcon, value);
                }

            }
        }

        public int SizeAddress => (address);
    }
}
