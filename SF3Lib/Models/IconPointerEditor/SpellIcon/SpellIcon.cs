using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using SF3.Values;

namespace SF3.Models.IconPointerEditor.SpellIcon {
    public class SpellIcon {
        private readonly IIconPointerFileEditor _fileEditor;

        //SPELLS
        private readonly int theSpellIcon;
        private readonly int realOffset;

        //private int npcOffset;
        private readonly int offset;
        private readonly int sub;

        /*public int NPCTableAddress1
        {
            get => _fileEditor.GetDouble(npcOffset);
            set => _fileEditor.SetDouble(npcOffset, value);
        }

        public int NPCTableAddress2 => _fileEditor.GetDouble(NPCTableAddress1 - 0x0605F000);

        public int NPCTableAddress3 => _fileEditor.GetDouble(NPCTableAddress2 - 0x0605F000);*/

        public SpellIcon(IIconPointerFileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            if (Scenario == ScenarioType.Scenario1) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x0a30; //scn1 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn1 initial pointer
                    sub = 0x06068000;
                }

                //MessageBox.Show("" + _fileEditor.GetDouble(offset));

                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //pointer

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
            else if (Scenario == ScenarioType.Scenario2) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x0a1c; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0xFC86;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x09cc; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0x12A48;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x07a0; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x00000030; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset -= sub; //pointer

                realOffset = 0x12A32;
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            ID = id;
            IconName = text;

            //int start = 0x354c + (id * 24);

            if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                var start = offset + (id * 0x02);
                theSpellIcon = start; //2 bytes  
                                      //unknown42 = start + 52;
                Address = offset + (id * 0x02);
            }
            else {
                var start = offset + (id * 0x04);
                theSpellIcon = start; //2 bytes  
                                      //unknown42 = start + 52;
                Address = offset + (id * 0x04);
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
        public int ID { get; }

        [BulkCopyRowName]
        public string IconName { get; }

        public string SpellName => new SpellValue(Scenario, ID).Name;

        [BulkCopy]
        public int TheSpellIcon {
            get {
                return _fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)
                    ? _fileEditor.GetWord(theSpellIcon)
                    : _fileEditor.GetDouble(theSpellIcon);
            }
            set {
                if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                    _fileEditor.SetWord(theSpellIcon, value);
                }
                else {
                    _fileEditor.SetDouble(theSpellIcon, value);
                }
            }
        }

        [BulkCopy]
        public int RealOffset {
            get {
                return _fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)
                    ? _fileEditor.GetWord(theSpellIcon) + realOffset
                    : _fileEditor.GetDouble(theSpellIcon) + realOffset;
            }
            set {
                if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                    _fileEditor.SetWord(theSpellIcon, value - realOffset);
                }
                else {
                    _fileEditor.SetDouble(theSpellIcon, value - realOffset);
                }
            }
        }

        public int Address { get; }
    }
}