using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.MPD.Interfaces;
using SF3.Win.Extensions;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class CollisionResources : ResourcesBase {
        protected override void PerformInit() { }

        public override void DeInit() { }

        public override void Reset() {
            IndividualModels?.Dispose();
            FullModel?.Dispose();
            FullModel = null;
        }

        public void Update(IMPD_Collisions collisions, IMPD_Surface surface, int groundY) {
            Reset();

            if (collisions?.Lines == null)
                return;

            var fullQuads = new List<Quad>();
            fullQuads.AddRange(GetLineQuads(collisions.Lines, surface, groundY));
            fullQuads.AddRange(GetPointQuads(collisions.Lines, collisions.Points, surface, groundY));

            FullModel = new QuadModel(fullQuads.ToArray());
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

        private Position GetPointPosition(int x, int y, IMPD_Surface surface, int groundY) {
            var groundYf = groundY / -32.0f;

            var xf = x / 32.0f + GeneralResources.ModelOffsetX;
            float? topY    = null;
            float? bottomY = null;
            var zf = y / -32.0f - GeneralResources.ModelOffsetZ;

            for (var ty = -1; ty <= 1; ty++) {
                for (var tx = -1; tx <= 1; tx++) {
                    var surfaceX = xf + 32f + tx * 0.5f;
                    var surfaceY = -zf + 32f + ty * 0.5f;

                    var tileX = (int) surfaceX;
                    var tileY = (int) surfaceY;

                    if (tileX > 0 && tileX < 64 && tileY > 0 && tileY < 64) {
                        var tile = surface.GetTile(tileX, tileY);
                        var xInTile = 1.00f - (surfaceX - tileX);
                        var yInTile = 1.00f - (surfaceY - tileY);

                        var heights1 = (tile.GetVertexHeight(CornerType.BottomLeft) * xInTile + tile.GetVertexHeight(CornerType.BottomRight) * (1.0f - xInTile)) / 16.0f;
                        var heights2 = (tile.GetVertexHeight(CornerType.TopLeft)    * xInTile + tile.GetVertexHeight(CornerType.TopRight)    * (1.0f - xInTile)) / 16.0f;
                        var height = heights1 * yInTile + heights2 * (1.0f - yInTile);

                        topY    = topY == null ? height + 1.0f : Math.Max(topY.Value, height + 1.0f);
                        bottomY = bottomY == null ? height : Math.Min(bottomY.Value, height);
                    }
                }
            }

            return new Position(xf, topY ?? groundYf + 1.0f, bottomY ?? groundYf, zf);
        }

        private List<Quad> GetLineQuads(IEnumerable<IMPD_CollisionLine> lines, IMPD_Surface surface, int groundY) {
            var quads = new List<Quad>();
            foreach (var line in lines) {
                var pos1 = GetPointPosition(line.X1, line.Y1, surface, groundY);
                var pos2 = GetPointPosition(line.X2, line.Y2, surface, groundY);

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
                    !line.FlagToDisable.HasValue ? 0.75f : 0.25f,
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
                var fullQuad  = new Quad(vertices, fullColors);

                IndividualModels.Add(new CollisionQuadModel([indivQuad], false, line.ID));
                quads.Add(fullQuad);
            }

            return quads;
        }

        private List<Quad> GetPointQuads(IEnumerable<IMPD_CollisionLine> lines, IEnumerable<IMPD_CollisionPoint> points, IMPD_Surface surface, int groundY) {
            var pointsInUse = lines.SelectMany(x => new IMPD_CollisionPoint[] { x.Point1, x.Point2 }).ToHashSet();

            var white = new Vector3[] {
                new(0.7f),
                new(0.8f),
                new(0.9f),
                new(1.0f),
            };
            var indivWhiteColors = white.Select(x => new Vector4(x, 0.66f)).ToArray();
            var fullWhiteColors  = white.Select(x => new Vector4(x, 0.25f)).ToArray();

            var red = new Vector3[] {
                new(1.0f, 0.0f, 0.0f),
                new(1.0f, 0.2f, 0.1f),
                new(1.0f, 0.4f, 0.2f),
                new(1.0f, 0.6f, 0.3f),
            };
            var indivRedColors = red.Select(x => new Vector4(x, 0.66f)).ToArray();
            var fullRedColors  = red.Select(x => new Vector4(x, 0.25f)).ToArray();

            var fullQuads = new List<Quad>();
            foreach (var point in points) {
                var pos1 = GetPointPosition(point.X - 2, point.Y - 2, surface, groundY);
                var pos2 = GetPointPosition(point.X + 2, point.Y + 2, surface, groundY);
                var topY    = Math.Max(pos1.TopY,    pos2.TopY)    + 2 / 32f;
                var bottomY = Math.Min(pos1.BottomY, pos2.BottomY) + 2 / 32f;

                var polyPoints = new VECTOR[] {
                    new(pos1.X, topY, pos1.Z),
                    new(pos2.X, topY, pos1.Z),
                    new(pos2.X, topY, pos2.Z),
                    new(pos1.X, topY, pos2.Z),

                    new(pos1.X, bottomY, pos1.Z),
                    new(pos2.X, bottomY, pos1.Z),
                    new(pos2.X, bottomY, pos2.Z),
                    new(pos1.X, bottomY, pos2.Z),
                };

                var polys = new POLYGON[] {
                    new([polyPoints[0], polyPoints[1], polyPoints[2], polyPoints[3]]),
                    new([polyPoints[4], polyPoints[5], polyPoints[1], polyPoints[0]]),
                    new([polyPoints[5], polyPoints[6], polyPoints[2], polyPoints[1]]),
                    new([polyPoints[6], polyPoints[7], polyPoints[3], polyPoints[2]]),
                    new([polyPoints[7], polyPoints[4], polyPoints[0], polyPoints[3]]),
                    new([polyPoints[7], polyPoints[6], polyPoints[5], polyPoints[4]]),
                };

                var hasLines = pointsInUse.Contains(point);
                var indivColors = hasLines ? indivWhiteColors : fullWhiteColors;
                var fullColors  = hasLines ? fullWhiteColors  : fullRedColors;

                var indivQuads = new List<Quad>();
                foreach (var poly in polys) {
                    var vertices  = poly.Vertices.Select(x => x.ToVector3()).ToArray();
                    var indivQuad = new Quad(vertices, indivColors);
                    var fullQuad  = new Quad(vertices, fullColors);

                    indivQuads.Add(indivQuad);
                    fullQuads.Add(fullQuad);
                }
                IndividualModels.Add(new CollisionQuadModel(indivQuads.ToArray(), true, point.ID));
            }

            return fullQuads;
        }

        public class CollisionQuadModel : QuadModel {
            public CollisionQuadModel(Quad[] quads, bool isPoint, int id) : base(quads) {
                IsPoint = isPoint;
                ID = id;
            }

            public bool IsPoint { get; }
            public int ID { get; }
        }

        public DisposableList<CollisionQuadModel> IndividualModels { get; } = [];
        public QuadModel FullModel { get; private set; } = null;
    }
}
