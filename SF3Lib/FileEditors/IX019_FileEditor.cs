using SF3.Tables.X019.Monster;

namespace SF3.FileEditors {
    public interface IX019_FileEditor : ISF3FileEditor {
        MonsterList MonsterList { get; }
        bool IsX044 { get; }
    }
}
