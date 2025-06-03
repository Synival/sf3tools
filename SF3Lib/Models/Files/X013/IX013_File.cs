using SF3.Models.Structs.X013;
using SF3.Models.Tables.X013;

namespace SF3.Models.Files.X013 {
    public interface IX013_File : IScenarioTableFile {
        SignificantValues SignificantValues { get; }
        SpecialTable SpecialsTable { get; }
        SpecialStatusEffectTable SpecialStatusEffectTable { get; }
        SupportTypeTable SupportTypeTable { get; }
        SupportStatsTable SupportStatsTable { get; }
        SoulmateTable SoulmateTable { get; }
        MagicBonusTable MagicBonusTable { get; }
        CritrateTable CritrateTable { get; }
        WeaponSpellRankTable WeaponSpellRankTable { get; }
        StatusEffectTable StatusEffectTable { get; }
        SpecialAnimationAssignmentTable SpecialAnimationAssignmentTable { get; }
    }
}
