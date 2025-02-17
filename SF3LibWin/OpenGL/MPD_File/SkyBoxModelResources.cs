using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using static CommonLib.Types.CornerTypeConsts;
using static SF3.Win.OpenGL.Shader;

namespace SF3.Win.OpenGL.MPD_File {
    public class SkyBoxModelResources : IDisposable {
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

        ~SkyBoxModelResources() {
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

        private static readonly Vector3[] c_skyBoxCoords = [
            new Vector3(Corner1DirX * 2, Corner1DirY, 0),
            new Vector3(Corner2DirX * 2, Corner2DirY, 0),
            new Vector3(Corner3DirX * 2, Corner3DirY, 0),
            new Vector3(Corner4DirX * 2, Corner4DirY, 0),
        ];

        private static readonly float[,] c_skyBoxUvCoords = new float[,] {
            { Corner1UVX * 2 - 0.5f, Corner1UVY },
            { Corner2UVX * 2 - 0.5f, Corner2UVY },
            { Corner3UVX * 2 - 0.5f, Corner3UVY },
            { Corner4UVX * 2 - 0.5f, Corner4UVY }
        };

        public void Update(IMPD_File mpdFile) {
            Reset();
            if (mpdFile?.SkyBoxImage == null)
                return;

            Texture = new Texture(mpdFile.SkyBoxImage.CreateBitmapARGB8888(), clampToEdge: false);

            var quad = new Quad(c_skyBoxCoords);
            var texInfo = GetTextureInfo(TextureUnit.Texture0);
            quad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, c_skyBoxUvCoords));

            Model = new QuadModel([quad]);
        }

        public QuadModel Model { get; private set; }
        public Texture Texture { get; private set; }
    }
}
