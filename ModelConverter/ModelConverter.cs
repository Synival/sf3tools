using CommonLib.SGL;

namespace ModelConverter {
    public class ModelConverter {
        public string ModelToGLTF(ISGL_Model model) {
            // WIP: The entire thing
            return "{}";
        }

        public ISGL_Model GLTF_ToModel(string gltfFile, int? modelCollectionId, int? modelId, int? levelOfDetail) {
            // WIP: The entire thing
            return new SGL_Model(modelCollectionId ?? 0, modelId ?? 0, levelOfDetail ?? 0);
        }
    }
}
