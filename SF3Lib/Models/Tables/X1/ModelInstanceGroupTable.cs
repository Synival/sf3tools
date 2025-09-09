using SF3.ByteData;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class ModelInstanceGroupTable : TerminatedTable<ModelInstanceGroup> {
        protected ModelInstanceGroupTable(IByteData data, string name, int address, bool addEndModel = true) : base(data, name, address, 4, null) {
            AddEndModel = addEndModel;
        }

        public static ModelInstanceGroupTable Create(IByteData data, string name, int address, bool addEndModel = true)
            => CreateBase(() => new ModelInstanceGroupTable(data, name, address, addEndModel: addEndModel));

        public override bool Load()
            => Load((id, address) => new ModelInstanceGroup(Data, id, $"{nameof(ModelInstanceGroup)}_{id:D2}", address),
                (rows, newModel) => newModel.ModelInstanceTablePtr != 0,
                addEndModel: AddEndModel);

        public bool AddEndModel { get; }
    }
}
