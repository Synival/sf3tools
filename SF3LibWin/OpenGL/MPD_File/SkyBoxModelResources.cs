using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;
using static CommonLib.Types.CornerTypeConsts;
using static SF3.Win.OpenGL.Shader;

namespace SF3.Win.OpenGL.MPD_File {
    public class SkyBoxModelResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Model?.Dispose();
            Texture?.Dispose();

            Model = null;
            Texture = null;
        }

        private static readonly Vector3[] c_skyBoxCoords = [
            new Vector3(Corner1DirX * 2, Corner1DirY * 2, 0),
            new Vector3(Corner2DirX * 2, Corner2DirY * 2, 0),
            new Vector3(Corner3DirX * 2, Corner3DirY * 2, 0),
            new Vector3(Corner4DirX * 2, Corner4DirY * 2, 0),
        ];

        private static readonly float[,] c_skyBoxUvCoords = new float[,] {
            { Corner1UVX * 2, Corner1UVY * 2 },
            { Corner2UVX * 2, Corner2UVY * 2 },
            { Corner3UVX * 2, Corner3UVY * 2 },
            { Corner4UVX * 2, Corner4UVY * 2 }
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
