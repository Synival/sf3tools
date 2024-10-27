using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.IconPointerEditor {
    public class ItemIcon : IModel {
        //ITEMS
        private readonly int theItemIcon;

        public ItemIcon(IIconPointerFileEditor fileEditor, int id, string text) {
            FileEditor = fileEditor;
            Scenario = fileEditor.Scenario;
            IsX026 = fileEditor.IsX026;
            ID = id;
            Name = text;

            int offset, sub;

            if (Scenario == ScenarioType.Scenario1) {
                if (IsX026 == true) {
                    offset = 0x08f0; //scn1 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn1 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer
            }
            else if (Scenario == ScenarioType.Scenario2) {
                if (IsX026 == true) {
                    offset = 0x0a08; //scn2 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn2 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer
            }
            else if (Scenario == ScenarioType.Scenario3) {
                if (IsX026 == true) {
                    offset = 0x09b4; //scn3 x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //scn3 initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                if (IsX026 == true) {
                    offset = 0x072c; //pd x026 initial pointer
                    sub = 0x06078000;
                }
                else {
                    offset = 0x0000003C; //pd initial pointer
                    sub = 0x06068000;
                }

                offset = FileEditor.GetDouble(offset);
                offset -= sub; //pointer
            }
            else
                throw new ArgumentException(nameof(fileEditor) + ".Scenario");

            if (IsSc1X026) {
                Size = 2;
                Address = offset + (id * Size);
                theItemIcon = Address; //1 byte
            }
            else {
                Size = 4;
                Address = offset + (id * Size);
                theItemIcon = Address; //2 bytes
            }
        }

        public IFileEditor FileEditor { get; }
        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public ScenarioType Scenario { get; }
        public bool IsX026 { get; }
        public bool IsSc1X026 => IsX026 && Scenario == ScenarioType.Scenario1;

        [BulkCopy]
        public int TheItemIcon {
            get {
                return IsSc1X026
                    ? FileEditor.GetWord(theItemIcon)
                    : FileEditor.GetDouble(theItemIcon);
            }
            set {
                if (IsSc1X026)
                    FileEditor.SetWord(theItemIcon, value);
                else
                    FileEditor.SetDouble(theItemIcon, value);
            }
        }
    }
}
