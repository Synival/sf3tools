using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.OpenGL {
    public class Shader : IDisposable {
        public class TextureInfo {
            public TextureInfo(int texIndex, TextureUnit texUnit, string uniformName, string texCoordName) {
                TexIndex     = texIndex;
                TexUnit      = texUnit;
                UniformName  = uniformName;
                TexCoordName = texCoordName;
            }

            public int TexIndex { get; }
            public TextureUnit TexUnit { get; }
            public string UniformName { get; }
            public string TexCoordName { get; }
        };

        public static readonly TextureInfo[] TextureInfos = [
            new TextureInfo(0, TextureUnit.Texture0, "texture0",           "texCoord0"),
            new TextureInfo(1, TextureUnit.Texture1, "texture1",           "texCoord1"),
            new TextureInfo(2, TextureUnit.Texture2, "texture2",           "texCoord2"),
            new TextureInfo(3, TextureUnit.Texture3, "texture3",           "texCoord3"),
            new TextureInfo(4, TextureUnit.Texture4, "textureAtlas",       "texCoordAtlas"),
            new TextureInfo(5, TextureUnit.Texture5, "textureTerrainType", "texCoordTerrainType"),
            new TextureInfo(6, TextureUnit.Texture6, "textureEventID",     "texCoordEventID"),
        ];

        public static TextureInfo GetTextureInfo(MPD_TextureUnit texUnit)
            => GetTextureInfo((TextureUnit) texUnit);

        public static TextureInfo GetTextureInfo(TextureUnit texUnit)
            => TextureInfos.FirstOrDefault(x => x.TexUnit == texUnit);

        public Shader(string vertexShaderSource, string fragmentShaderSource) {
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
                foreach (var texInfo in TextureInfos) {
                    var handle = GL.GetUniformLocation(Handle, texInfo.UniformName);
                    if (handle >= 0)
                        GL.Uniform1(handle, texInfo.TexIndex);
                }

                // Get all shader attributes.
                GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out var attribCount);
                var _attributes = new ShaderAttribute[attribCount];

                int stride = 0;
                for (var i = 0; i < attribCount; i++) {
                    GL.GetActiveAttrib(Handle, i, 16, out var length, out var size, out var type, out var name);
                    var location = GL.GetAttribLocation(Handle, name);
                    _attributes[i] = new ShaderAttribute(location, size, type, name);
                    stride += _attributes[i].SizeInBytes;
                }

                Attributes = _attributes.OrderBy(x => x.Location).ToArray();
                _attributesByName = Attributes.ToDictionary(x => x.Name);

                VertexBufferStride = stride;
            }
        }

        public ShaderAttribute GetAttributeByName(string name)
            => _attributesByName.TryGetValue(name, out var value) ? value : null;

        public int GetVertexBufferSize(int vertices)
            => vertices * VertexBufferStride;

        private void AssignAttribute(ShaderAttribute shaderAttr, VBO_Attribute vboAttr, int stride) {
            if (vboAttr == null || !vboAttr.OffsetInBytes.HasValue || !shaderAttr.IsAssignable(vboAttr))
                GL.DisableVertexAttribArray(shaderAttr.Location);
            else {
                GL.VertexAttribPointer(shaderAttr.Location, shaderAttr.TypeElements, shaderAttr.PointerType, false, stride, vboAttr.OffsetInBytes.Value);
                GL.EnableVertexAttribArray(shaderAttr.Location);
            }
        }

        public void AssignAttributes(VBO vbo) {
            using (vbo.Use(BufferTarget.ArrayBuffer))
                foreach (var shaderAttr in Attributes)
                    AssignAttribute(shaderAttr, vbo?.GetAttributeByName(shaderAttr.Name), vbo.StrideInBytes);
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
