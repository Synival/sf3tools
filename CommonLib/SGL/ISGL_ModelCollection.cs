namespace CommonLib.SGL {
    public interface ISGL_ModelCollection {
        /// <summary>
        /// Fetches a single model belonging to this collection by ID.
        /// </summary>
        /// <param name="id">ID of the model to fetch.</param>
        /// <param name="lod">Level-of-detail index.</param>
        /// <returns>A model in a structure compatible with SGL.</returns>
        ISGL_Model GetModel(int id, int lod);
    }
}
