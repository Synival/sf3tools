using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SignificantValues : Struct {
        public SignificantValues(
            IByteData data, int id, string name,
            int critModAddr,
            int specialChancesAddr,
            int expLimitAddr,
            int friendshipExpAddr,
            int healExpAddr,
            int soulFailAddr,
            bool hasLargeSpecialChancesTable)
        : base(data, id, name, -1 /* actually N/A */, 0x01 /* actually N/A */) {
            CritMod        = new CritMod       (data, 0, "CritMod",        critModAddr);
            SpecialChances = new SpecialChances(data, 0, "SpecialChances", specialChancesAddr, hasLargeSpecialChancesTable);
            ExpLimit       = new ExpLimit      (data, 0, "ExpLimit",       expLimitAddr);
            FriendshipExp  = new FriendshipExp (data, 0, "FriendshipExp",  friendshipExpAddr);
            HealExp        = new HealExp       (data, 0, "HealExp",        healExpAddr);
            SoulFail       = new SoulFail      (data, 0, "SoulFail",       soulFailAddr);
        }

        private CritMod CritMod { get; }
        private SpecialChances SpecialChances { get; }
        private ExpLimit ExpLimit { get; }
        private FriendshipExp FriendshipExp { get; }
        private HealExp HealExp { get; }
        private SoulFail SoulFail { get; }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 0, displayName: "Attack Advantage Crit% Mod")]
        [BulkCopy]
        public int AttackAdvantageCritMod {
            get => CritMod.Advantage;
            set => CritMod.Advantage = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 1, displayName: "Attack Disadvantage Crit% Mod")]
        [BulkCopy]
        public int AttackDisadvantageCritMod {
            get => CritMod.Disadvantage;
            set => CritMod.Disadvantage = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 2, displayName: "Max Exp Checked Value")]
        [BulkCopy]
        public int MaxExpCheckedValue {
            get => ExpLimit.ExpCheck;
            set => ExpLimit.ExpCheck = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 3, displayName: "Max Exp Replacement Value")]
        [BulkCopy]
        public int MaxExpReplacementValue {
            get => ExpLimit.ExpReplacement;
            set => ExpLimit.ExpReplacement = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 4, displayName: "Heal Bonus Exp")]
        [BulkCopy]
        public int HealBonusExp {
            get => HealExp.HealBonus;
            set => HealExp.HealBonus = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 5, displayName: "Friendship Exp Required for Ally")]
        [BulkCopy]
        public int FriendshipExp_Lvl0_Ally {
            get => FriendshipExp.SLvl0_Ally;
            set => FriendshipExp.SLvl0_Ally = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 6, displayName: "Friendship Exp Required for Partner")]
        [BulkCopy]
        public int FriendshipExp_Lvl1_Partner {
            get => FriendshipExp.SLvl1_Partner;
            set => FriendshipExp.SLvl1_Partner = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 7, displayName: "Friendship Exp Required for Friend")]
        [BulkCopy]
        public int FriendshipExp_Lvl2_Friend {
            get => FriendshipExp.SLvl2_Friend;
            set => FriendshipExp.SLvl2_Friend = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 8, displayName: "Friendship Exp Required for Trusted")]
        [BulkCopy]
        public int FriendshipExp_Lvl3_Trusted {
            get => FriendshipExp.SLvl3_Trusted;
            set => FriendshipExp.SLvl3_Trusted = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 9, displayName: "Friendship Exp Required for Soulmate")]
        [BulkCopy]
        public int FriendshipExp_Lvl4_Soulmate {
            get => FriendshipExp.SLvl4_Soulmate;
            set => FriendshipExp.SLvl4_Soulmate = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 10, displayName: "Soulmate Fail Exp Mod")]
        [BulkCopy]
        public int SoulmateFailExpMod {
            get => SoulFail.ExpLost;
            set => SoulFail.ExpLost = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 11, displayName: "Special Chances for 2 Specials 2")]
        [BulkCopy]
        public int SpecialChances2Specials2 {
            get => SpecialChances.TwoSpecials2;
            set => SpecialChances.TwoSpecials2 = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 12, displayName: "Special Chances for 3 Specials 3")]
        [BulkCopy]
        public int SpecialChances3Specials3 {
            get => SpecialChances.ThreeSpecials3;
            set => SpecialChances.ThreeSpecials3 = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 13, displayName: "Special Chances for 3 Specials 2")]
        [BulkCopy]
        public int SpecialChances3Specials2 {
            get => SpecialChances.ThreeSpecials2;
            set => SpecialChances.ThreeSpecials2 = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 14, displayName: "Special Chances for 4 Specials 4")]
        [BulkCopy]
        public int SpecialChances4Specials4 {
            get => SpecialChances.FourSpecials4;
            set => SpecialChances.FourSpecials4 = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 15, displayName: "Special Chances for 4 Specials 4")]
        [BulkCopy]
        public int SpecialChances4Specials3 {
            get => SpecialChances.FourSpecials3;
            set => SpecialChances.FourSpecials3 = value;
        }

        // TODO: get address!
        [TableViewModelColumn(addressField: null, displayOrder: 16, displayName: "Special Chances for 4 Specials 2")]
        [BulkCopy]
        public int SpecialChances4Specials2 {
            get => SpecialChances.FourSpecials2;
            set => SpecialChances.FourSpecials2 = value;
        }
    }
}
