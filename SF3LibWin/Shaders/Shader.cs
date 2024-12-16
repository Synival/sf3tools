using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SF3.Win.Shaders {
    public class Shader : IDisposable {
        public Shader(string vertexPath, string fragmentPath) {
            string vertexShaderSource   = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);

            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);

            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            {
                GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            }

            GL.CompileShader(fragmentShaderHandle);
            {
                GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
            }

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShaderHandle);
            GL.AttachShader(_handle, fragmentShaderHandle);
            GL.LinkProgram(_handle);

            {
                GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);
                if (success == 0)
                    System.Diagnostics.Debug.WriteLine(GL.GetProgramInfoLog(_handle));
            }

            GL.DetachShader(_handle, vertexShaderHandle);
            GL.DetachShader(_handle, fragmentShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
        }

        public void Use() => GL.UseProgram(_handle);

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                GL.DeleteProgram(_handle);
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

        private readonly int _handle;
    }
}
