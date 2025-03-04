using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class GroundModelResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Model?.Dispose();
            Texture?.Dispose();

            Model = null;
            Texture = null;
        }

        public void Update(IMPD_File mpdFile) {
            Reset();
            if (mpdFile?.RepeatingGroundImage != null)
                CreateGroundImageModel(mpdFile, mpdFile.RepeatingGroundImage, 65536.0f);
            else if (mpdFile?.TiledGroundImage != null)
                CreateGroundImageModel(mpdFile, mpdFile.TiledGroundImage, 128.0f);
        }

        private static readonly float[,] c_normalCoordsVboData = new float[4, 3] {
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 0},
        };

        private static readonly float[,] c_glowVboData = new float[4, 3] {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0},
        };

        private void CreateGroundImageModel(IMPD_File mpdFile, ITexture texture, float size) {
            Texture = new Texture(texture.CreateBitmapARGB8888(), clampToEdge: false);

            var header = mpdFile.MPDHeader;

            var position = new Vector3(
                header.GroundX / 32.0f,
                header.GroundY / -32.0f,
                header.GroundZ / -32.0f
            );

            // A lot of maps like MUCHUR.MPD and BEER.MPD have some pretty stupid offsets for their ground planes.
            // Put the ground plane into the most ideal location based on the camera boundaries.
            MoveToMostIdealCameraBoundaries(mpdFile, ref position);

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

            var uvCoordsVboData = Enum.GetValues<CornerType>()
                .SelectMany(x => new float[] {
                    x.GetUVX() * uvWidth  + uvOffsetX,
                    x.GetUVY() * uvHeight + uvOffsetY
                })
                .ToArray()
                .To2DArray(4, 2);

            var quad = new Quad(vertices);
            var texInfo = Shader.GetTextureInfo(TextureUnit.Texture0);
            quad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, uvCoordsVboData));

            Model = new QuadModel([quad]);
        }

        private void MoveToMostIdealCameraBoundaries(IMPD_File mpdFile, ref Vector3 position) {
            if (mpdFile.BoundariesTable?.Length == 0)
                return;
            var cameraBoundaries = mpdFile.BoundariesTable[0];

            float centerX = 0.0f;
            float centerZ = 0.0f;
            try {
                centerX = (cameraBoundaries.X1 + cameraBoundaries.X2) / 2.0f / 32.0f - 32.0f;
                centerZ = -((cameraBoundaries.Y1 + cameraBoundaries.Y2) / 2.0f / 32.0f - 32.0f);
            }
            catch {
                // TODO: some error when reading camera bounds. What to do here???
            }

            void MoveCoordNearestToCameraBounds(ref float positionCoord, float cameraCoord) {
                // Move positionCoord to a +(0, 63) offset to cameraCoord.
                while (positionCoord < cameraCoord)
                    positionCoord += 64.0f;
                while (positionCoord > cameraCoord + 64.0f)
                    positionCoord -= 64.0f;

                // Move to a negative position if it's closer than the positive one.
                if (cameraCoord - (positionCoord - 64.0f) < positionCoord - cameraCoord)
                    positionCoord -= 64.0f;
            }

            MoveCoordNearestToCameraBounds(ref position.X, centerX);
            MoveCoordNearestToCameraBounds(ref position.Z, centerZ);
        }

        public QuadModel Model { get; private set; }
        public Texture Texture { get; private set; }
    }
}
