using CommonLib.Attributes;
using SF3.StreamEditors;

namespace SF3.Models {
    public abstract class Model : IModel {
        public Model(IByteEditor editor, int id, string name, int address, int size) {
            Editor  = editor;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = size;
        }

        public IByteEditor Editor { get; protected set; }

        [BulkCopyRowName]
        public string Name { get; protected set; }
        public int ID { get; protected set; }
        public int Address { get; protected set; }
        public int Size { get; protected set; }
    }
}
