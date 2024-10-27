using SF3.Tables.X019;

namespace SF3.FileEditors {
    public interface IX019_FileEditor : ISF3FileEditor {
        MonsterTable MonsterTable { get; }
        bool IsX044 { get; }
    }
}
