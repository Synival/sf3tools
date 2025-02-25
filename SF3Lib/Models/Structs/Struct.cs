using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs {
    public abstract class Struct : IStruct {
        public Struct(IByteData data, int id, string name, int address, int size) {
            Data    = data;
            Name    = name;
            ID      = id;
            Address = address;
            Size    = size;
        }

        public IByteData Data { get; protected set; }

        [TableViewModelColumn(displayOrder: -3, displayFormat: "X2", minWidth: 45, displayGroup: "Metadata")]
        public int ID { get; protected set; }

        [TableViewModelColumn(displayOrder: -2, displayFormat: "X4", displayGroup: "Metadata")]
        public int Address { get; protected set; }

        [BulkCopyRowName]
        [TableViewModelColumn(displayOrder: -1, minWidth: 120, displayGroup: "Metadata")]
        public string Name { get; protected set; }

        public int Size { get; protected set; }
    }
}
