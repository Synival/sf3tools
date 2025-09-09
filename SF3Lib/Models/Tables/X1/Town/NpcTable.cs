using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : TerminatedTable<Npc> {
        protected NpcTable(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts)
        : base(data, name, address, 2, 100) {
            ActorScripts = actorScripts;
        }

        public static NpcTable Create(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts)
            => Create(() => new NpcTable(data, name, address, actorScripts));

        public override bool Load() {
            return Load(
                (id, address) => new Npc(Data, id, "Npc" + id.ToString("D2"), address, ActorScripts),
                (rows, model) => model.SpriteID != 0xFFFF,
                false);
        }

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
    }
}
