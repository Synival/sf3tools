﻿using SF3.Models.X019.Monsters;

namespace SF3.FileEditors
{
    public interface IX019_FileEditor : ISF3FileEditor
    {
        MonsterList MonsterList { get; }
        bool IsX044 { get; }
    }
}
