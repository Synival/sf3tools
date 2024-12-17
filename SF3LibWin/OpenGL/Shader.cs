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
        }

        public void Use() => GL.UseProgram(Handle);

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        ~Shader() {
            if (disposedValue == false)
                System.Diagnostics.Debug.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Handle { get; }
    }
}
