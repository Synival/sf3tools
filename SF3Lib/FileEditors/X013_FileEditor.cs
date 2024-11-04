using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

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

            int checkVersion2 = GetByte(0x000A);

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    critModAddress         = 0x2e74;
                    critrateAddress        = 0x73f8;
                    expLimitAddress        = 0x2173;
                    friendshipExpAddress   = 0x747c;
                    healExpAddress         = 0x4c8b;
                    magicBonusAddress      = 0x6e70;
                    soulFailAddress        = 0x5e5f;
                    soulmateAddress        = 0x7530;
                    specialAddress         = 0x7104;
                    specialChanceAddress   = 0x27ae;
                    statusEffectAddress    = 0x7408;
                    supportStatsAddress    = 0x74b5;
                    supportTypeAddress     = 0x7484;
                    weaponSpellRankAddress = 0x70F0;

                    if (checkVersion2 == 0x0A) { //original jp
                        critModAddress         -= 0x70;
                        critrateAddress        -= 0x0C;
                        expLimitAddress        -= 0x68;
                        friendshipExpAddress   -= 0x0C;
                        healExpAddress         -= 0x64;
                        magicBonusAddress      -= 0x0C;
                        soulFailAddress        -= 0x36;
                        soulmateAddress        -= 0x0C;
                        specialAddress         -= 0x0C;
                        specialChanceAddress   -= 0x70;
                        statusEffectAddress    -= 0x0C;
                        supportStatsAddress    -= 0x0C;
                        supportTypeAddress     -= 0x0C;
                        weaponSpellRankAddress -= 0x0C;
                    }
                    break;

                case ScenarioType.Scenario2:
                    critModAddress         = 0x3050;
                    critrateAddress        = 0x7304;
                    expLimitAddress        = 0x234f;
                    friendshipExpAddress   = 0x7388;
                    healExpAddress         = 0x4ebf;
                    magicBonusAddress      = 0x6ec8;
                    soulFailAddress        = 0x650b;
                    soulmateAddress        = 0x7484;
                    specialAddress         = 0x6fdc;
                    specialChanceAddress   = 0x29c6;
                    statusEffectAddress    = 0x7314;
                    supportStatsAddress    = 0x7409;
                    supportTypeAddress     = 0x7390;
                    weaponSpellRankAddress = 0x6FC8;
                    break;

                case ScenarioType.Scenario3:
                    critModAddress         = 0x2d58;
                    critrateAddress        = 0x71dc;
                    expLimitAddress        = 0x218b;
                    friendshipExpAddress   = 0x7270;
                    healExpAddress         = 0x4aed;
                    magicBonusAddress      = 0x6a40;
                    soulFailAddress        = 0x6077;
                    soulmateAddress        = 0x736c;
                    specialAddress         = 0x6d18;
                    specialChanceAddress   = 0x27a2;
                    statusEffectAddress    = 0x71fc;
                    supportStatsAddress    = 0x72f1;
                    supportTypeAddress     = 0x7278;
                    weaponSpellRankAddress = 0x6D04;
                    break;
                case ScenarioType.PremiumDisk:
                    critModAddress         = 0x2d78;
                    critrateAddress        = 0x70b8;
                    expLimitAddress        = 0x21ab;
                    friendshipExpAddress   = 0x714c;
                    healExpAddress         = 0x4b01;
                    magicBonusAddress      = 0x6914;
                    soulFailAddress        = 0x5f37;
                    soulmateAddress        = 0x7248;
                    specialAddress         = 0x6bf4;
                    specialChanceAddress   = 0x27c2;
                    statusEffectAddress    = 0x70d8;
                    supportStatsAddress    = 0x71cd;
                    supportTypeAddress     = 0x7154;
                    weaponSpellRankAddress = 0x6BE0;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            return new List<ITable>() {
                (SpecialsTable        = new SpecialTable(this, ResourceFileForScenario(Scenario, "Specials.xml"), specialAddress)),
                (SupportTypeTable     = new SupportTypeTable(this, ResourceFileForScenario(Scenario, "Characters.xml"), supportTypeAddress)),
                (FriendshipExpTable   = new FriendshipExpTable(this, ResourceFile("ExpList.xml"), friendshipExpAddress)),
                (SupportStatsTable    = new SupportStatsTable(this, ResourceFile("X013StatList.xml"), supportStatsAddress)),
                (SoulmateTable        = new SoulmateTable(this, ResourceFile("SoulmateList.xml"), soulmateAddress)),
                (SoulfailTable        = new SoulfailTable(this, ResourceFile("Soulfail.xml"), soulFailAddress)),
                (MagicBonusTable      = new MagicBonusTable(this, ResourceFileForScenario(Scenario, "MagicBonus.xml"), magicBonusAddress, Scenario == ScenarioType.Scenario1)),
                (CritModTable         = new CritModTable(this, ResourceFile("CritModList.xml"), critModAddress)),
                (CritrateTable        = new CritrateTable(this, ResourceFile("CritrateList.xml"), critrateAddress)),
                (SpecialChanceTable   = new SpecialChanceTable(this, ResourceFile("SpecialChanceList.xml"), specialChanceAddress, Scenario <= ScenarioType.Scenario2)),
                (ExpLimitTable        = new ExpLimitTable(this, ResourceFile("ExpLimitList.xml"), expLimitAddress)),
                (HealExpTable         = new HealExpTable(this, ResourceFile("HealExpList.xml"), healExpAddress)),
                (WeaponSpellRankTable = new WeaponSpellRankTable(this, ResourceFile("WeaponSpellRankList.xml"), weaponSpellRankAddress)),
                (StatusEffectTable    = new StatusEffectTable(this, ResourceFile("StatusGroupList.xml"), statusEffectAddress)),
            };
        }

        public override void DestroyTables() {
            SpecialsTable        = null;
            SupportTypeTable     = null;
            FriendshipExpTable   = null;
            SupportStatsTable    = null;
            SoulmateTable        = null;
            SoulfailTable        = null;
            MagicBonusTable      = null;
            CritModTable         = null;
            CritrateTable        = null;
            SpecialChanceTable   = null;
            ExpLimitTable        = null;
            HealExpTable         = null;
            WeaponSpellRankTable = null;
            StatusEffectTable    = null;
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
