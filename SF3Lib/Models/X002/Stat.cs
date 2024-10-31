using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;

namespace SF3.Models.X002 {
    public class StatBoost : IModel {
        private readonly int stat;

        public StatBoost(ISF3FileEditor editor, int id, string name, int address) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = 0x01;

            stat = Address; // 1 byte
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

        [BulkCopy]
        public int Stat {
            get => Editor.GetByte(stat);
            set => Editor.SetByte(stat, (byte) value);
        }
    }
}
