using SF3.X033_X031_Editor.Models.InitialInfos;
using SF3.X033_X031_Editor.Models.Stats;
using SF3.X033_X031_Editor.Models.WeaponLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X033_X031_Editor
{
    public interface IX033_X031_FileEditor : ISF3FileEditor
    {
        StatsList StatsList { get; }
        InitialInfoList InitialInfoList { get; }
        WeaponLevelList WeaponLevelList { get; }
    }
}
