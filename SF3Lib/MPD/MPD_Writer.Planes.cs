using CommonLib.Extensions;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WritePlaneChunks(IMPD_Planes planes) {
            // Write the background image.
            var bgImage = planes.GroundImage ?? planes.GroundTileset ?? planes.BackgroundImage;
            if (bgImage != null && bgImage.Width == 512 && bgImage.Height == 256 && bgImage.BytesPerPixel == 1) {
                var data = bgImage.ImageData8Bit.To1DArrayTransposed();
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x00000, 0x10000));
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x10000, 0x10000));
            }
            else {
                WriteEmptyChunk();
                WriteEmptyChunk();
            }

            // TODO: tile assigmnent data (1/2) would go here
            WriteEmptyChunk();

            // Write the background image.
            var fgImage = planes.SkyBoxImage ?? planes.ForegroundTileset;
            if (fgImage != null && fgImage.Width == 512 && fgImage.Height == 256 && fgImage.BytesPerPixel == 1) {
                var data = fgImage.ImageData8Bit.To1DArrayTransposed();
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x00000, 0x10000));
                WriteCompressedChunk(writer => writer.WriteBytes(data, 0x10000, 0x10000));
            }
            else {
                WriteEmptyChunk();
                WriteEmptyChunk();
            }

            // TODO: tile assigmnent data (2/2) would go here
            WriteEmptyChunk();
        }
    }
}
