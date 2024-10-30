using SF3.Tables;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
