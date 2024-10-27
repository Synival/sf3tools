using SF3.Tables.IconPointerEditor;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        bool IsX026 { get; }

        SpellIconTable SpellIconList { get; }
        ItemIconTable ItemIconList { get; }
    }
}
