using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SF3.ByteData;
using SF3.Models.Structs;
using SF3.Models.Structs.Shared;

namespace SF3.Models.Tables.Shared {
    /// <summary>
    /// Table of *mathematical* statistics for *character* stats for all character and promotion combinations.
    /// </summary>
    public class StatStatisticsTable : ITable<StatStatistics> {
        public StatStatisticsTable(StatsTable statsTable, string name) {
            StatsTable = statsTable;
            Name = name;
            Rows = StatsTable.Select(x => new StatStatistics(x)).ToArray();
        }

        public StatsTable StatsTable { get; }

        public bool Load() => false;
        public bool Unload() => false;

        public string Name { get; }
        public bool IsLoaded => true;

        public StatStatistics[] Rows { get; }
        public IStruct[] RowObjs => Rows;
        public IEnumerator GetEnumerator() => Rows.GetEnumerator();
        IEnumerator<StatStatistics> IEnumerable<StatStatistics>.GetEnumerator() => ((IEnumerable<StatStatistics>) Rows).GetEnumerator();
        public StatStatistics this[int index] => Rows[index];

        public IByteData Data => StatsTable.Data;
        public int Address => StatsTable.Address;
        public int Length => StatsTable.Length;
        public int SizeInBytes => StatsTable.SizeInBytes;
        public int TerminatorSize => StatsTable.TerminatorSize;
        public int SizeInBytesPlusTerminator => StatsTable.SizeInBytesPlusTerminator;
        public bool IsContiguous => StatsTable.IsContiguous;
    }
}
