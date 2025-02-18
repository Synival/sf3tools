using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.OpenGL {
    public class Texture : IDisposable {
        public Texture(Bitmap image, bool minNearest = false, bool magNearest = true, bool clampToEdge = true) {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new ArgumentException(nameof(image.PixelFormat));

            Handle = GL.GenTexture();
            Width  = image.Width;
            Height = image.Height;
            MinNearest = minNearest;
            MagNearest = magNearest;
            ClampToEdge = clampToEdge;

            using (Use()) {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (minNearest ? TextureMagFilter.Nearest : TextureMagFilter.Linear));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (magNearest ? TextureMagFilter.Nearest : TextureMagFilter.Linear));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) (clampToEdge ? TextureWrapMode.ClampToEdge : TextureWrapMode.Repeat));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) (clampToEdge ? TextureWrapMode.ClampToEdge : TextureWrapMode.Repeat));
            }

            Update(image);
        }

        public Texture(int width, int height, PixelInternalFormat internalFormat, OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType, bool minNearest = false, bool magNearest = true, bool clampToEdge = true) {
            Handle = GL.GenTexture();
            Width  = width;
            Height = height;
            MinNearest = minNearest;
            MagNearest = magNearest;
            ClampToEdge = clampToEdge;

            using (Use()) {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) (minNearest ? TextureMagFilter.Nearest : TextureMagFilter.Linear));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) (magNearest ? TextureMagFilter.Nearest : TextureMagFilter.Linear));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) (clampToEdge ? TextureWrapMode.ClampToEdge : TextureWrapMode.Repeat));
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) (clampToEdge ? TextureWrapMode.ClampToEdge : TextureWrapMode.Repeat));
                Update(width, height, internalFormat, format, pixelType);
            }
        }

        public void Update(Bitmap image) {
            Width  = image.Width;
            Height = image.Height;

            using (Use()) {
                var bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
                GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0
                );
                image.UnlockBits(bitmapData);
            }
        }

        public void Update(int width, int height, PixelInternalFormat internalFormat, OpenTK.Graphics.OpenGL.PixelFormat format, PixelType pixelType) {
            Width  = width;
            Height = height;
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, pixelType, 0);
        }

        public StackElement Use(MPD_TextureUnit activeTexture)
            => Use((TextureUnit) activeTexture);

        public StackElement Use(TextureUnit activeTexture = TextureUnit.Texture0) {
            var state = State.GetCurrentState();

            var lastActiveTexture = state.ActiveTexture;
            var lastHandle = state.TextureHandles[activeTexture];
            if (lastActiveTexture == activeTexture && lastHandle == Handle)
                return null;

            return new StackElement(
                () => {
                    if (activeTexture != lastActiveTexture) {
                        GL.ActiveTexture(activeTexture);
                        state.ActiveTexture = activeTexture;
                    }
                    if (Handle != lastHandle) {
                        GL.BindTexture(TextureTarget.Texture2D, Handle);
                        state.TextureHandles[activeTexture] = Handle;
                    }
                },
                () => {
                    if (Handle != lastHandle) {
                        GL.BindTexture(TextureTarget.Texture2D, lastHandle);
                        state.TextureHandles[activeTexture] = lastHandle;
                    }
                    if (activeTexture != lastActiveTexture) {
                        GL.ActiveTexture(lastActiveTexture);
                        state.ActiveTexture = lastActiveTexture;
                    }
                }
            );
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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool MinNearest { get; }
        public bool MagNearest { get; }
        public bool ClampToEdge { get; }
    }
}
