namespace CommonLib.Imaging {
    public static class TextureDataValidators {
        /// <summary>
        /// Returns 'null' when 'data' has dimensions of 'width' and 'height'. Otherwise, returns an error string.
        /// </summary>
        /// <typeparam name="T">Type of 'data'.</typeparam>
        /// <param name="data">Data to validate.</param>
        /// <param name="width">Required width of data.</param>
        /// <param name="height">Required height of data.</param>
        /// <returns>An error string or, if no error, null.</returns>
        public static string IsSameDimensions<T>(T[,] data, int width, int height) {
            return data.GetLength(0) != width || data.GetLength(1) != height
                ? $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {width}x{height}"
                : null;
        }
    }
}
