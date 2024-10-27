using System;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models {
    public class ItemIcon : IModel {
        //ITEMS
        private readonly int theItemIcon;

        public ItemIcon(IByteEditor editor, int id, string name, int address, bool isSc1X026) {
            Editor    = editor;
            ID        = id;
            Name      = name;
            Address   = address;

            IsSc1X026 = isSc1X026;

            if (IsSc1X026) {
                Size = 2;
                theItemIcon = Address; // 1 byte
            }
            else {
                Size = 4;
                theItemIcon = Address; // 2 bytes
            }
        }

        public IByteEditor Editor { get; }
        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        public bool IsX026 { get; }
        public bool IsSc1X026 { get; }

        [BulkCopy]
        public int TheItemIcon {
            get {
                return IsSc1X026
                    ? Editor.GetWord(theItemIcon)
                    : Editor.GetDouble(theItemIcon);
            }
            set {
                if (IsSc1X026)
                    Editor.SetWord(theItemIcon, value);
                else
                    Editor.SetDouble(theItemIcon, value);
            }
        }
    }
}
