using CommonLib;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    public interface IMPD_ModelCollection {
        /// <summary>
        /// Identifier for this collection of models.
        /// </summary>
        MPD_CollectionType Collection { get; }

        /// <summary>
        /// Fetches a single model belonging to this collection by ID.
        /// </summary>
        /// <param name="id">ID of the model to fetch.</param>
        /// <param name="lod">Level-of-detail index.</param>
        /// <returns>A model in a structure compatible with SGL.</returns>
        IMPD_Model GetModel(int id, int lod);

        /// <summary>
        /// All models that belong to this collection.
        /// </summary>
        /// <returns>Several models in a structure compatible with SGL.</returns>
        IEnumerableWithLength<IMPD_Model> Models { get; }

        /// <summary>
        /// All models that belong to this collection.
        /// </summary>
        /// <returns>Several models in a structure compatible with SGL.</returns>
        IEnumerableWithLength<IMPD_ModelWithLoD> ModelsWithLoD { get; }

        /// <summary>
        /// All instances of models that belong to this collection.
        /// </summary>
        /// <returns>Several instances of models in a structure compatible with SGL.</returns>
        IEnumerableWithLength<IMPD_ModelInstance> ModelInstances { get; }

        /// <summary>
        /// All textures associated with this collection.
        /// </summary>
        IEnumerableWithLength<IMPD_AnimatableTexture> Textures { get; }

        /// <summary>
        /// Optional data that exists after the model instance table but before PDATAs.
        /// This appears in ATBTL2 (S2), ELINB, and ATBTL2 (S3).
        /// </summary>
        IIndexedEnumerableWithLength<byte> DataAfterInstances { get; }

        /// <summary>
        /// When 'true', this model is serialized but not referenced in the MPD header.
        /// </summary>
        bool IsUnreferenced { get; set; }

        /// <summary>
        /// When true, textures exist for this model collection, but not models.
        /// </summary>
        bool HasMissingModels { get; }
    }
}
