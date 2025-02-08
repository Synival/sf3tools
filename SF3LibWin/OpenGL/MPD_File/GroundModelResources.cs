using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class GroundModelResources : IDisposable {
        public const float c_groundXZ = -32768;
        public const float c_groundWidthDepth = c_groundXZ * -2;

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                Reset();

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GroundModelResources() {
            if (!disposed)
                System.Diagnostics.Debug.WriteLine(GetType().Name + ": GPU Resource leak! Did you forget to call Dispose()?");
            Dispose(false);
        }

        private bool _isInitialized = false;
        public void Init() {
            if (_isInitialized)
                return;
            _isInitialized = true;

            // TODO: nothing to initialize yet!
        }

        public void Reset() {
            Model?.Dispose();
            Texture?.Dispose();

            Model = null;
            Texture = null;
        }

        public void Update(IMPD_File mpdFile) {
            Reset();
            if (mpdFile?.RepeatingGroundImage != null)
                CreateRepeatingGroundImageModel(mpdFile);
        }

        private void CreateRepeatingGroundImageModel(IMPD_File mpdFile) {
            Texture = new Texture(mpdFile.RepeatingGroundImage.FullTexture.CreateBitmapARGB8888(), clampToEdge: false);

            var header = mpdFile.MPDHeader[0];
            var position = new Vector3(header.GroundX - 32.0f, header.GroundY / -32.0f, header.GroundZ / -32.0f);

            var uvWidth  = c_groundWidthDepth / (Texture.Width  / 32.0f);
            var uvHeight = c_groundWidthDepth / (Texture.Height / 32.0f);

            var theta = (header.GroundAngle - 0.25f) * (float) Math.PI * 2.0f;
            var sin = (float) Math.Sin(theta);
            var cos = (float) Math.Cos(theta);

            var vertices = Enum.GetValues<CornerType>()
                .Select(c => {
                    var x = c.GetDirectionX() * c_groundXZ;
                    var z = c.GetDirectionZ() * c_groundXZ;
                    return new Vector3(
                        position.X + cos * x - sin * z,
                        position.Y,
                        position.Z + sin * x + cos * z
                    );
                })
                .ToArray();

            var uvCoords = Enum.GetValues<CornerType>()
                .SelectMany(x => new float[] { (x.GetUVX() - 0.5f) * uvWidth, (x.GetUVY() - 0.5f) * uvHeight })
                .ToArray()
                .To2DArray(4, 2);

            var quad = new Quad(vertices);
            var texInfo = Shader.GetTextureInfo(TextureUnit.Texture0);
            quad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, uvCoords));

            Model = new QuadModel([quad]);
        }

        public QuadModel Model { get; private set; }
        public Texture Texture { get; private set; }
    }
}
