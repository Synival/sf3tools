using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Framebuffer : IDisposable {
        public Framebuffer(int width, int height, int colorBpp, RenderbufferStorage? depthStencilRenderBufferType) {
            Width  = width;
            Height = height;

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
            if (state.FramebufferReadHandle == Handle && state.FramebufferDrawHandle == Handle)
                return new StackElement();

            var lastReadHandle = state.FramebufferReadHandle;
            var lastDrawHandle = state.FramebufferDrawHandle;

            return new StackElement(
                () => {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
                    state.FramebufferReadHandle = Handle;
                    state.FramebufferDrawHandle = Handle;
                },
                () => {
                    if (lastDrawHandle == lastReadHandle)
                        GL.BindFramebuffer(FramebufferTarget.Framebuffer, lastReadHandle);
                    else if (lastDrawHandle != state.FramebufferReadHandle)
                        GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, lastReadHandle);
                    else
                        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, lastDrawHandle);

                    state.FramebufferReadHandle = lastReadHandle;
                    state.FramebufferDrawHandle = lastDrawHandle;
                }
            );
        }

        public StackElement UseRead() {
            var state = State.GetCurrentState();
            if (state.FramebufferReadHandle == Handle)
                return new StackElement();

            var lastReadHandle = state.FramebufferReadHandle;
            return new StackElement(
                () => {
                    GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, Handle);
                    state.FramebufferReadHandle = Handle;
                },
                () => {
                    GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, lastReadHandle);
                    state.FramebufferReadHandle = lastReadHandle;
                }
            );
        }

        public StackElement UseDraw() {
            var state = State.GetCurrentState();
            if (state.FramebufferDrawHandle == Handle)
                return new StackElement();

            var lastDrawHandle = state.FramebufferDrawHandle;
            return new StackElement(
                () => {
                    GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, Handle);
                    state.FramebufferDrawHandle = Handle;
                },
                () => {
                    GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, lastDrawHandle);
                    state.FramebufferDrawHandle = lastDrawHandle;
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

        public int Width { get; }
        public int Height { get; }
        public int Handle { get; }

        private Texture ColorTexture { get; set; }
        private Renderbuffer DepthStencilBuffer { get; set; }
    }
}
