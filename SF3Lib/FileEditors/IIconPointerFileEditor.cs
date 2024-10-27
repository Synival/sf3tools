using SF3.Tables;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        bool IsX026 { get; }

        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
