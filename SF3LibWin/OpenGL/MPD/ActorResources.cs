using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Win.App;
using SF3.Win.Properties;

namespace SF3.Win.OpenGL.MPD {
    public class ActorResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() {
            Textures = [
                (UnknownSpriteTexture = new Texture(Resources.UnknownSpriteBmp)),
            ];
        }

        public override void DeInit() {
            UnknownSpriteTexture?.Dispose();
        }

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

            if (ActorsBySpriteID != null) {
                ActorsBySpriteID.Clear();
                ActorsBySpriteID = null;
            }
        }

        private static readonly Vector3[] s_vertexData = [
            new Vector3( 0.5f,  0.0f,  0.0f),
            new Vector3( 0.5f,  1.0f,  0.0f),
            new Vector3(-0.5f,  1.0f,  0.0f),
            new Vector3(-0.5f,  0.0f,  0.0f),
        ];

        private static readonly Vector3[] s_shadowVertexData = [
            new Vector3( 0.25f,  0.0f,  0.25f),
            new Vector3( 0.25f,  0.0f, -0.25f),
            new Vector3(-0.25f,  0.0f, -0.25f),
            new Vector3(-0.25f,  0.0f,  0.25f),
        ];

        private static readonly float[,] s_texCoords = {
            { 1.0f, 1.0f },
            { 1.0f, 0.0f },
            { 0.0f, 0.0f },
            { 0.0f, 1.0f },
        };

        private readonly Vector4 _enemyColor    = new(1, 0, 0, 1);
        private readonly Vector4 _friendlyColor = new(0, 1, 0, 1);

        public void Update(IMPD mpdFile) {
            Reset();

            var currentActorCollection = AppResources.Get().ActiveActorCollection;
            if (currentActorCollection == null)
                return;

            var texInfo = Shader.GetTextureInfo(TextureUnit.Texture0);
            var actorsGrouped = currentActorCollection.Actors.OrderBy(x => x.SpriteID).GroupBy(x => x.SpriteID).ToArray();

            ModelsBySpriteID  = new Dictionary<int, QuadModel>();
            ShadowsBySpriteID = new Dictionary<int, QuadModel>();
            ActorsBySpriteID  = new Dictionary<int, ActorModelInstance[]>();

            foreach (var actors in actorsGrouped) {
                var spriteId = actors.Key;

                var spriteQuad = new Quad(s_vertexData, (spriteId < 0xC8 || spriteId > 0x185) ? _friendlyColor : _enemyColor);
                spriteQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, s_texCoords));

                var shadowQuad = new Quad(s_shadowVertexData, new Vector4(0, 0, 0, 1));
                shadowQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.FloatVec2, texInfo.TexCoordName, 4, s_texCoords));

                ModelsBySpriteID[spriteId] = new QuadModel([spriteQuad]);
                ShadowsBySpriteID[spriteId] = new QuadModel([shadowQuad]);

                ActorsBySpriteID[spriteId] = actors
                    .Select(x => {
                        var actorX = x.ActorX;
                        var actorZ = x.ActorZ;

                        return new ActorModelInstance() {
                            X = actorX /  32.0f + GeneralResources.ModelOffsetX,
                            Y = (mpdFile?.Surface?.GetHeightAt(actorX, actorZ) ?? 0) / 16.0f,
                            Z = actorZ / -32.0f - GeneralResources.ModelOffsetZ
                        };
                    })
                    .ToArray();
            }
        }

        public struct ActorModelInstance {
            public float X, Y, Z;
        }

        public Texture UnknownSpriteTexture { get; private set; }
        public Texture[] Textures { get; private set; }

        public Dictionary<int, ActorModelInstance[]> ActorsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ModelsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ShadowsBySpriteID { get; private set; } = null;
    }
}
