using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using CommonLib.Extensions;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.App;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD {
    public class ActorResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() {}
        public override void DeInit() {}

        public override void Reset() {
            ResetActorSprites();
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

            Texture?.Dispose();
            Texture = null;

            TexInfoBySpriteID?.Clear();
            TexInfoBySpriteID = null;
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
            Reset();

            var currentActorCollection = AppResources.Get().ActiveActorCollection;
            if (currentActorCollection == null)
                return;

            var texInfo = Shader.GetTextureInfo(TextureUnit.Texture0);
            var actorsGrouped = currentActorCollection.Actors.OrderBy(x => x.SpriteID).GroupBy(x => x.SpriteID).ToArray();

            var spriteIds = actorsGrouped.Select(x => x.Key).ToArray();
            BuildTexture(spriteIds);

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

                ActorsBySpriteID[spriteId] = actors
                    .Select(x => {
                        var actorX = x.ActorX;
                        var actorZ = x.ActorZ;

                        return new ActorModelInstance(
                            id: x.ID,
                            spriteId: x.SpriteID,
                            x: actorX /  32.0f + GeneralResources.ModelOffsetX,
                            y: (mpdFile?.Surface?.GetHeightAt(actorX, actorZ) ?? 0) / 16.0f,
                            z: actorZ / -32.0f - GeneralResources.ModelOffsetZ,
                            verticalOffset: spriteTexInfo.VerticalOffset,
                            x.ActorDirection
                        );
                    })
                    .ToArray();
            }
        }

        private void BuildTexture(int[] spriteIds) {
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
            int offsetY = unknownImage.Height;
            if (sprites != null) {
                foreach (var spriteId in spriteIds) {
                    var sprite = sprites.FirstOrDefault(x => x.Header.SpriteID == spriteId);
                    if (sprite == null || !(sprite.AnimationTable?.Length >= 1))
                        continue;

                    // Use the idle animation if it exists, otherwise fall back on the still frame.
                    var anim = (sprite.AnimationTable.Length >= 2) ? sprite.AnimationTable[1] :
                               (sprite.AnimationTable.Length >= 1) ? sprite.AnimationTable[0] : null;
                    if (anim == null)
                        continue;

                    // Get the first frame command for the animation.
                    var aniCommands = anim.AnimationCommandTable;
                    var aniCommand = aniCommands.FirstOrDefault(x => x.IsFrameCommand);
                    if (aniCommand == null)
                        continue;

                    // Get the frame used for that command.
                    var firstFrameIndex = aniCommand.Command;
                    if (firstFrameIndex >= sprite.FrameTable.Length)
                        continue;

                    int frameWidth  = sprite.Header.Width;
                    int frameHeight = sprite.Header.Height;
                    var dirCount    = aniCommand.Directions.GetAnimationFrameCount();
                    var firstFrame  = sprite.FrameTable[firstFrameIndex];
                    var texBuf      = new uint[frameWidth * dirCount, frameHeight];

                    // Add the frames!
                    int offsetX = 0;
                    for (int i = 0; i < dirCount; i++) {
                        var frameIndex = firstFrameIndex + i;
                        var frame = (frameIndex < sprite.FrameTable.Length) ? sprite.FrameTable[frameIndex] : firstFrame;
                        var frameImageData = frame.Texture.GetBitmapDataARGB8888().ToUInts().To2DArrayColumnMajor(frameWidth, frameHeight);

                        for (int y = 0; y < frameHeight; y++)
                            for (int x = 0; x < frameWidth; x++)
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

                for (int y = 0; y < bufHeight; y++, offsetY++) {
                    int offset = offsetY * width * 4;
                    for (int x = 0; x < bufWidth; x++) {
                        var value = texBuf[x, y];
                        textureData[offset++] = (byte) (value >> 24);
                        textureData[offset++] = (byte) (value >> 16);
                        textureData[offset++] = (byte) (value >> 8);
                        textureData[offset++] = (byte) (value >> 0);
                    }
                }
            }

            Texture = new Texture(width, height, PixelInternalFormat.Rgba, PixelFormat.Bgra, PixelType.UnsignedByte, imageData: textureData);
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

        public Texture Texture { get; private set; }
        private Dictionary<int, SpriteTexInfo> TexInfoBySpriteID = null;

        public Dictionary<int, ActorModelInstance[]> ActorsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ModelsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ShadowsBySpriteID { get; private set; } = null;
    }
}
