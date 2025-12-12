using SF3.MPD;
using SF3.Types;

namespace SF3.Extensions {
    public static class IMPD_ModelCollectionExtensions {
        public static bool IsHeaderModelCollection(this IMPD_ModelCollection mc)
            => mc.Collection.IsHeaderModelCollection();
    }
}
