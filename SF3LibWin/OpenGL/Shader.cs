using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            using (Use()) {
                // Texture uniforms need to be set.
                for (int i = 0; i < 4; i++) {
                    var handle = GL.GetUniformLocation(Handle, "texture" + i);
                    if (handle >= 0)
                        GL.Uniform1(handle, i);
                }

                // Get all shader attributes.
                GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out var attribCount);
                var _attributes = new ShaderAttribute[attribCount];

                int offset = 0;
                for (var i = 0; i < attribCount; i++) {
                    GL.GetActiveAttrib(Handle, i, 16, out var length, out var size, out var type, out var name);
                    _attributes[i] = new ShaderAttribute(i, size, type, offset, name);
                    offset += _attributes[i].Size;
                }

                Attributes = _attributes.OrderBy(x => x.Location).ToArray();
                _attributesByName = Attributes.ToDictionary(x => x.Name);

                VertexBufferStride = offset;
            }
        }

        public ShaderAttribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public int GetVertexBufferSize(int vertices)
            => vertices * VertexBufferStride;

        public void EnableVAO_Attribute(string attribName) {
            var attrib = GetAttributeByName(attribName);
            if (attrib == null)
                return;

            GL.VertexAttribPointer(attrib.Location, attrib.TypeElements, attrib.PointerType, false, VertexBufferStride, attrib.Offset);
            GL.EnableVertexAttribArray(attrib.Location);
        }

        public StackElement Use() {
            var state = State.GetCurrentState();
            if (state.ShaderHandle == Handle)
                return new StackElement();

            var lastHandle = state.ShaderHandle;
            return new StackElement(
                () => {
                    GL.UseProgram(Handle);
                    state.ShaderHandle = Handle;
                },
                () => {
                    GL.UseProgram(lastHandle);
                    state.ShaderHandle = lastHandle;
                }
            );
        }

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
                System.Diagnostics.Debug.WriteLine("Shader: GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public int Handle { get; }
        public int VertexBufferStride { get; }
        public ShaderAttribute[] Attributes { get; }

        private Dictionary<string, ShaderAttribute> _attributesByName;
    }
}
