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
                CreateGroundImageModel(mpdFile, mpdFile.RepeatingGroundImage.FullTexture, 65536.0f);
            else if (mpdFile?.TiledGroundImage != null)
                CreateGroundImageModel(mpdFile, mpdFile.TiledGroundImage, 64.0f);
        }

        private void CreateGroundImageModel(IMPD_File mpdFile, ITexture texture, float size) {
            Texture = new Texture(texture.CreateBitmapARGB8888(), clampToEdge: false);

            var header = mpdFile.MPDHeader[0];
            var position = new Vector3(header.GroundX / 32.0f, header.GroundY / -32.0f, header.GroundZ / -32.0f);

            var uvWidth  = size / (Texture.Width  / 32.0f);
            var uvHeight = size / (Texture.Height / 32.0f);

            var theta = (header.GroundAngle - 0.25f) * (float) Math.PI * 2.0f;
            var sin = (float) Math.Sin(theta);
            var cos = (float) Math.Cos(theta);

            var offsetXZ = size / -2.0f;
            var texTileWidth  = texture.Width / 32.0f;
            var texTileHeight = texture.Height / 32.0f;
            var uvOffsetX = (size - texTileWidth)  / -texTileWidth  / 2.0f;
            var uvOffsetY = (size - texTileHeight) / -texTileHeight / 2.0f;

            var vertices = Enum.GetValues<CornerType>()
                .Select(c => {
                    var x = c.GetDirectionX() * offsetXZ;
                    var z = c.GetDirectionZ() * offsetXZ;
                    return new Vector3(
                        position.X + cos * x - sin * z,
                        position.Y,
                        position.Z + sin * x + cos * z
                    );
                })
                .ToArray();

            var uvCoords = Enum.GetValues<CornerType>()
                .SelectMany(x => new float[] {
                    x.GetUVX() * uvWidth  + uvOffsetX,
                    x.GetUVY() * uvHeight + uvOffsetY
                })
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
