using System;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Logging;
using CommonLib.Types;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_GroundModelResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }
        public override void DeInit() { }

        public override void Reset() {
            Model?.Dispose();
            Texture?.Dispose();

            Model = null;
            Texture = null;
        }

        public void Update(IMPD mpdFile) {
            Reset();
            if (mpdFile?.Planes?.GroundImage != null && mpdFile.Flags.Bit_0x0400_HasGroundImage) {
                try {
                    CreateGroundImageModel(mpdFile, mpdFile.Planes.GroundImage, 65536.0f * 8.0f);
                }
                catch (Exception e) {
                    Logger.LogException(e);
                }
            }
            else if (mpdFile?.Planes?.GroundTiledImage != null && mpdFile.Flags.Bit_0x1000_HasTileBasedGroundImage) {
                try {
                    CreateGroundImageModel(mpdFile, mpdFile.Planes.GroundTiledImage.TiledImage, 4096.0f);
                }
                catch (Exception e) {
                    Logger.LogException(e);
                }
            }
        }

        private void CreateGroundImageModel(IMPD mpdFile, ITextureData texture, float size) {
            Texture = new Texture(texture.CreateBitmapARGB8888(), clampToEdge: false);

            var planes = mpdFile.Planes;

            var position = new Vector3(
                planes.GroundX,
                -planes.GroundY,
                -planes.GroundZ
            );

            // A lot of maps like MUCHUR.MPD and BEER.MPD have some pretty stupid offsets for their ground planes.
            // Put the ground plane into the most ideal location based on the camera boundaries.
            MoveToMostIdealCameraBoundaries(mpdFile, ref position);

            var uvWidth  = size / Texture.Width;
            var uvHeight = size / Texture.Height;

            // TODO: *X-axis* rotation, not *Y-axis* rotation!
            var theta = Math.PI;// settings.GroundAngle * (float) Math.PI / 180.0f;
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

        private void MoveToMostIdealCameraBoundaries(IMPD mpdFile, ref Vector3 position) {
            var cameraBoundaries = mpdFile.CameraBoundaries;
            if (cameraBoundaries == null)
                return;

            var centerX = 0.0f;
            var centerZ = 0.0f;
            try {
                centerX =   cameraBoundaries.Width  / 2.0f - 1024.0f;
                centerZ = -(cameraBoundaries.Height / 2.0f - 1024.0f);
            }
            catch {
                // TODO: some error when reading camera bounds. What to do here???
            }

            void MoveCoordNearestToCameraBounds(ref float positionCoord, float cameraCoord) {
                // Move positionCoord to a +(0, 63) offset to cameraCoord.
                while (positionCoord < cameraCoord)
                    positionCoord += 2048.0f;
                while (positionCoord > cameraCoord + 2048.0f)
                    positionCoord -= 2048.0f;

                // Move to a negative position if it's closer than the positive one.
                if (cameraCoord - (positionCoord - 2048.0f) < positionCoord - cameraCoord)
                    positionCoord -= 2048.0f;
            }

            MoveCoordNearestToCameraBounds(ref position.X, centerX);
            MoveCoordNearestToCameraBounds(ref position.Z, centerZ);
        }

        public QuadModel Model { get; private set; }
        public Texture Texture { get; private set; }
    }
}
