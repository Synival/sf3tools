﻿using System;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using SF3.Win.ThirdParty.TexturePacker;

namespace SF3.Win.OpenGL {
    public class QuadModel : IDisposable {
        public QuadModel(Quad[] quads, Shader shader) {
            if (quads == null)
                throw new ArgumentNullException(nameof(quads));
            if (shader == null)
                throw new ArgumentNullException(nameof(quads));

            Quads = quads;
            Shader = shader;

            // Create a texture atlas for this model and generate a texture for it.
            // TODO: let QuadModels share TextureAtlas' + Textures
            var textures = quads
                .Where(x => x.TextureAnim != null)
                .SelectMany(x => x.TextureAnim.Frames)
                .Distinct()
                .ToArray();

            _textureAtlas = new TextureAtlas(textures, 0, true);
            _textureBitmap = _textureAtlas.CreateBitmap();
            _texture = _textureBitmap != null ? new Texture(_textureBitmap) : null;

            // Create the VBO.
            _vertexBuffer = new float[shader.GetVertexBufferSize(Quads.Length * 4) / sizeof(float)];
            AssignVertexBufferPositions();
            AssignVertexBufferColors();
            _ = AssignVertexBufferTexCoords();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexBuffer.Length * sizeof(float), _vertexBuffer, BufferUsageHint.DynamicDraw);

            // Create the VAO.
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            foreach (var attr in new string[] { "position", "color", "texCoord" })
                Shader.EnableVAO_Attribute(attr);

            // Create indices for DrawElements().
            // TODO: quads are malformed :( :( :(
            _elementBuffer = quads
                .Select((x, i) => new { Quad = x, StartIndex = (uint) i * 4 })
                .SelectMany(x => new uint[] {
                    x.StartIndex + 0, x.StartIndex + 1, x.StartIndex + 2,
                    x.StartIndex + 0, x.StartIndex + 2, x.StartIndex + 3
                })
                .ToArray();

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _elementBuffer.Length * sizeof(uint), _elementBuffer, BufferUsageHint.StaticDraw);
        }

        private void AssignVertexBufferColors() {
            var attr = Shader.GetAttributeByName("position");
            if (attr == null)
                return;

            var pos = attr.Offset / sizeof(float);
            int i = 0;
            foreach (var quad in Quads) {
                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    var color = quad.Colors[vertexIndex];
                    _vertexBuffer[pos + 0] = color.X;
                    _vertexBuffer[pos + 1] = color.Y;
                    _vertexBuffer[pos + 2] = color.Z;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
                i++;
            }
        }

        private void AssignVertexBufferPositions() {
            var attr = Shader.GetAttributeByName("color");
            if (attr == null)
                return;

            var pos = attr.Offset / sizeof(float);
            foreach (var quad in Quads) {
                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    var vertex = quad.Vertices[vertexIndex];
                    _vertexBuffer[pos + 0] = vertex.X;
                    _vertexBuffer[pos + 1] = vertex.Y;
                    _vertexBuffer[pos + 2] = vertex.Z;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
            }
        }

        private int _frame = 0;

        public bool UpdateAnimatedTextures(int frameIncrement = 1) {
            _frame += frameIncrement;
            return AssignVertexBufferTexCoords();
        }

        private bool AssignVertexBufferTexCoords() {
            if (_texture == null)
                return false;

            var attr = Shader.GetAttributeByName("texCoord");
            if (attr == null)
                return false;

            // Update UV coordinates
            var pos = attr.Offset / sizeof(float);
            var modified = false;
            foreach (var quad in Quads) {
                var textureAnim = quad.TextureAnim;
                if (textureAnim == null)
                    continue;

                var frame = textureAnim.GetFrame(_frame);
                var texCoords = _textureAtlas.GetUVCoordinatesByTextureIDFrame(
                    frame.ID, frame.Frame, _textureBitmap.Width, _textureBitmap.Height, quad.TextureFlags);

                for (var vertexIndex = 0; vertexIndex < 4; vertexIndex++) {
                    if (!modified && (_vertexBuffer[pos] != texCoords[vertexIndex].X || _vertexBuffer[pos + 1] != texCoords[vertexIndex].Y))
                        modified = true;
                    _vertexBuffer[pos + 0] = texCoords[vertexIndex].X;
                    _vertexBuffer[pos + 1] = texCoords[vertexIndex].Y;
                    pos += Shader.VertexBufferStride / sizeof(float);
                }
            }

            if (modified) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertexBuffer.Length * sizeof(float), _vertexBuffer, BufferUsageHint.DynamicDraw);
            }

            return modified;
        }

        public void Draw() {
            _texture?.Use();
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _elementBuffer.Length, DrawElementsType.UnsignedInt, 0);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                _textureAtlas?.Dispose();
                _texture?.Dispose();
                _textureBitmap?.Dispose();
                GL.DeleteBuffer(_elementBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteBuffer(_vertexBufferObject);
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QuadModel() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        public Quad[] Quads { get; }
        public Shader Shader { get; }

        public TextureAtlas _textureAtlas { get; }
        public Bitmap _textureBitmap { get; }
        public Texture _texture { get; }

        private readonly float[] _vertexBuffer;
        private readonly uint[] _elementBuffer;

        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;
        private readonly int _vertexArrayObject;
    }
}