using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X014;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Models.Files.X014 {
    public class X014_File : ScenarioTableFile, IX014_File {
        public override int RamAddress => 0x06088000;
        public override int RamAddressLimit => 0x0609D800;

        protected X014_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            if (Scenario == ScenarioType.Scenario2) {
                var versionCheckValue = data.GetInt32(0x08);
                if (versionCheckValue == 0x06094A60)
                    IsScn2V2 = false;
                else if (versionCheckValue == 0x06094A00)
                    IsScn2V2 = true;
                else
                    throw new Exception("Unable to determine Scenario 2 version");
            }

            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X014_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X014_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize X014_File");
            return newFile;
        }

        private uint GetCharacterBattleModelSc1TableAddr()
            => (Scenario == ScenarioType.Scenario1) ? 0xF5A8u : 0;

        private uint GetCharacterBattleModelAnimSc1TableAddr()
            => (Scenario == ScenarioType.Scenario1) ? 0x108ECu : 0;

        private uint GetEnemyBattleModelSc1TableAddr()
            => (Scenario == ScenarioType.Scenario1) ? 0xF848u : 0;

        private uint GetSpellEffectLocationTableAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0xFDA8u;
                case ScenarioType.Scenario2:   return IsScn2V2 ? 0x12920u : 0x12978u;
                case ScenarioType.Scenario3:   return 0x130D0u;
                case ScenarioType.PremiumDisk: return 0x131B0u;
                default:                       return 0;
            }
        }

        private uint GetSpecialEffectLocationTableAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0xFE7Cu;
                case ScenarioType.Scenario2:   return IsScn2V2 ? 0x12A18u : 0x12A70u;
                case ScenarioType.Scenario3:   return 0x13164u;
                case ScenarioType.PremiumDisk: return 0x1324Cu;
                default:                       return 0;
            }
        }

        private uint GetCharacterBattleModelSc2TableAddr()
            => (Scenario == ScenarioType.Scenario2) ? (IsScn2V2 ? 0x125C4u: 0x1261Cu) : 0;

        private uint GetCharacterBattleModelSc3TableAddr() {
            switch (Scenario) {
                case ScenarioType.Scenario3:   return 0x12A00u;
                case ScenarioType.PremiumDisk: return 0x12AB4u;
                default:                       return 0;
            }
        }

        private uint GetMPDBattleSceneInfoTableAddr() {
            switch (Scenario) {
            // These tables work differently (worse) in Scenario 1
            case ScenarioType.Scenario2:   return IsScn2V2 ? 0x119A0u : 0x11A00u;
            case ScenarioType.Scenario3:   return 0x11EDC;
            case ScenarioType.PremiumDisk: return 0x12110;
            default:                       return 0;
            }
        }

        private uint GetSpecialModelAnimationSubstitutionTableAddr() {
            switch (Scenario) {
            // TODO: Scenario 1,2,3?
            case ScenarioType.PremiumDisk: return 0x12d84;
            default: return 0;
            }
        }

        private uint GetSpellModelAnimationSubstitutionTableAddr() {
            switch (Scenario) {
            // TODO: Scenario 1,2,3?
            case ScenarioType.PremiumDisk: return 0x12f6c;
            default: return 0;
            }
        }

        public override IEnumerable<ITable> MakeTables() {
            var characterBattleModelSc1Addr = GetCharacterBattleModelSc1TableAddr();
            var characterBattleModelAnimSc1Addr = GetCharacterBattleModelAnimSc1TableAddr();
            var characterBattleModelSc2Addr = GetCharacterBattleModelSc2TableAddr();
            var characterBattleModelSc3Addr = GetCharacterBattleModelSc3TableAddr();
            var enemyBattleModelsSc1Addr    = GetEnemyBattleModelSc1TableAddr();
            var spellEffectLocationsAddr    = GetSpellEffectLocationTableAddr();
            var specialEffectLocationsAddr  = GetSpecialEffectLocationTableAddr();
            var mpdBattleSceneIdsAddr       = (Scenario == ScenarioType.Scenario1) ? 0xFA8Cu : 0;
            var mpdBattleSceneAddr          = GetMPDBattleSceneInfoTableAddr();
            var specAniSubTableAddr         = GetSpecialModelAnimationSubstitutionTableAddr();
            var spellAniSubTableAddr        = GetSpellModelAnimationSubstitutionTableAddr();

            var tables = new List<ITable>() {
                (CharacterBattleModelsSc1Table = (characterBattleModelSc1Addr   == 0) ? null : CharacterBattleModelsSc1Table.Create(Data, nameof(CharacterBattleModelsSc1Table), ResourceFileForScenario(Scenario, "ClassEquip.xml"), (int) characterBattleModelSc1Addr)),
                (CharacterBattleModelAnimsSc1Table = (characterBattleModelAnimSc1Addr == 0) ? null : ModelAnimLoadInfoSc1Table.Create(Data, nameof(ModelAnimLoadInfoSc1Table), (int) characterBattleModelAnimSc1Addr)),
                (CharacterBattleModelsSc2Table = (characterBattleModelSc2Addr   == 0) ? null : CharacterBattleModelsSc2Table.Create(Data, nameof(CharacterBattleModelsSc2Table), ResourceFileForScenario(Scenario, "ClassEquip.xml"), (int) characterBattleModelSc2Addr)),
                (CharacterBattleModelsSc3Table = (characterBattleModelSc3Addr   == 0) ? null : CharacterBattleModelsSc3Table.Create(Data, nameof(CharacterBattleModelsSc3Table), ResourceFileForScenario(Scenario, "ClassEquip.xml"), (int) characterBattleModelSc3Addr)),
                (EnemyBattleModelSc1Table      = (enemyBattleModelsSc1Addr      == 0) ? null : FileIdTable                  .Create(Data, nameof(CharacterBattleModelsSc1Table), ResourceFileForScenario(Scenario, "EnemyModels.xml"), (int) enemyBattleModelsSc1Addr)),
                (SpellEffectLocationTable      = (spellEffectLocationsAddr      == 0) ? null : EffectLocationTable          .Create(Data, nameof(SpellEffectLocationTable), ResourceFileForScenario(Scenario, "SpellAnimations.xml"), (int) spellEffectLocationsAddr, Scenario >= ScenarioType.Scenario3)),
                (SpecialEffectLocationTable    = (specialEffectLocationsAddr    == 0) ? null : EffectLocationTable          .Create(Data, nameof(SpecialEffectLocationTable), ResourceFileForScenario(Scenario, "SpecialAnimations.xml"), (int) specialEffectLocationsAddr, Scenario >= ScenarioType.Scenario3)),
                (MPDBattleSceneIdTable         = (mpdBattleSceneIdsAddr         == 0) ? null : MPDBattleSceneIdTable        .Create(Data, nameof(MPDBattleSceneIdTable), (int) mpdBattleSceneIdsAddr)),
                (BattleScenesByMapTable        = (Scenario != ScenarioType.Scenario1) ? null : Sc1BattleSceneFileIdTable    .Create(Data, nameof(BattleScenesByMapTable), ResourceFileForScenario(ScenarioType.Scenario1, "BattleScenesByBattle.xml"),  0xFBB0, 30, 0x000)),
                (BattleScenesOtherTable        = (Scenario != ScenarioType.Scenario1) ? null : Sc1BattleSceneFileIdTable    .Create(Data, nameof(BattleScenesByMapTable), ResourceFileForScenario(ScenarioType.Scenario1, "OtherBattleScenes.xml"),     0xFC64, 13, 0x100)),
                (BattleScenesByTerrainTable    = (Scenario != ScenarioType.Scenario1) ? null : Sc1BattleSceneFileIdTable    .Create(Data, nameof(BattleScenesByMapTable), ResourceFileForScenario(ScenarioType.Scenario1, "BattleScenesByTerrain.xml"), 0xFC28, 15, 0x200)),
                (MPDBattleSceneInfoTable       = (mpdBattleSceneAddr            == 0) ? null : MPDBattleSceneInfoTable      .Create(Data, nameof(MPDBattleSceneInfoTable), (int) mpdBattleSceneAddr)),
                (SpecialModelAnimationSubstitutionTable = (specAniSubTableAddr       == 0) ? null : SpecialModelAnimationSubstitutionTable.Create(Data, nameof(SpecialModelAnimationSubstitutionTable), (int) specAniSubTableAddr)),
                (SpellModelAnimationSubstitutionTable   = (spellAniSubTableAddr      == 0) ? null : SpellModelAnimationSubstitutionTable.Create(Data, nameof(SpellModelAnimationSubstitutionTable), (int) spellAniSubTableAddr)),
            }.Where(x => x != null).ToList();

            if (MPDBattleSceneInfoTable != null) {
                var addresses = MPDBattleSceneInfoTable
                    .Select(x => x.BattleSceneFileID)
                    .Where(x => x > 0x0600_0000)
                    .Distinct()
                    .ToList();

                // The Premium Disk has this, but it's unreferenced. Add if it doesn't exist.
                if (Scenario == ScenarioType.PremiumDisk && !addresses.Contains(0x120D0 + RamAddress))
                    addresses.Add(0x120D0 + RamAddress);

                TerrainBasedBattleSceneTablesByRamAddress = addresses
                    .ToDictionary(x => x, x => TerrainBasedBattleSceneTable.Create(Data, "TerrainBasedBattleSceneTable @" + x.ToString("X"), (x - RamAddress)));
                tables.AddRange(TerrainBasedBattleSceneTablesByRamAddress.Values);
            }

            return tables;
        }

        public bool IsScn2V2 { get; }

        public CharacterBattleModelsSc1Table CharacterBattleModelsSc1Table { get; private set; } = null;
        public ModelAnimLoadInfoSc1Table CharacterBattleModelAnimsSc1Table { get; private set; } = null;
        public CharacterBattleModelsSc2Table CharacterBattleModelsSc2Table { get; private set; } = null;
        public CharacterBattleModelsSc3Table CharacterBattleModelsSc3Table { get; private set; } = null;
        public FileIdTable EnemyBattleModelSc1Table { get; private set; } = null;
        public EffectLocationTable SpellEffectLocationTable { get; private set; } = null;
        public EffectLocationTable SpecialEffectLocationTable { get; private set; } = null;
        public MPDBattleSceneIdTable MPDBattleSceneIdTable { get; private set; } = null;
        public MPDBattleSceneInfoTable MPDBattleSceneInfoTable { get; private set; } = null;
        public Dictionary<int, TerrainBasedBattleSceneTable> TerrainBasedBattleSceneTablesByRamAddress { get; private set; } = null;
        public Sc1BattleSceneFileIdTable BattleScenesByMapTable { get; private set; } = null;
        public Sc1BattleSceneFileIdTable BattleScenesByTerrainTable { get; private set; } = null;
        public Sc1BattleSceneFileIdTable BattleScenesOtherTable { get; private set; } = null;
        public SpecialModelAnimationSubstitutionTable SpecialModelAnimationSubstitutionTable { get; private set; } = null;
        public SpellModelAnimationSubstitutionTable SpellModelAnimationSubstitutionTable { get; private set; } = null;
    }
}
