using System.Collections.Generic;
using CommonLib;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Models.Structs.X1.Town;
using SF3.Scenes;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : TerminatedTable<Npc>, IScene, IIndexedEnumerableWithLength<IActor> {
        protected NpcTable(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts)
        : base(data, name, address, 2, 100) {
            ActorScripts = actorScripts;
        }

        public static NpcTable Create(IByteData data, string name, int address, Dictionary<uint, ActorScript> actorScripts)
            => Create(() => new NpcTable(data, name, address, actorScripts));

        public override bool Load() {
            Npc lastNewNPC = null;
            return Load(
                (id, address) => {
                    lastNewNPC = new Npc(Data, id, "Npc" + id.ToString("D2"), address, ActorScripts, lastNewNPC);
                    return lastNewNPC;
                },
                (rows, model) => model.SpriteID != 0xFFFF,
                false);
        }

        IActor[] IIndexedEnumerableWithLength<IActor>.AsArray() => Rows;
        IEnumerator<IActor> IEnumerable<IActor>.GetEnumerator() => GetEnumerator();
        IActor IIndexedEnumerableWithLength<IActor>.this[int index] => Rows[index];

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

        public bool IsBattle => false;
        public string SceneName => Name;
        public IIndexedEnumerableWithLength<IActor> Actors => this;
    }
}
