using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.X013;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Editors.X013 {
    public class X013_Editor : ScenarioTableEditor, IX013_Editor {
        protected X013_Editor(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static X013_Editor Create(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new X013_Editor(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
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
            int specialEffectAddress;
            int statusEffectAddress;
            int supportStatsAddress;
            int supportTypeAddress;
            int weaponSpellRankAddress;

            var checkVersion2 = Editor.GetByte(0x000A);

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
                    specialEffectAddress   = -1; // not present in scn1
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
                    specialEffectAddress   = -1; // not present in scn2
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
                    specialEffectAddress   = 0x711c;
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
                    specialEffectAddress   = 0x6ff8;
                    statusEffectAddress    = 0x70d8;
                    supportStatsAddress    = 0x71cd;
                    supportTypeAddress     = 0x7154;
                    weaponSpellRankAddress = 0x6BE0;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            var tables = new List<ITable>() {
                (SpecialsTable        = new SpecialTable(Editor, ResourceFileForScenario(Scenario, "Specials.xml"), specialAddress)),
                (SupportTypeTable     = new SupportTypeTable(Editor, ResourceFileForScenario(Scenario, "Characters.xml"), supportTypeAddress)),
                (FriendshipExpTable   = new FriendshipExpTable(Editor, ResourceFile("ExpList.xml"), friendshipExpAddress)),
                (SupportStatsTable    = new SupportStatsTable(Editor, ResourceFile("X013StatList.xml"), supportStatsAddress)),
                (SoulmateTable        = new SoulmateTable(Editor, ResourceFile("SoulmateList.xml"), soulmateAddress)),
                (SoulfailTable        = new SoulfailTable(Editor, ResourceFile("Soulfail.xml"), soulFailAddress)),
                (MagicBonusTable      = new MagicBonusTable(Editor, ResourceFileForScenario(Scenario, "MagicBonus.xml"), magicBonusAddress, Scenario == ScenarioType.Scenario1)),
                (CritModTable         = new CritModTable(Editor, ResourceFile("CritModList.xml"), critModAddress)),
                (CritrateTable        = new CritrateTable(Editor, ResourceFile("CritrateList.xml"), critrateAddress)),
                (SpecialChanceTable   = new SpecialChanceTable(Editor, ResourceFile("SpecialChanceList.xml"), specialChanceAddress, Scenario <= ScenarioType.Scenario2)),
                (ExpLimitTable        = new ExpLimitTable(Editor, ResourceFile("ExpLimitList.xml"), expLimitAddress)),
                (HealExpTable         = new HealExpTable(Editor, ResourceFile("HealExpList.xml"), healExpAddress)),
                (WeaponSpellRankTable = new WeaponSpellRankTable(Editor, ResourceFile("WeaponSpellRankList.xml"), weaponSpellRankAddress)),
                (StatusEffectTable    = new StatusEffectTable(Editor, ResourceFile("StatusGroupList.xml"), statusEffectAddress)),
            };

            if (specialEffectAddress >= 0)
                tables.Add(SpecialEffectTable = new SpecialEffectTable(Editor, ResourceFile("SpecialEffects.xml"), specialEffectAddress));

            return tables;
        }

        [BulkCopyRecurse]
        public SpecialTable SpecialsTable { get; private set; }
        [BulkCopyRecurse]
        public SpecialEffectTable SpecialEffectTable { get; private set; }
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