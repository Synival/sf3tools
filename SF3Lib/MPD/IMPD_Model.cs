using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_Model : ISGL_Model {
        MPD_CollectionType Collection { get; }
    }
}
