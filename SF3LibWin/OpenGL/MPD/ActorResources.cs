using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Win.App;

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

            if (ActorsBySpriteID != null) {
                ActorsBySpriteID.Clear();
                ActorsBySpriteID = null;
            }
        }

        private readonly float[,] _applyLightingVboData = new float[,] {{0}, {0}, {0}, {0}};

        private readonly Vector3[] _vertexData = [
            new Vector3( 0.4f,  0.0f,  0.0f),
            new Vector3( 0.4f,  0.8f,  0.0f),
            new Vector3(-0.4f,  0.8f,  0.0f),
            new Vector3(-0.4f,  0.0f,  0.0f),
        ];

        private readonly Vector4 _enemyColor    = new(1, 0, 0, 1);
        private readonly Vector4 _friendlyColor = new(0, 1, 0, 1);

        public void Update(IMPD mpdFile) {
            Reset();

            var currentActorCollection = AppResources.Get().ActiveActorCollection;
            if (currentActorCollection == null)
                return;

            var actorsGrouped = currentActorCollection.Actors.OrderBy(x => x.SpriteID).GroupBy(x => x.SpriteID).ToArray();
            ModelsBySpriteID = new Dictionary<int, QuadModel>();
            ActorsBySpriteID = new Dictionary<int, ActorModelInstance[]>();

            foreach (var actors in actorsGrouped) {
                var spriteId = actors.Key;
                var newQuad = new Quad(_vertexData, (spriteId < 0xC8 || spriteId > 0x185) ? _friendlyColor : _enemyColor);
                newQuad.AddAttribute(new PolyAttribute(1, ActiveAttribType.Float, "applyLighting", 4, _applyLightingVboData));
                ModelsBySpriteID[spriteId] = new QuadModel([newQuad]);
                ActorsBySpriteID[spriteId] = actors
                    .Select(x => {
                        var actorX = x.ActorX;
                        var actorY = x.ActorY;
                        var actorZ = x.ActorZ;

                        var surfaceY = (int) mpdFile?.Surface?.GetHeightAt(actorX, actorZ) * 2;

                        return new ActorModelInstance() {
                            X = actorX /  32.0f + GeneralResources.ModelOffsetX,
                            Y = Math.Max(-actorY, surfaceY) / 32.0f,
                            Z = actorZ / -32.0f - GeneralResources.ModelOffsetZ
                        };
                    })
                    .ToArray();
            }
        }

        public struct ActorModelInstance {
            public float X, Y, Z;
        }

        public Dictionary<int, ActorModelInstance[]> ActorsBySpriteID { get; private set; } = null;
        public Dictionary<int, QuadModel> ModelsBySpriteID { get; private set; } = null;
    }
}
