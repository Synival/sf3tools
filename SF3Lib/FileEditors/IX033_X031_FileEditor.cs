using SF3.Models.X033_X031.InitialInfos;
using SF3.Models.X033_X031.Stats;
using SF3.Models.X033_X031.WeaponLevel;

namespace SF3.FileEditors
{
    public interface IX033_X031_FileEditor : ISF3FileEditor
    {
        StatsList StatsList { get; }
        InitialInfoList InitialInfoList { get; }
        WeaponLevelList WeaponLevelList { get; }
    }
}
