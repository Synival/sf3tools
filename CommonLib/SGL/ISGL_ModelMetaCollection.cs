namespace CommonLib.SGL {
    public interface ISGL_ModelMetaCollection {
        /// <summary>
        /// Fetches a collection of models.
        /// </summary>
        /// <param name="mcId">Corresponding ModelCollectionID.</param>
        /// <returns>A collection if it exists, otherwise null.</returns>
        ISGL_ModelCollection GetModelCollection(int mcId);
    }
}
