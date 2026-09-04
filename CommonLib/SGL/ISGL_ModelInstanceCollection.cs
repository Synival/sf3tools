using System.Collections.Generic;

namespace CommonLib.SGL {
    public interface ISGL_ModelInstanceCollection : IEnumerable<ISGL_ModelInstance> {
        /// <summary>
        /// Fetches a single model instance belonging to this collection by ID.
        /// </summary>
        /// <param name="id">ID of the model instance to fetch.</param>
        /// <returns>A model instance in a structure compatible with SGL.</returns>
        ISGL_ModelInstance GetModelInstance(int id);
    }
}
