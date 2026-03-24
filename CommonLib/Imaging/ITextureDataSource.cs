namespace CommonLib.Imaging {
    public interface ITextureDataSource {
        byte[,] FetchImageData8Bit(ITextureData tex);
        ushort[,] FetchImageData16Bit(ITextureData tex);
    }
}
