using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class ModelMatrixGroupLinkTable : TerminatedTable<ModelMatrixGroupLink> {
        protected ModelMatrixGroupLinkTable(IByteData data, string name, int address, bool addEndModel = true) : base(data, name, address, 2, null) {
            AddEndModel = addEndModel;
        }

        public static ModelMatrixGroupLinkTable Create(IByteData data, string name, int address, bool addEndModel = true) {
            var newTable = new ModelMatrixGroupLinkTable(data, name, address, addEndModel: addEndModel);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ModelMatrixGroupLink(Data, id, $"ModelMatrixLink_{id}", address),
                (rows, newModel) => newModel.ModelID != -1,
                addEndModel: AddEndModel);

        public bool AddEndModel { get; }
    }
}
