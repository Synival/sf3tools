using CommonLib.Attributes;

namespace SF3.Models.Structs.Shared {
    public class StatStatistics : Struct {
        /// <summary>
        /// *Mathematical* statistics for *character* stats for an individual character at a specific promotion.
        /// </summary>
        /// <param name="stats">Character stats to generate mathematical statistics from.</param>
        public StatStatistics(Stats stats)
        : base(stats.Data, stats.ID, stats.Name + " (Stats)", stats.Address, stats.Size) {
            Stats = stats;
        }

        public Stats Stats { get; }

        [TableViewModelColumn(displayOrder: 0, minWidth: 200)]
        public string HelloWorld => "Hello, " + Stats.PromotionLevel.ToString() + "!";
    }
}
