using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Exceptions;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public class IconPointerFileEditor : SF3FileEditor, IIconPointerFileEditor {
        public IconPointerFileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override IEnumerable<ITable> MakeTables() {
            const int sub_X021 = 0x06068000;
            const int sub_X026 = 0x06078000;

            int spellIconAddress_X021;
            int spellIconAddress_X026;
            int itemIconAddress_X021;
            int itemIconAddress_X026;
            int spellIconRealOffsetStart;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    spellIconAddress_X021 = GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = GetDouble(0x0a30) - sub_X026;
                    itemIconAddress_X021  = GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = GetDouble(0x08f0) - sub_X026;
                    spellIconRealOffsetStart = 0xFF8E;
                    break;

                case ScenarioType.Scenario2:
                    spellIconAddress_X021 = GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = GetDouble(0x0a1c) - sub_X026;
                    itemIconAddress_X021  = GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = GetDouble(0x0a08) - sub_X026;
                    spellIconRealOffsetStart = 0xFC86;
                    break;

                case ScenarioType.Scenario3:
                    spellIconAddress_X021 = GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = GetDouble(0x09cc) - sub_X026;
                    itemIconAddress_X021  = GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = GetDouble(0x09b4) - sub_X026;
                    spellIconRealOffsetStart = 0x12A48;
                    break;

                case ScenarioType.PremiumDisk:
                    spellIconAddress_X021 = GetDouble(0x0030) - sub_X021;
                    spellIconAddress_X026 = GetDouble(0x07a0) - sub_X026;
                    itemIconAddress_X021  = GetDouble(0x003C) - sub_X021;
                    itemIconAddress_X026  = GetDouble(0x072c) - sub_X026;
                    spellIconRealOffsetStart = 0x12A32;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            bool isX021 = (spellIconAddress_X021 >= 0 && spellIconAddress_X021 < Data.Length &&
                           itemIconAddress_X021  >= 0 && itemIconAddress_X021  < Data.Length);
            bool isX026 = (spellIconAddress_X026 >= 0 && spellIconAddress_X026 < Data.Length &&
                           itemIconAddress_X026  >= 0 && itemIconAddress_X026  < Data.Length);

            if (!(isX021 || isX026))
                throw new FileEditorException("This doesn't look like an X021 or X026 file");
            else if (isX021 && isX026)
                throw new FileEditorException("This looks like both an X021 and X026 file");

            int spellIconAddress = isX026 ? spellIconAddress_X026 : spellIconAddress_X021;
            int itemIconAddress  = isX026 ? itemIconAddress_X026  : itemIconAddress_X021;
            var has16BitIconAddr = Scenario == ScenarioType.Scenario1 && isX026;

            return new List<ITable>() {
                (SpellIconTable = new SpellIconTable(this, spellIconAddress, has16BitIconAddr, spellIconRealOffsetStart)),
                (ItemIconTable  = new ItemIconTable(this, itemIconAddress, has16BitIconAddr))
            };
        }

        public override void DestroyTables() {
            SpellIconTable = null;
            ItemIconTable = null;
        }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }
    }
}
