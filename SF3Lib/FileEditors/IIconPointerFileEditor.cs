using SF3.Models.IconPointerEditor.ItemIcons;
using SF3.Models.IconPointerEditor.SpellIcons;

namespace SF3.FileEditors {
    public interface IIconPointerFileEditor : ISF3FileEditor {
        bool IsX026 { get; }

        SpellIconList SpellIconList { get; }
        ItemIconList ItemIconList { get; }
    }
}
