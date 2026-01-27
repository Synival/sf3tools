using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Main;
using SF3.MPD.Interfaces;

namespace SF3.Models.Tables.MPD.Main {
    public class ModelSwitchGroupsTable : TerminatedTable<ModelSwitchGroup>, IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup> {
        protected ModelSwitchGroupsTable(IByteData data, string name, int address, INameGetterContext nameGetterContext) : base(data, name, address, 4, null) {
            NameGetterContext = nameGetterContext;
        }

        public static ModelSwitchGroupsTable Create(IByteData data, string name, int address, INameGetterContext nameGetterContext)
            => Create(() => new ModelSwitchGroupsTable(data, name, address, nameGetterContext));

        public override bool Load() {
            return Load(
                (id, address) => new ModelSwitchGroup(Data, id, "ModelSwitchGroup" + id.ToString("D2"), address, NameGetterContext),
                (currentRows, model) => model.Flag != -1,
                false);
        }

        public uint GetEarliestRamAddress() {
            var lowest = (uint) Address + 0x290000;

            void updateLowest(uint addr) {
                if (addr < 0x290000)
                    return;
                if (addr < lowest)
                    lowest = addr;
            }

            foreach (var msg in Rows) {
                updateLowest(msg.VisibleModelsWhenFlagOnOffset);
                updateLowest(msg.VisibleModelsWhenFlagOffOffset);
            }

            return lowest;
        }

        IEnumerator<IMPD_ModelSwitchGroup> IEnumerable<IMPD_ModelSwitchGroup>.GetEnumerator() => GetEnumerator();
        IMPD_ModelSwitchGroup IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup>.this[int index] => Rows[index];
        IMPD_ModelSwitchGroup[] IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup>.AsArray() => ((IEnumerable<IMPD_ModelSwitchGroup>) this).ToArray();

        public INameGetterContext NameGetterContext { get; }
    }
}
