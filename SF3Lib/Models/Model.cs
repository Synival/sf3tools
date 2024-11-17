using CommonLib.Attributes;
using SF3.BulkOperations;
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
        [DataMetadata(displayOrder: -1)]
        public string Name { get; protected set; }

        [DataMetadata(intDisplayMode: IntDisplayMode.Hex, displayOrder: -3)]
        public int ID { get; protected set; }

        [DataMetadata(displayName: "Address", displayOrder: -2, intDisplayMode: IntDisplayMode.Hex, displayFormat: "{0:X4}")]
        public int Address { get; protected set; }

        public int Size { get; protected set; }
    }
}
