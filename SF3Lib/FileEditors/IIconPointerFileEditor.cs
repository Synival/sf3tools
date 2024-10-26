using SF3.Tables.IconPointerEditor.ItemIcon;
using SF3.Tables.IconPointerEditor.SpellIcon;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        bool IsX026 { get; }

        SpellIconList SpellIconList { get; }
        ItemIconList ItemIconList { get; }
    }
}
