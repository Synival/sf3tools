using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class CollisionResources : ResourcesBase {
        protected override void PerformInit() { }

        public override void DeInit() { }

        public override void Reset() {
            IndividualModels?.Dispose();
            FullModel?.Dispose();
            FullModel = null;
        }

        private struct Position {
            public Position(float x, float topY, float bottomY, float z) {
                X = x;
                Z = z;
                TopY = topY;
                BottomY = bottomY;
            }

            public float X, TopY, BottomY, Z;
        }

        public void Update(IMPD_Collisions collisions, IMPD_Surface surface, int groundY) {
            Reset();

            if (collisions?.Lines == null)
                return;

            var groundYf = groundY / -32.0f;
            var fullQuads = new List<Quad>();

            foreach (var line in collisions.Lines) {
                Position GetPointPosition(int x, int y) {
                    var xf = x / 32.0f + GeneralResources.ModelOffsetX;
                    float? topY    = null;
                    float? bottomY = null;
                    var zf = y / -32.0f - GeneralResources.ModelOffsetZ;

                    for (int ty = -1; ty <= 1; ty++) {
                        for (int tx = -1; tx <= 1; tx++) {
                            var surfaceX = xf + 32f + (tx * 0.5f);
                            var surfaceY = -zf + 32f + (ty * 0.5f);

                            var tileX = (int) surfaceX;
                            var tileY = (int) surfaceY;

                            if (tileX > 0 && tileX < 64 && tileY > 0 && tileY < 64) {
                                var tile = surface.GetTile(tileX, tileY);
                                var xInTile = 1.00f - (surfaceX - tileX);
                                var yInTile = 1.00f - (surfaceY - tileY);

                                var heights1 = tile.GetVertexHeight(CornerType.BottomLeft) * xInTile + tile.GetVertexHeight(CornerType.BottomRight) * (1.0f - xInTile);
                                var heights2 = tile.GetVertexHeight(CornerType.TopLeft)    * xInTile + tile.GetVertexHeight(CornerType.TopRight)    * (1.0f - xInTile);
                                var height = heights1 * yInTile + heights2 * (1.0f - yInTile);

                                topY    = (topY == null) ? height + 1.0f : Math.Max(topY.Value, height + 1.0f);
                                bottomY = (bottomY == null) ? height : Math.Min(bottomY.Value, height);
                            }
                        }
                    }

                    return new Position(xf, topY ?? groundYf + 1.0f, bottomY ?? groundYf, zf);
                }

                var pos1 = GetPointPosition(line.X1, line.Y1);
                var pos2 = GetPointPosition(line.X2, line.Y2);

                var poly = new POLYGON([
                    new VECTOR(pos1.X, pos1.BottomY, pos1.Z),
                    new VECTOR(pos1.X, pos1.TopY,    pos1.Z),
                    new VECTOR(pos2.X, pos2.TopY,    pos2.Z),
                    new VECTOR(pos2.X, pos2.BottomY, pos2.Z)
                ]);

                var dist = (new Vector2(pos2.X, pos2.Z) - new Vector2(pos1.X, pos1.Z)).Length;
                var absCos = (float) Math.Abs(pos2.X - pos1.X) / dist;
                var absSin = (float) Math.Abs(pos2.Z - pos1.Z) / dist;

                // Assign a color similarly to a "normals" view, with blue for horizontal and red for vertical.
                // Highlight special line segments magenta/orange.
                var color = new Vector3(
                    absSin * 0.25f + 0.75f,
                    (!line.FlagToDisable.HasValue) ? 0.75f : 0.25f,
                    absCos * 0.25f + 0.75f
                );
                var colors = new Vector3[] {
                    color * 0.7f,
                    color * 0.8f,
                    color * 1.2f,
                    color * 1.3f,
                };

                var indivColors = colors.Select(x => new Vector4(x, 0.66f)).ToArray();
                var fullColors  = colors.Select(x => new Vector4(x, 0.25f)).ToArray();

                var vertices  = poly.Vertices.Select(x => x.ToVector3()).ToArray();
                var indivQuad = new Quad(vertices, indivColors);
                var fullQuad = new Quad(vertices, fullColors);

                IndividualModels.Add(new QuadModel([indivQuad]));
                fullQuads.Add(fullQuad);
            }

            FullModel = new QuadModel(fullQuads.ToArray());
        }

        public DisposableList<QuadModel> IndividualModels { get; } = [];
        public QuadModel FullModel { get; private set; } = null;
    }
}
