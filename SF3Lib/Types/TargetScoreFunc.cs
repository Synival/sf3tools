using System.ComponentModel;
using CommonLib.Attributes;

namespace SF3.Types {
    public enum TargetScoreFunc {
        [Description("-4 for each terrain penalty, +1 if leader, +X1 bonus")]
        [EnumDisplayName("Closest + Ranking")]
        ClosestPlusIsLeaderPlusRanking = 0,

        [Description("+10 if can kill, +2 if last attacked, +X1 bonus")]
        [EnumDisplayName("Will Kill then Ranking + Is Last Attacked")]
        WillKill_Then_RankingPlusIsLastAttacked = 1,

        [Description("+10 if can kill, +1 if new HP < 33%, +1 if new HP < 20%, +2 if last attacked, +X1 bonus")]
        [EnumDisplayName("Will Kill then Ranking + Is Last Attacked + Will Weaken")]
        WillKill_Then_RankingPlusIsLastAttackedPlusWillWeaken = 2,

        [Description("+10 if can kill, +1 if new HP < 20%")]
        [EnumDisplayName("Will Kill then Will Weaken")]
        WillKill_Then_WillWeaken = 3
    }
}
