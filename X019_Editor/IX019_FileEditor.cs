using SF3.X019_Editor.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X019_Editor
{
    public interface IX019_FileEditor : ISF3FileEditor
    {
        MonsterList MonsterList { get; }
        bool IsX044 { get; }
    }
}
