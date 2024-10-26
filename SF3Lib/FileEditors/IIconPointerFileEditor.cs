using SF3.Models.IconPointerEditor.ItemIcon;
using SF3.Models.IconPointerEditor.SpellIcon;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        bool IsX026 { get; }

        SpellIconList SpellIconList { get; }
        ItemIconList ItemIconList { get; }
    }
}
