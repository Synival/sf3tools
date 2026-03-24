using CommonLib.Arrays;
using CommonLib.Types;

namespace CommonLib.Imaging {
    public class InDataTextureData : TextureDataStandard, ITextureData {
        public InDataTextureData(
            IByteArray data, int imageDataOffset,
            int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool isCompressed, bool zeroIsTransparent, bool canSetImage
        ) : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new ExternalTextureDataSource(data, imageDataOffset, isCompressed)
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

        protected new ExternalTextureDataSource DataSource => (ExternalTextureDataSource) base.DataSource;
    }
}
