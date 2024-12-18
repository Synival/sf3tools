using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.OpenGL {
    public class Shader : IDisposable {
        public Shader(string vertexPath, string fragmentPath) {
            var vertexShaderSource   = File.ReadAllText(vertexPath);
            var fragmentShaderSource = File.ReadAllText(fragmentPath);

            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);

            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            {
                GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out var success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            }

            GL.CompileShader(fragmentShaderHandle);
            {
                GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out var success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShaderHandle);
            GL.AttachShader(Handle, fragmentShaderHandle);
            GL.LinkProgram(Handle);

            {
                GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetProgramInfoLog(Handle));
            }

            GL.DetachShader(Handle, vertexShaderHandle);
            GL.DetachShader(Handle, fragmentShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);

            Use();
            var handle = GL.GetUniformLocation(Handle, "texture0");
            GL.Uniform1(handle, 0);

            // TODO: Determine from program
            Stride = 8 * sizeof(float);
        }

        public void Use() => GL.UseProgram(Handle);

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                GL.DeleteProgram(Handle);

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
        public int Stride { get; }
    }
}
