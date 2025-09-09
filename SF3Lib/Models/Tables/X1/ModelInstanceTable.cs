using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Models.Structs.X1;

namespace SF3.Models.Tables.X1 {
    public class ModelInstanceTable : TerminatedTable<ModelInstance> {
        protected ModelInstanceTable(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts, bool addEndModel = true)
        : base(data, name, address, 2, null) {
            _actorScripts = actorScripts;
            AddEndModel = addEndModel;
        }

        public static ModelInstanceTable Create(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts, bool addEndModel = true)
            => Create(() => new ModelInstanceTable(data, name, address, actorScripts, addEndModel: addEndModel));

        public override bool Load()
            => Load((id, address) => new ModelInstance(Data, id, $"{nameof(ModelInstance)}_{id:D2}", address, ActorScripts),
                (rows, newModel) => newModel.ModelID != -1,
                addEndModel: AddEndModel);

        private Dictionary<uint, ActorScript> _actorScripts;
        public Dictionary<uint, ActorScript> ActorScripts {
            get => _actorScripts;
            set {
                if (value != _actorScripts) {
                    _actorScripts = value;
                    foreach (var row in this)
                        row.ActorScripts = value;
                }
            }
        }

        public bool AddEndModel { get; }
    }
}
