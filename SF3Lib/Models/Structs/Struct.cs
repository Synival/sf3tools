using CommonLib.Attributes;
using SF3.RawData;

namespace SF3.Models.Structs {
    public abstract class Struct : IStruct {
        public Struct(IRawData data, int id, string name, int address, int size) {
            Data    = data;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = size;
        }

        public IRawData Data { get; protected set; }

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
