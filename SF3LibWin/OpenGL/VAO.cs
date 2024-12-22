using System;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class VAO {
        /// <summary>
        /// Vertex Array Object.
        /// </summary>
        public VAO() {
            Handle = GL.GenVertexArray();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;
            if (disposing)
                GL.DeleteVertexArray(Handle);
            disposed = true;
        }

        public StackElement Use() {
            var state = State.GetCurrentState();
            if (state.VertexArrayHandle == Handle)
                return new StackElement();

            var lastHandle = state.VertexArrayHandle;
            return new StackElement(
                () => {
                    GL.BindVertexArray(Handle);
                    state.VertexArrayHandle = Handle;
                },
                () => {
                    GL.BindVertexArray(lastHandle);
                    state.VertexArrayHandle = lastHandle;
                }
            );
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~VAO() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("VertexArray: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
    }
}
