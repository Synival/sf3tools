using System.Collections.Generic;
using SF3.Models.Tables.Shared;

namespace SF3.Models.Files {
    public interface IMonsterTableFile {
        IEnumerable<MonsterTable> MonsterTables { get; }
    }
}
