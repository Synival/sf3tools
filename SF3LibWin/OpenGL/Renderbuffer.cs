using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Renderbuffer : IDisposable {
        public Renderbuffer(int width, int height) {
            Handle = GL.GenRenderbuffer();
            using (Use())
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthStencil, width, height);
        }

        public StackElement Use() {
            var state = State.GetCurrentState();
            if (state.RenderbufferHandle == Handle)
                return new StackElement();

            var lastHandle = state.RenderbufferHandle;
            return new StackElement(
                () => {
                    GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
                    state.RenderbufferHandle = Handle;
                },
                () => {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, lastHandle);
                    state.RenderbufferHandle = lastHandle;
                }
            );
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                GL.DeleteRenderbuffer(Handle);
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Renderbuffer() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("Renderbuffer: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
    }
}
