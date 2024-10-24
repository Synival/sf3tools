using SF3.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.IconPointerEditor.ItemIcons {
    public class ItemIcon {
        private IIconPointerFileEditor _fileEditor;

        //ITEMS
        private int theItemIcon;

        private int offset;
        private int checkVersion2;
        private int address;

        private int index;
        private string name;
        private int sub;

        public ItemIcon(IIconPointerFileEditor fileEditor, int id, string text) {
            _fileEditor = fileEditor;

            checkVersion2 = _fileEditor.GetByte(0x0000000B);

            if (Scenario == ScenarioType.Scenario1) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x08f0; //scn1 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn1 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer
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
                    offset = 0x0a08; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer

                /*offset = 0x00000024; //scn2 initial pointer
                npcOffset = offset;
                npcOffset = _fileEditor.GetDouble(offset);
                sub = 0x0605e000;
                offset = npcOffset - sub + 4; //second pointer
                npcOffset = _fileEditor.GetDouble(offset);
                offset = npcOffset - sub; //third pointer
                //offset value should now point to where npc placements are
                */
            }
            else if (Scenario == ScenarioType.Scenario3) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x09b4; //scn3 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                if (_fileEditor.IsX026 == true) {
                    offset = 0x072c; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = _fileEditor.GetDouble(offset);
                offset = offset - sub; //pointer
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                int start = offset + (id * 0x02);
                theItemIcon = start; //1 bytes

                address = offset + (id * 0x02);
            }
            else {
                int start = offset + (id * 0x04);
                theItemIcon = start; //1 bytes

                address = offset + (id * 0x04);
            }

            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public int SizeID => index;

        [BulkCopyRowName]
        public string SizeName => name;

        [BulkCopy]
        public int TheItemIcon {
            get {
                if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                    return _fileEditor.GetWord(theItemIcon);
                }
                else {
                    return _fileEditor.GetDouble(theItemIcon);
                }
            }
            set {
                if (_fileEditor.IsX026 == true && (Scenario == ScenarioType.Scenario1)) {
                    _fileEditor.SetWord(theItemIcon, value);
                }
                else {
                    _fileEditor.SetDouble(theItemIcon, value);
                }
            }
        }

        public int SizeAddress => (address);
    }
}
