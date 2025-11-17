using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_ModelCollection {
        SGL_Model GetSGLModel(int id);
        SGL_Model[] GetSGLModels();
        IMPD_ModelInstance[] GetModelInstances();

        ModelCollectionType CollectionType { get; }
        int? MovableModelsIndex { get; }
    }
}
