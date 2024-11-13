using CommonLib.Attributes;
using SF3.RawEditors;

namespace SF3.Models {
    public abstract class Model : IModel {
        public Model(IRawEditor editor, int id, string name, int address, int size) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = size;
        }

        public IRawEditor Editor { get; protected set; }

        [BulkCopyRowName]
        public string Name { get; protected set; }
        public int ID { get; protected set; }
        public int Address { get; protected set; }
        public int Size { get; protected set; }
    }
}
