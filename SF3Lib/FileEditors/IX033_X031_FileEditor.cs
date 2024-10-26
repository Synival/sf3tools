using SF3.Tables.X033_X031.InitialInfo;
using SF3.Tables.X033_X031.Stats;
using SF3.Tables.X033_X031.WeaponLevel;

namespace SF3.FileEditors {
    public interface IX033_X031_FileEditor : ISF3FileEditor {
        StatsTables StatsList { get; }
        InitialInfoTables InitialInfoList { get; }
        WeaponLevelTables WeaponLevelList { get; }
    }
}
