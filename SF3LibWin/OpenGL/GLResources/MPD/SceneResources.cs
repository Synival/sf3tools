using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.App;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class SceneResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() {}
        public override void DeInit() {}

        public override void Reset() {
            ResetActorSprites();
            ResetZones();
        }

        public void ResetActorSprites() {
            if (ModelsBySpriteID != null) {
                foreach (var model in ModelsBySpriteID)
                    model.Value.Dispose();
                ModelsBySpriteID.Clear();
                ModelsBySpriteID = null;
            }

            if (ShadowsBySpriteID != null) {
                foreach (var shadow in ShadowsBySpriteID)
                    shadow.Value.Dispose();
                ShadowsBySpriteID.Clear();
                ShadowsBySpriteID = null;
            }

            ActorsBySpriteID?.Clear();
            ActorsBySpriteID = null;

            ActorTextureAtlas?.Dispose();
            ActorTextureAtlas = null;

            TexInfoBySpriteID?.Clear();
            TexInfoBySpriteID = null;
        }

        public void ResetZones() {
            ZoneModels?.Clear();
            ZoneModels = null;
        }

        private static readonly Vector3[] c_spriteVertexData = [
            new Vector3( 0.5f,  0.0f,  0.0f),
            new Vector3( 0.5f,  1.0f,  0.0f),
            new Vector3(-0.5f,  1.0f,  0.0f),
            new Vector3(-0.5f,  0.0f,  0.0f),
        ];

        private static readonly Vector3[] c_shadowVertexData = [
            new Vector3( 0.25f,  0.0f,  0.25f),
            new Vector3( 0.25f,  0.0f, -0.25f),
            new Vector3(-0.25f,  0.0f, -0.25f),
            new Vector3(-0.25f,  0.0f,  0.25f),
        ];

        private static readonly float[,] c_isRightVertex = { {1}, {1}, {0}, {0} };

        private static readonly Vector4 c_enemyColor    = new(1, 0, 0, 1);
        private static readonly Vector4 c_friendlyColor = new(0, 1, 0, 1);
        private static readonly Vector4 c_white         = new(1, 1, 1, 1);

        public void Update(IMPD mpdFile) {
            UpdateActors(mpdFile);
            UpdateZones(mpdFile);
        }

        public void UpdateActors(IMPD mpdFile) {
            ResetActorSprites();

            var currentScene = AppResources.Get().ActiveScene?.Scene;
            if (currentScene == null)
                return;

            var texInfo = Shader.GetTextureInfo(TextureUnit.Texture0);
            var actorsGrouped = currentScene.Actors
                .Take(currentScene.NumActors)
                .OrderBy(x => x.SpriteID)
                .GroupBy(x => x.SpriteID)
                .ToDictionary(x => x.Key, x => x.ToArray());

            var spriteIds = actorsGrouped.Select(x => x.Key).ToArray();
            BuildActorTextureAtlas(spriteIds);

            ModelsBySpriteID  = new Dictionary<int, QuadModel>();
            ShadowsBySpriteID = new Dictionary<int, QuadModel>();
            ActorsBySpriteID  = new Dictionary<int, ActorModelInstance[]>();

            var unknownTexInfo = TexInfoBySpriteID[-1];
            var shadowTexCoords = new float[,] {
                { unknownTexInfo.U + unknownTexInfo.Width, unknownTexInfo.V + unknownTexInfo.Height },
                { unknownTexInfo.U + unknownTexInfo.Width, unknownTexInfo.V                         },
                { unknownTexInfo.U,                        unknownTexInfo.V                         },
                { unknownTexInfo.U,                        unknownTexInfo.V + unknownTexInfo.Height },
            };

            foreach (var actors in actorsGrouped) {
                var spriteId = actors.Key;
                var spriteTexInfo = TexInfoBySpriteID.TryGetValue(spriteId, out var texInfoOut) ? texInfoOut : TexInfoBySpriteID[-1];

                var spriteQuad = new Quad(c_spriteVertexData
                    .Select(x => new Vector3(
                        x.X * spriteTexInfo.ScaleWidth,
                        x.Y * spriteTexInfo.ScaleHeight,
                        x.Z * spriteTexInfo.ScaleWidth
                    )).ToArray(),
                    c_white
                );

                // ('U' component offset by width is calculated in the shader depending on flip.)
                var spriteTexCoords = new float[,] {
                    { spriteTexInfo.U, spriteTexInfo.V + spriteTexInfo.Height },
                    { spriteTexInfo.U, spriteTexInfo.V                        },
                    { spriteTexInfo.U, spriteTexInfo.V                        },
                    { spriteTexInfo.U, spriteTexInfo.V + spriteTexInfo.Height },
                };

                {
                    var w = spriteTexInfo.Width;
                    var f = spriteTexInfo.Directions.IsFlippable() ? 1.0f : 0.0f;
                    var d = spriteTexInfo.Directions.GetAnimationFrameCount() * (f + 1.0f);

                    spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, spriteTexCoords));
                    spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "isRightVertex", 4, c_isRightVertex));
                    spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "width",         4, new float[,] { {w}, {w}, {w}, {w} }));
                    spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "directions",    4, new float[,] { {d}, {d}, {d}, {d} }));
                    spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "isFlippable",   4, new float[,] { {f}, {f}, {f}, {f} }));
                }

                var shadowQuad = new Quad(c_shadowVertexData.Select(x => x * spriteTexInfo.CollisionShadowSize).ToArray(), new Vector4(0, 0, 0, 1));
                shadowQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, shadowTexCoords));

                ModelsBySpriteID[spriteId] = new QuadModel([spriteQuad]);
                ShadowsBySpriteID[spriteId] = new QuadModel([shadowQuad]);

                ActorsBySpriteID[spriteId] = actors.Value
                    .Select(x => {
                        var actorX = x.ActorX;
                        var actorZ = x.ActorZ;

                        return new ActorModelInstance(
                            id: x.ID,
                            spriteId: x.SpriteID,
                            x: actorX /  32.0f + ModelResources.ModelOffsetX,
                            y: (mpdFile?.Surface?.GetHeightAt(actorX, actorZ) ?? 0) / 16.0f,
                            z: actorZ / -32.0f - ModelResources.ModelOffsetZ,
                            verticalOffset: spriteTexInfo.VerticalOffset,
                            x.ActorDirection
                        );
                    })
                    .ToArray();
            }
        }

        private readonly Vector4[] c_zoneColors = [
            new Vector4(1.0f, 1.0f, 1.0f, 0.25f),
            new Vector4(1.0f, 0.5f, 0.5f, 0.25f),
            new Vector4(1.0f, 1.0f, 0.5f, 0.25f),
            new Vector4(0.5f, 1.0f, 0.5f, 0.25f),
            new Vector4(0.5f, 1.0f, 1.0f, 0.25f),
            new Vector4(0.5f, 0.5f, 1.0f, 0.25f),
            new Vector4(1.0f, 0.5f, 1.0f, 0.25f)
        ];

        // NOTE: All of this is just a rough expression that hasn't been thought out!
        // This should probably be overlayed on tiles or something.
        public void UpdateZones(IMPD mpdFile) {
            ResetZones();

            var currentScene = AppResources.Get().ActiveScene?.Scene;
            if (currentScene == null || currentScene.NumZones == 0 || currentScene.Zones == null)
                return;

            var zoneY = (mpdFile.Planes?.GroundY ?? 0) / -32.0f - 0.05f;
            Vector3 ZoneVertex(int x, int z)
                => new Vector3(0.5f + x + ModelResources.ModelOffsetX, zoneY, 63.5f - z + ModelResources.ModelOffsetZ);

            var newZones = new List<QuadModel>();

            foreach (var zone in currentScene.Zones) {
                if (zone.ID >= currentScene.NumZones)
                    break;

                var point1 = ZoneVertex(zone.X1, zone.Z1);
                var point2 = ZoneVertex(zone.X2, zone.Z2);
                var point3 = ZoneVertex(zone.X3, zone.Z3);
                var point4 = ZoneVertex(zone.X4, zone.Z4);

                if (zone.NumPoints == 3)
                    newZones.Add(new QuadModel([new Quad([point1, point2, point3, point3], c_zoneColors[zone.ID % 7])]));
                else if (zone.NumPoints == 4)
                    newZones.Add(new QuadModel([new Quad([point1, point2, point3, point4], c_zoneColors[zone.ID % 7])]));
            }

            ZoneModels = newZones;
        }

        private void BuildActorTextureAtlas(ushort[] spriteIds) {
            // Always include the 'unknown sprite' image.
            var unknownImage = Resources.UnknownSpriteBmp;

            var texBuffers = new List<uint[,]> {
                unknownImage.GetBitmapDataARGB8888().ToUInts().To2DArrayColumnMajor(unknownImage.Width, unknownImage.Height)
            };

            TexInfoBySpriteID = new Dictionary<int, SpriteTexInfo>() {
                { -1, new SpriteTexInfo(0, 0, unknownImage.Width, unknownImage.Height, SpriteDirectionCountType.OneNoFlip, 1, 1, 1, 0.1f) }
            };

            // Build a texture atlas with all frames.
            var sprites = AppResources.Get().ActiveCHR?.CHR?.SpriteTable;
            var offsetY = unknownImage.Height;
            if (sprites != null) {
                foreach (var spriteId in spriteIds) {
                    var sprite = sprites.FirstOrDefault(x => x.Header.SpriteID == spriteId);
                    if (sprite == null || !(sprite.AnimationTable?.Count >= 1))
                        continue;

                    // Use the idle animation if it exists, otherwise fall back on the still frame.
                    var anim = sprite.AnimationTable.Count >= 2 ? sprite.AnimationTable[1] :
                               sprite.AnimationTable.Count >= 1 ? sprite.AnimationTable[0] : null;
                    if (anim == null)
                        continue;

                    // Get the first frame command for the animation.
                    var aniCommands = anim.AnimationCommandTable;
                    var aniCommand = aniCommands.FirstOrDefault(x => x.IsFrameCommand);
                    if (aniCommand == null)
                        continue;

                    // Get the frame used for that command.
                    var firstFrameIndex = aniCommand.Command;
                    if (firstFrameIndex >= sprite.FrameTable.Count)
                        continue;

                    int frameWidth  = sprite.Header.Width;
                    int frameHeight = sprite.Header.Height;
                    var dirCount    = aniCommand.Directions.GetAnimationFrameCount();
                    var firstFrame  = sprite.FrameTable[firstFrameIndex];
                    var texBuf      = new uint[frameWidth * dirCount, frameHeight];

                    // Add the frames!
                    var offsetX = 0;
                    for (var i = 0; i < dirCount; i++) {
                        var frameIndex = firstFrameIndex + i;
                        var frame = frameIndex < sprite.FrameTable.Count ? sprite.FrameTable[frameIndex] : firstFrame;
                        var frameImageData = frame.Texture.GetBitmapDataARGB8888().ToUInts().To2DArrayColumnMajor(frameWidth, frameHeight);

                        for (var y = 0; y < frameHeight; y++)
                            for (var x = 0; x < frameWidth; x++)
                                texBuf[x + offsetX, y] = frameImageData[x, y];

                        offsetX += frameWidth;
                    }

                    texBuffers.Add(texBuf);

                    TexInfoBySpriteID[spriteId] = new SpriteTexInfo(0, offsetY, frameWidth, frameHeight, aniCommand.Directions,
                        frameWidth  / 32.0f * sprite.Header.Scale / 0x10000,
                        frameHeight / 32.0f * sprite.Header.Scale / 0x10000,
                        sprite.Header.CollisionShadowDiameter / 32.0f,
                        sprite.Header.VerticalOffset / -32.0f
                    );

                    offsetY += frameHeight;
                }
            }

            var width  = texBuffers.Max(x => x.GetLength(0));
            var height = texBuffers.Sum(x => x.GetLength(1));

            foreach (var texInfo in TexInfoBySpriteID.Values) {
                texInfo.U /= width;
                texInfo.V /= height;
                texInfo.Width /= width;
                texInfo.Height /= height;
            }

            var textureData = new byte[width * height * 4];
            offsetY = 0;
            foreach (var texBuf in texBuffers) {
                var bufWidth  = texBuf.GetLength(0);
                var bufHeight = texBuf.GetLength(1);

                for (var y = 0; y < bufHeight; y++, offsetY++) {
                    var offset = offsetY * width * 4;
                    for (var x = 0; x < bufWidth; x++) {
                        var value = texBuf[x, y];
                        textureData[offset++] = (byte) (value >> 24);
                        textureData[offset++] = (byte) (value >> 16);
                        textureData[offset++] = (byte) (value >> 8);
                        textureData[offset++] = (byte) (value >> 0);
                    }
                }
            }

            ActorTextureAtlas = new Texture(width, height, PixelInternalFormat.Rgba, PixelFormat.Bgra, PixelType.UnsignedByte, imageData: textureData);
        }

        public class ActorModelInstance {
            public ActorModelInstance(int id, int spriteId, float x, float y, float z, float verticalOffset, float direction) {
                ID = id;
                SpriteID = spriteId;
                X = x;
                Y = y;
                Z = z;
                VerticalOffset = verticalOffset;
                Direction = direction;
            }

            public int ID;
            public int SpriteID;
            public readonly float X, Y, Z;
            public readonly float VerticalOffset;
            public readonly float Direction;
        }

        public class SpriteTexInfo {
            public SpriteTexInfo(float u, float v, float width, float height, SpriteDirectionCountType directions, float scaleWidth, float scaleHeight, float collision, float verticalOffset) {
                U = u;
                V = v;
                Width  = width;
                Height = height;
                Directions = directions;
                ScaleWidth  = scaleWidth;
                ScaleHeight = scaleHeight;
                CollisionShadowSize = collision;
                VerticalOffset = verticalOffset;
            }

            public float U, V;
            public float Width, Height;
            public SpriteDirectionCountType Directions;
            public float ScaleWidth, ScaleHeight, CollisionShadowSize;
            public float VerticalOffset;
        }

        public Texture ActorTextureAtlas { get; private set; }
        private Dictionary<int, SpriteTexInfo> TexInfoBySpriteID = null;

        public Dictionary<int, ActorModelInstance[]> ActorsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ModelsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ShadowsBySpriteID { get; private set; } = null;

        public List<QuadModel> ZoneModels { get; private set; } = null;
    }
}
