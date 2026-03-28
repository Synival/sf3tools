using CommonLib.Arrays;
using CommonLib.Types;

namespace CommonLib.Imaging {
    /// <summary>
    /// A texture with caching capabilities whose data is stored in an IByteArray.
    /// </summary>
    public class InDataTextureData : CachedTextureData, ITextureData {
        public InDataTextureData(
            IByteArray data, int imageDataOffset,
            int width, int height, TexturePixelFormat pixelFormat, IPalette palette, bool isCompressed, bool zeroIsTransparent, bool canSetImage,
            IndexedColorUpdateStrategy indexedUpdateStrategy
        ) : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new InDataTextureDataSource(data, imageDataOffset, isCompressed),
            indexedUpdateStrategy
        ) {
        }

        public IByteArray Data {
            get => DataSource.Data;
            set {
                if (DataSource.Data != value) {
                    DataSource.Data = value;
                    Invalidate();
                }
            }
        }

        public virtual int ImageDataOffset {
            get => DataSource.ImageDataOffset;
            set {
                if (DataSource.ImageDataOffset != value) {
                    DataSource.ImageDataOffset = value;
                    Invalidate();
                }
            }
        }

        public bool IsCompressed {
            get => DataSource.IsCompressed;
            set {
                if (DataSource.IsCompressed != value) {
                    DataSource.IsCompressed = value;
                    Invalidate();
                }
            }
        }

        public int? StoredImageDataSize => DataSource.StoredImageDataSize;

        protected new InDataTextureDataSource DataSource => (InDataTextureDataSource) base.DataSource;
    }
}
