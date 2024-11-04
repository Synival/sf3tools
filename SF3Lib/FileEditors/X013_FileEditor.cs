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
            int critModAddress;
            int critrateAddress;
            int expLimitAddress;
            int friendshipExpAddress;
            int healExpAddress;
            int magicBonusAddress;
            int soulFailAddress;
            int soulmateAddress;
            int specialChanceAddress;
            int specialAddress;
            int statusEffectAddress;
            int supportStatsAddress;
            int supportTypeAddress;
            int weaponSpellRankAddress;

            int checkVersion2 = GetByte(0x0000000A);

            if (Scenario == ScenarioType.Scenario1) {
                critModAddress = 0x00002e74; //scn1
                critrateAddress = 0x000073f8; //scn1
                expLimitAddress = 0x00002173; //scn1
                friendshipExpAddress = 0x0000747c; //scn1
                healExpAddress = 0x00004c8b; //scn1
                magicBonusAddress = 0x00006e70; //scn1
                soulFailAddress = 0x00005e5f; //scn1
                soulmateAddress = 0x00007530; //scn1
                specialAddress = 0x00007104; //scn1
                specialChanceAddress = 0x000027ae; //scn1
                statusEffectAddress = 0x00007408; //scn1
                supportStatsAddress = 0x000074b5; //scn1
                supportTypeAddress = 0x00007484; //scn1
                weaponSpellRankAddress = 0x000070F0; //scn1

                if (checkVersion2 == 0x0A) { //original jp
                    critModAddress -= 0x70;
                    critrateAddress -= 0x0C;
                    expLimitAddress -= 0x68;
                    friendshipExpAddress -= 0x0C;
                    healExpAddress -= 0x64;
                    magicBonusAddress -= 0x0C;
                    soulFailAddress -= 0x36;
                    soulmateAddress -= 0x0C;
                    specialAddress -= 0x0C;
                    specialChanceAddress -= 0x70;
                    statusEffectAddress -= 0x0C;
                    supportStatsAddress -= 0x0C;
                    supportTypeAddress -= 0x0C;
                    weaponSpellRankAddress -= 0x0C;
                }
            }
            else if (Scenario == ScenarioType.Scenario2) {
                critModAddress = 0x00003050; //scn2
                critrateAddress = 0x00007304; //scn2
                expLimitAddress = 0x0000234f; //scn2
                friendshipExpAddress = 0x00007388; //scn2
                healExpAddress = 0x00004ebf; //scn2
                magicBonusAddress = 0x00006ec8; //scn2
                soulFailAddress = 0x0000650b; //scn2
                soulmateAddress = 0x00007484; //scn2
                specialAddress = 0x00006fdc; //scn2
                specialChanceAddress = 0x000029c6; //scn2
                statusEffectAddress = 0x00007314; //scn2
                supportStatsAddress = 0x00007409; //scn2
                supportTypeAddress = 0x00007390; //scn2
                weaponSpellRankAddress = 0x00006FC8; //scn2
            }
            else if (Scenario == ScenarioType.Scenario3) {
                critModAddress = 0x00002d58; //scn3
                critrateAddress = 0x000071dc; //scn3
                expLimitAddress = 0x0000218b; //scn3
                friendshipExpAddress = 0x00007270; //scn3
                healExpAddress = 0x00004aed; //scn3
                magicBonusAddress = 0x00006a40; //scn3
                soulFailAddress = 0x00006077; //scn3
                soulmateAddress = 0x0000736c; //scn3
                specialAddress = 0x00006d18; //scn3
                specialChanceAddress = 0x000027a2; //scn3
                statusEffectAddress = 0x000071fc; //scn3
                supportStatsAddress = 0x000072f1; //scn3
                supportTypeAddress = 0x00007278; //scn3
                weaponSpellRankAddress = 0x00006D04; //scn3
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                critModAddress = 0x00002d78; //pd
                critrateAddress = 0x000070b8; //pd
                expLimitAddress = 0x000021ab; //pd
                friendshipExpAddress = 0x0000714c; //pd
                healExpAddress = 0x00004b01; //pd
                magicBonusAddress = 0x00006914; //pd
                soulFailAddress = 0x00005f37; //pd
                soulmateAddress = 0x00007248; //pd
                specialAddress = 0x00006bf4; //pd
                specialChanceAddress = 0x000027c2; //pd
                statusEffectAddress = 0x000070d8; //pd
                supportStatsAddress = 0x000071cd; //pd
                supportTypeAddress = 0x00007154; //pd
                weaponSpellRankAddress = 0x00006BE0; //pd
            }
            else
                throw new ArgumentException(nameof(Scenario));

            return new List<ITable>() {
                (SpecialsTable = new SpecialTable(this, specialAddress)),
                (SupportTypeTable = new SupportTypeTable(this, supportTypeAddress)),
                (FriendshipExpTable = new FriendshipExpTable(this, friendshipExpAddress)),
                (SupportStatsTable = new SupportStatsTable(this, supportStatsAddress)),
                (SoulmateTable = new SoulmateTable(this, soulmateAddress)),
                (SoulfailTable = new SoulfailTable(this, soulFailAddress)),
                (MagicBonusTable = new MagicBonusTable(this, magicBonusAddress)),
                (CritModTable = new CritModTable(this, critModAddress)),
                (CritrateTable = new CritrateTable(this, critrateAddress)),
                (SpecialChanceTable = new SpecialChanceTable(this, specialChanceAddress)),
                (ExpLimitTable = new ExpLimitTable(this, expLimitAddress)),
                (HealExpTable = new HealExpTable(this, healExpAddress)),
                (WeaponSpellRankTable = new WeaponSpellRankTable(this, weaponSpellRankAddress)),
                (StatusEffectTable = new StatusEffectTable(this, statusEffectAddress)),
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
