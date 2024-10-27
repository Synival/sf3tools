using SF3.Tables.X033_X031;

namespace SF3.FileEditors {
    public interface IX033_X031_FileEditor : ISF3FileEditor {
        StatsTable StatsList { get; }
        InitialInfoTable InitialInfoList { get; }
        WeaponLevelTable WeaponLevelList { get; }
    }
}
