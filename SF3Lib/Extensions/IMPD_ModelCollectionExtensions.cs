using SF3.MPD;
using SF3.Types;

namespace SF3.Extensions {
    public static class IMPD_ModelCollectionExtensions {
        public static bool IsMovableModelCollection(this IMPD_ModelCollection mc)
            => mc.CollectionType >= ModelCollectionType.MovableModels1 && mc.CollectionType <= ModelCollectionType.MovableModels3;
    }
}
