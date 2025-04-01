using SF3.Models.Structs.X013;
using SF3.Models.Tables.X013;

namespace SF3.Models.Files.X013 {
    public interface IX013_File : IScenarioTableFile {
        SpecialTable SpecialsTable { get; }
        SpecialEffectTable SpecialEffectTable { get; }
        SupportTypeTable SupportTypeTable { get; }
        FriendshipExp FriendshipExp { get; }
        SupportStatsTable SupportStatsTable { get; }
        SoulmateTable SoulmateTable { get; }
        SoulFail SoulFail { get; }
        MagicBonusTable MagicBonusTable { get; }
        CritMod CritMod { get; }
        CritrateTable CritrateTable { get; }
        SpecialChances SpecialChances { get; }
        ExpLimit ExpLimit { get; }
        HealExp HealExp { get; }
        WeaponSpellRankTable WeaponSpellRankTable { get; }
        StatusEffectTable StatusEffectTable { get; }
    }
}
