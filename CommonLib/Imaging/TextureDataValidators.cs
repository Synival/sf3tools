namespace CommonLib.Imaging {
    public static class TextureDataValidators {
        public static string IsSameDimensions<T>(T[,] data, int width, int height) {
            return data.GetLength(0) != width || data.GetLength(1) != height
                ? $"Incoming texture height ({data.GetLength(0)}x{data.GetLength(1)}) should be {width}x{height}"
                : null;
        }
    }
}
