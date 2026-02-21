using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Framebuffer : IDisposable {
        public Framebuffer(int width, int height, int colorBpp, RenderbufferStorage? depthStencilRenderBufferType) {
            if (colorBpp == 4)
                ColorTexture = new Texture(width, height, PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte);
            else if (colorBpp == 3)
                ColorTexture = new Texture(width, height, PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.UnsignedByte);
            else if (colorBpp == 1)
                ColorTexture = new Texture(width, height, PixelInternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte);
            else
                throw new ArgumentException($"{nameof(colorBpp)} should be 1, 3, or 4");

            if (depthStencilRenderBufferType.HasValue)
                DepthStencilBuffer = new Renderbuffer(depthStencilRenderBufferType.Value, width, height);

            Handle = GL.GenFramebuffer();

            // Attach the textures to the framebuffer.
            using (Use()) {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, ColorTexture.Handle, 0);

                if (depthStencilRenderBufferType.HasValue) {
                    if (depthStencilRenderBufferType.Value == RenderbufferStorage.DepthComponent)
                        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DepthStencilBuffer.Handle);
                    else if (depthStencilRenderBufferType.Value == RenderbufferStorage.DepthStencil)
                        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, DepthStencilBuffer.Handle);
                    else
                        throw new ArgumentException($"{nameof(depthStencilRenderBufferType)} should be DepthComponent or DepthStencil");
                }
            }
        }

        public StackElement Use() {
            var state = State.GetCurrentState();
            if (state.FramebufferHandle == Handle)
                return new StackElement();

            var lastHandle = state.FramebufferHandle;
            return new StackElement(
                () => {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
                    state.FramebufferHandle = Handle;
                },
                () => {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, lastHandle);
                    state.FramebufferHandle = lastHandle;
                }
            );
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                ColorTexture?.Dispose();
                DepthStencilBuffer?.Dispose();
                GL.DeleteFramebuffer(Handle);

                ColorTexture = null;
                DepthStencilBuffer = null;
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

        private Texture ColorTexture { get; set; }
        private Renderbuffer DepthStencilBuffer { get; set; }
    }
}
