using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.X013;
using SF3.Types;

namespace SF3.FileEditors {
    public class X013_FileEditor : SF3FileEditor, IX013_FileEditor {
        public X013_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override IEnumerable<ITable> MakeTables() {
            int checkVersion2 = GetByte(0x0000000A);

            int critModAddress;

            if (Scenario == ScenarioType.Scenario1) {
                critModAddress = 0x00002e74; //scn1
                if (checkVersion2 == 0x0A) //original jp
                    critModAddress -= 0x70;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                critModAddress = 0x00003050; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                critModAddress = 0x00002d58; //scn3
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                critModAddress = 0x00002d78; //pd
            }
            else
                throw new ArgumentException(nameof(Scenario));

            return new List<ITable>() {
                (SpecialsTable = new SpecialTable(this, 0)),
                (SupportTypeTable = new SupportTypeTable(this, 0)),
                (FriendshipExpTable = new FriendshipExpTable(this, 0)),
                (SupportStatsTable = new SupportStatsTable(this, 0)),
                (SoulmateTable = new SoulmateTable(this, 0)),
                (SoulfailTable = new SoulfailTable(this, 0)),
                (MagicBonusTable = new MagicBonusTable(this, 0)),
                (CritModTable = new CritModTable(this, critModAddress)),
                (CritrateTable = new CritrateTable(this, 0)),
                (SpecialChanceTable = new SpecialChanceTable(this, 0)),
                (ExpLimitTable = new ExpLimitTable(this, 0)),
                (HealExpTable = new HealExpTable(this, 0)),
                (WeaponSpellRankTable = new WeaponSpellRankTable(this, 0)),
                (StatusEffectTable = new StatusEffectTable(this, 0)),
            };
        }

        public override void DestroyTables() {
            SpecialsTable = null;
            SupportTypeTable = null;
            FriendshipExpTable = null;
            SupportStatsTable = null;
            SoulmateTable = null;
            SoulfailTable = null;
            MagicBonusTable = null;
            CritModTable = null;
            CritrateTable = null;
            SpecialChanceTable = null;
            ExpLimitTable = null;
            HealExpTable = null;
            WeaponSpellRankTable = null;
            StatusEffectTable = null;
        }

        [BulkCopyRecurse]
        public SpecialTable SpecialsTable { get; private set; }
        [BulkCopyRecurse]
        public SupportTypeTable SupportTypeTable { get; private set; }
        [BulkCopyRecurse]
        public FriendshipExpTable FriendshipExpTable { get; private set; }
        [BulkCopyRecurse]
        public SupportStatsTable SupportStatsTable { get; private set; }
        [BulkCopyRecurse]
        public SoulmateTable SoulmateTable { get; private set; }
        [BulkCopyRecurse]
        public SoulfailTable SoulfailTable { get; private set; }
        [BulkCopyRecurse]
        public MagicBonusTable MagicBonusTable { get; private set; }
        [BulkCopyRecurse]
        public CritModTable CritModTable { get; private set; }
        [BulkCopyRecurse]
        public CritrateTable CritrateTable { get; private set; }
        [BulkCopyRecurse]
        public SpecialChanceTable SpecialChanceTable { get; private set; }
        [BulkCopyRecurse]
        public ExpLimitTable ExpLimitTable { get; private set; }
        [BulkCopyRecurse]
        public HealExpTable HealExpTable { get; private set; }
        [BulkCopyRecurse]
        public WeaponSpellRankTable WeaponSpellRankTable { get; private set; }
        [BulkCopyRecurse]
        public StatusEffectTable StatusEffectTable { get; private set; }
    }
}
