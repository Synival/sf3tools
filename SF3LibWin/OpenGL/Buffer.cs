
using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Buffer : IDisposable {
        public Buffer() {
            Handle = GL.GenBuffer();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing)
                GL.DeleteBuffer(Handle);
            disposed = true;
        }

        public StackElement Use(BufferTarget bufferTarget) {
            var state = State.GetCurrentState();
            if (state.BufferHandles[bufferTarget] == Handle)
                return new StackElement();

            var lastHandle = state.BufferHandles[bufferTarget];
            return new StackElement(
                () => {
                    GL.BindBuffer(bufferTarget, Handle);
                    state.BufferHandles[bufferTarget] = Handle;
                },
                () => {
                    GL.BindBuffer(bufferTarget, lastHandle);
                    state.BufferHandles[bufferTarget] = lastHandle;
                }
            );
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Buffer() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("Buffer: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
    }
}
