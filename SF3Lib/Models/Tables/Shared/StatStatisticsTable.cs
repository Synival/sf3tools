namespace SF3.Models.Tables.Shared {
    /// <summary>
    /// Container for useful statistics (*data* statistics, not HP, Atk, etc) related to character growth.
    /// </summary>
    public class StatStatisticsTable {
        public StatStatisticsTable(StatsTable statsTable) {
            StatsTable = statsTable;
        }

        public StatsTable StatsTable { get; }
    }
}
