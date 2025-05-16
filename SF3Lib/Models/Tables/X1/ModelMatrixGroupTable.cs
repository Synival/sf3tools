using System;
using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class ModelMatrixGroupTable : TerminatedTable<ModelMatrixSetupGroup> {
        protected ModelMatrixGroupTable(IByteData data, string name, int address, bool addEndModel = true) : base(data, name, address, 4, null) {
            AddEndModel = addEndModel;
        }

        public static ModelMatrixGroupTable Create(IByteData data, string name, int address, bool addEndModel = true) {
            var newTable = new ModelMatrixGroupTable(data, name, address, addEndModel: addEndModel);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new ModelMatrixSetupGroup(Data, id, $"ModelMatrixGroup_{id}", address),
                (rows, newModel) => newModel.ModelMatrixGroupLinkTablePtr != 0,
                addEndModel: AddEndModel);

        public bool AddEndModel { get; }
    }
}
