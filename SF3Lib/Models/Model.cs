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

        [TableViewModelColumn(displayOrder: -3, displayFormat: "X2", minWidth: 45)]
        public int ID { get; protected set; }

        [TableViewModelColumn(displayOrder: -2, displayFormat: "X4")]
        public int Address { get; protected set; }

        [BulkCopyRowName]
        [TableViewModelColumn(displayOrder: -1, minWidth: 120)]
        public string Name { get; protected set; }

        public int Size { get; protected set; }
    }
}
