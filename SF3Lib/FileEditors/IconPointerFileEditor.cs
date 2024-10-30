using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileEditors {
    public class IconPointerFileEditor : SF3FileEditor, IIconPointerFileEditor {
        public IconPointerFileEditor(ScenarioType scenario, bool isX026) : base(scenario) {
            IsX026 = isX026;
        }

        public override IEnumerable<ITable> MakeTables() {
            bool has16BitIconAddr = Scenario == ScenarioType.Scenario1 && IsX026;

            int spellIconAddress;
            int spellIconRealOffsetStart;
            int itemIconAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1: {
                    int spellIconOffset = IsX026 ? 0x0a30 : 0x0030;
                    int sub = IsX026 ? 0x06078000 : 0x06068000;
                    int itemIconOffset = IsX026 ? 0x08f0 : 0x003C;

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0xFF8E;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.Scenario2: {
                    int spellIconOffset = IsX026 ? 0x0a1c : 0x0030;
                    int itemIconOffset  = IsX026 ? 0x0a08 : 0x003C;
                    int sub = IsX026 ? 0x06078000 : 0x06068000;

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0xFC86;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.Scenario3: {
                    int spellIconOffset = IsX026 ? 0x09cc : 0x0030;
                    int itemIconOffset  = IsX026 ? 0x09b4 : 0x003C;
                    int sub             = IsX026 ? 0x06078000 : 0x06068000;

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0x12A48;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.PremiumDisk: {
                    int spellIconOffset = IsX026 ? 0x07a0 : 0x0030;
                    int itemIconOffset  = IsX026 ? 0x072c : 0x003C;
                    int sub             = IsX026 ? 0x06078000 : 0x06068000;

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0x12A32;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            return new List<ITable>() {
                (SpellIconTable = new SpellIconTable(this, spellIconAddress, has16BitIconAddr, spellIconRealOffsetStart)),
                (ItemIconTable  = new ItemIconTable(this, itemIconAddress, has16BitIconAddr))
            };
        }

        public override void DestroyTables() {
            SpellIconTable = null;
            ItemIconTable = null;
        }

        public bool IsX026 { get; }

        [BulkCopyRecurse]
        public SpellIconTable SpellIconTable { get; private set; }

        [BulkCopyRecurse]
        public ItemIconTable ItemIconTable { get; private set; }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + (IsX026 ? " (X026)" : "")
            : base.BaseTitle;
    }
}
