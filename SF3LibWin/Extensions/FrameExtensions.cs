using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using SF3.RawEditors;
using SF3.Models.Structs.MPD.TextureAnimation;

namespace SF3.Win.Extensions {
    public static class FrameExtensions {
        /// <summary>
        /// Creates a bitmap image using an a texture's BitmapDataARGB1555.
        /// </summary>
        /// <param name="texture">This texture whose Bitmap image should be generated.</param>
        /// <returns>A bitmap image for the texture.</returns>
        public static Bitmap CreateBitmap(this FrameModel frame, IByteEditor textureEditor) {
            // TODO: massive refactoring!
            var imageDataInput = textureEditor.GetAllData();
            var imageDataOutput = new byte[imageDataInput.Length];

            for (int i = 0; i < imageDataInput.Length; i += 2) {
                var inputValue = (ushort) ((imageDataInput[i + 0] << 8) + imageDataInput[i + 1] << 0);
                var newBits = PixelConversion.ARGB1555toABGR1555(inputValue);
                imageDataOutput[i + 0] = (byte) ((newBits & 0x00FF));
                imageDataOutput[i + 1] = (byte) ((newBits & 0xFF00) >> 8);
            }

            var bitmap = new Bitmap(frame.Width, frame.Height, PixelFormat.Format16bppArgb1555);
            var bitmapLock = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(imageDataOutput, 0, bitmapLock.Scan0, imageDataOutput.Length);
            bitmap.UnlockBits(bitmapLock);

            return bitmap;
        }
    }
}
