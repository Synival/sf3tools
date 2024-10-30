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
                    int spellIconOffset, itemIconOffset, sub;
                    if (IsX026) {
                        spellIconOffset = 0x0a30;
                        itemIconOffset  = 0x08f0;
                        sub = 0x06078000;
                    }
                    else {
                        spellIconOffset = 0x0030;
                        itemIconOffset  = 0x003C;
                        sub = 0x06068000;
                    }

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0xFF8E;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.Scenario2: {
                    int spellIconOffset, itemIconOffset, sub;
                    if (IsX026) {
                        spellIconOffset = 0x0a1c;
                        itemIconOffset  = 0x0a08;
                        sub = 0x06078000;
                    }
                    else {
                        spellIconOffset = 0x00000030;
                        itemIconOffset  = 0x0000003C;
                        sub = 0x06068000;
                    }

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0xFC86;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.Scenario3: {
                    int spellIconOffset, itemIconOffset, sub;
                    if (IsX026) {
                        spellIconOffset = 0x09cc;
                        itemIconOffset  = 0x09b4;
                        sub = 0x06078000;
                    }
                    else {
                        spellIconOffset = 0x0030;
                        itemIconOffset  = 0x003C;
                        sub = 0x06068000;
                    }

                    spellIconAddress = GetDouble(spellIconOffset) - sub;
                    spellIconRealOffsetStart = 0x12A48;
                    itemIconAddress = GetDouble(itemIconOffset) - sub;
                    break;
                }

                case ScenarioType.PremiumDisk: {
                    int spellIconOffset, itemIconOffset, sub;
                    if (IsX026) {
                        spellIconOffset = 0x07a0;
                        itemIconOffset  = 0x072c;
                        sub = 0x06078000;
                    }
                    else {
                        spellIconOffset = 0x0030;
                        itemIconOffset  = 0x003C;
                        sub = 0x06068000;
                    }

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
