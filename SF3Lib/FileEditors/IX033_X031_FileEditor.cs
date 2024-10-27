using SF3.Tables.X033_X031;

namespace SF3.FileEditors {
    public interface IX033_X031_FileEditor : ISF3FileEditor {
        StatsTable StatsTable { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevelTable WeaponLevelTable { get; }
    }
}
