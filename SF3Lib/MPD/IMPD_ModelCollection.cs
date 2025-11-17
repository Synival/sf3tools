using CommonLib.SGL;
using SF3.Models.Structs.MPD.Model;
using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_ModelCollection {
        SGL_Model GetSGLModel(PDataModel pdata);
        SGL_Model[] GetSGLModels();

        ModelCollectionType CollectionType { get; }
        int? MovableModelsIndex { get; }
    }
}
