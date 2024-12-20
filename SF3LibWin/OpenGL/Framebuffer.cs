using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Framebuffer {
        public Framebuffer(int width, int height) {
            ColorTexture = new Texture(width, height, PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte);
            DepthStencilTexture = new Texture(width, height, PixelInternalFormat.Depth24Stencil8, PixelFormat.DepthStencil, PixelType.UnsignedInt248);
            Handle = GL.GenFramebuffer();

            // Attach the textures to the framebuffer.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorTexture.Handle, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, TextureTarget.Texture2D, DepthStencilTexture.Handle, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Use(FramebufferTarget framebufferTarget)
            => GL.BindFramebuffer(framebufferTarget, Handle);

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                ColorTexture.Dispose();
                DepthStencilTexture.Dispose();
                GL.DeleteFramebuffer(Handle);
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Framebuffer() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("Framebuffer: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }

        private Texture ColorTexture { get; }
        private Texture DepthStencilTexture { get; }
    }
}
