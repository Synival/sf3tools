using System.Collections.Generic;
using CommonLib.SGL;
using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_ModelCollection {
        /// <summary>
        /// Identifier for this collection of models.
        /// </summary>
        CollectionType Collection { get; }

        /// <summary>
        /// Fetches a single model belonging to this collection by ID.
        /// </summary>
        /// <param name="id">ID of the model to fetch.</param>
        /// <returns>A model in a structure compatible with SGL.</returns>
        ISGL_Model GetModel(int id);

        /// <summary>
        /// All models that belong to this collection.
        /// </summary>
        /// <returns>Several models in a structure compatible with SGL.</returns>
        IEnumerable<ISGL_Model> Models { get; }

        /// <summary>
        /// All instances of models that belong to this collection.
        /// </summary>
        /// <returns>Several instances of models in a structure compatible with SGL.</returns>
        IEnumerable<IMPD_ModelInstance> ModelInstances { get; }

        /// <summary>
        /// All textures associated with this collection.
        /// </summary>
        IEnumerable<ITexture> Textures { get; }

        int? MovableModelsIndex { get; }
    }
}
