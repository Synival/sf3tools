using SF3.Tables;

namespace SF3.FileEditors {
    public interface IX033_X031_FileEditor : ISF3FileEditor {
        StatsTable StatsTable { get; }
        InitialInfoTable InitialInfoTable { get; }
        WeaponLevelTable WeaponLevelTable { get; }
    }
}
