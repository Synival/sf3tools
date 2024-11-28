using SF3.TableModels.IconPointer;

namespace SF3.FileModels.IconPointer {
    public interface IIconPointerEditor : IScenarioTableEditor {
        SpellIconTable SpellIconTable { get; }
        ItemIconTable ItemIconTable { get; }
    }
}
