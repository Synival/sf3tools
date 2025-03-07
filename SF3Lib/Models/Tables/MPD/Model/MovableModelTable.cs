using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.Models.Tables.MPD.Model {
    public class MovableModelTable : TerminatedTable<MovableModel> {
        protected MovableModelTable(IByteData data, string name, int address, ModelCollectionType collectionType) : base(data, name, address, 4, null) {
            CollectionType = collectionType;
        }

        public static MovableModelTable Create(IByteData data, string name, int address, ModelCollectionType collectionType) {
            var newTable = new MovableModelTable(data, name, address, collectionType);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new MovableModel(Data, id, "MovableModel" + id.ToString("D2"), address, CollectionType),
                (rows, lastRow) => lastRow.PData0 != 0x00,
                false);
        }

        public ModelCollectionType CollectionType { get; }
    }
}
