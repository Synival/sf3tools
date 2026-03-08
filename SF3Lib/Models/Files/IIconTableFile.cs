using SF3.Models.Tables.Shared;

namespace SF3.Models.Files {
    public interface IIconTableFile {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
