using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Texture : IDisposable {
        public Texture(Bitmap image) {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new ArgumentException(nameof(image.PixelFormat));

            Handle = GL.GenTexture();
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

            Update(image);
        }

        public Texture(int width, int height, PixelInternalFormat internalFormat, OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType) {
            Handle = GL.GenTexture();
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

            Update(width, height, internalFormat, format, pixelType);
        }

        public void Update(Bitmap image) {
            var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0
            );
            image.UnlockBits(bitmapData);
        }

        public void Update(int width, int height, PixelInternalFormat internalFormat, OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType)
            => GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, pixelType, 0);

        public void Use(TextureUnit activeTexture = TextureUnit.Texture0) {
            GL.ActiveTexture(activeTexture);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                GL.DeleteTexture(Handle);

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Texture() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("Texture: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
    }
}
