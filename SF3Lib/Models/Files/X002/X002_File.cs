using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Models.Files;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X002;
using SF3.RawEditors;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X002 {
    public class X002_File : ScenarioTableFile, IX002_File {
        protected X002_File(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static X002_File Create(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new X002_File(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
        }

        public override IEnumerable<ITable> MakeTables() {
            var checkVersion2 = Editor.GetByte(0x000B);

            int attackResistAddress;
            int itemAddress;
            int loadedOverrideAddress;
            int loadingAddress;
            int presetAddress;
            int spellAddress;
            int statBoostAddress;
            int warpAddress;
            int weaponRankAddress;

            switch (Scenario) {
                case ScenarioType.Scenario1:
                    var isOriginalJpVersion = checkVersion2 == 0x10;
                    attackResistAddress   = 0x0cb5;
                    itemAddress           = isOriginalJpVersion ? 0x2b1c : 0x2b28;
                    loadingAddress        = isOriginalJpVersion ? 0x4798 : 0x47A4;
                    loadedOverrideAddress = isOriginalJpVersion ? 0x526e : 0x527a;
                    presetAddress         = isOriginalJpVersion ? 0x472c : 0x4738;
                    spellAddress          = isOriginalJpVersion ? 0x431c : 0x4328;
                    statBoostAddress      = isOriginalJpVersion ? 0x452b : 0x4537;
                    warpAddress           = isOriginalJpVersion ? 0x53c0 : 0x53cc;
                    weaponRankAddress     = isOriginalJpVersion ? 0x29ec : 0x29f8;
                    break;

                case ScenarioType.Scenario2:
                    var isScn2Ver1003 = checkVersion2 == 0x2C;
                    attackResistAddress   = isScn2Ver1003 ? 0x0cd5 : 0x0d15;
                    itemAddress           = isScn2Ver1003 ? 0x2e58 : 0x2e9c;
                    loadingAddress        = isScn2Ver1003 ? 0x4b94 : 0x4bd8;
                    loadedOverrideAddress = isScn2Ver1003 ? 0x587a : 0x58be;
                    presetAddress         = isScn2Ver1003 ? 0x4b1c : 0x4b60;
                    spellAddress          = isScn2Ver1003 ? 0x4658 : 0x469c;
                    statBoostAddress      = isScn2Ver1003 ? 0x4867 : 0x48ab;
                    warpAddress           = 0; // not present
                    weaponRankAddress     = isScn2Ver1003 ? 0x2cbc : 0x2d00;
                    break;

                case ScenarioType.Scenario3:
                    attackResistAddress   = 0x0dcd;
                    itemAddress           = 0x354c;
                    loadedOverrideAddress = 0x6266;
                    loadingAddress        = 0x57d0;
                    presetAddress         = 0x5734;
                    spellAddress          = 0x516c;
                    statBoostAddress      = 0x537b;
                    warpAddress           = 0; // not present
                    weaponRankAddress     = 0x339c;
                    break;

                case ScenarioType.PremiumDisk:
                    attackResistAddress   = 0x0dd9;
                    itemAddress           = 0x35fc;
                    loadedOverrideAddress = 0x5aa2;
                    loadingAddress        = 0x58bc;
                    presetAddress         = 0x5820;
                    spellAddress          = 0x521c;
                    statBoostAddress      = 0x542b;
                    warpAddress           = 0; // not present
                    weaponRankAddress     = 0x344c;
                    break;

                default:
                    throw new ArgumentException(nameof(Scenario));
            }

            var tables = new List<ITable>() {
                (AttackResistTable   = new AttackResistTable(Editor, ResourceFile("AttackResistList.xml"), attackResistAddress)),
                (ItemTable           = new ItemTable(Editor, ResourceFileForScenario(Scenario, "Items.xml"), itemAddress)),
                (LoadedOverrideTable = new LoadedOverrideTable(Editor, ResourceFileForScenario(Scenario, "LoadedOverrideList.xml"), loadedOverrideAddress)),
                (LoadingTable        = new LoadingTable(Editor, ResourceFileForScenario(Scenario, "LoadList.xml"), loadingAddress)),
                (WeaponSpellTable    = new WeaponSpellTable(Editor, ResourceFileForScenario(Scenario, "WeaponSpells.xml"), presetAddress)),
                (SpellTable          = new SpellTable(Editor, ResourceFileForScenario(Scenario, "Spells.xml"), spellAddress)),
                (StatBoostTable      = new StatBoostTable(Editor, ResourceFile("X002StatList.xml"), statBoostAddress)),
                (WeaponRankTable     = new WeaponRankTable(Editor, ResourceFile("WeaponRankList.xml"), weaponRankAddress)),
            };

            if (Scenario == ScenarioType.Scenario1)
                tables.Add(WarpTable = new WarpTable(Editor, ResourceFileForScenario(ScenarioType.Scenario1, "Warps.xml"), warpAddress));

            return tables;
        }

        [BulkCopyRecurse]
        public AttackResistTable AttackResistTable { get; private set; }
        [BulkCopyRecurse]
        public ItemTable ItemTable { get; private set; }
        [BulkCopyRecurse]
        public LoadedOverrideTable LoadedOverrideTable { get; private set; }
        [BulkCopyRecurse]
        public LoadingTable LoadingTable { get; private set; }
        [BulkCopyRecurse]
        public WeaponSpellTable WeaponSpellTable { get; private set; }
        [BulkCopyRecurse]
        public SpellTable SpellTable { get; private set; }
        [BulkCopyRecurse]
        public StatBoostTable StatBoostTable { get; private set; }
        [BulkCopyRecurse]
        public WarpTable WarpTable { get; private set; }
        [BulkCopyRecurse]
        public WeaponRankTable WeaponRankTable { get; private set; }
    }
}
