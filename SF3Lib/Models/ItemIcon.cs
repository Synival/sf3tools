using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class ItemIcon : IModel {
        //ITEMS
        private readonly int theItemIcon;

        public ItemIcon(IByteEditor editor, int id, string name, int address, bool has16BitIconAddr) {
            Editor    = editor;
            ID        = id;
            Name      = name;
            Address   = address;

            Has16BitIconAddr = has16BitIconAddr;

            if (Has16BitIconAddr) {
                Size = 2;
                theItemIcon = Address; // 2 bytes
            }
            else {
                Size = 4;
                theItemIcon = Address; // 4 bytes
            }
        }

        public IByteEditor Editor { get; }
        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public bool IsX026 { get; }
        public bool Has16BitIconAddr { get; }

        [BulkCopy]
        public int TheItemIcon {
            get {
                return Has16BitIconAddr
                    ? Editor.GetWord(theItemIcon)
                    : Editor.GetDouble(theItemIcon);
            }
            set {
                if (Has16BitIconAddr)
                    Editor.SetWord(theItemIcon, value);
                else
                    Editor.SetDouble(theItemIcon, value);
            }
        }
    }
}
