using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.SGL;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Win.Extensions;

namespace SF3.Win.OpenGL.MPD_File {
    public class CollisionResources : ResourcesBase, IMPD_Resources {
        protected override void PerformInit() { }

        public override void DeInit() { }

        public override void Reset() {
            IndividualModels?.Dispose();
            FullModel?.Dispose();
            FullModel = null;
        }

        public void Update(IMPD_File mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var groundY = (mpdFile.MPDHeader?.GroundY ?? 0) / -32.0f;
            var fullQuads = new List<Quad>();
            foreach (var mc in mpdFile.ModelCollections) {
                if (mc.CollisionLineTable == null || mc.CollisionPointTable == null)
                    continue;

                int pos = 0;
                foreach (var line in mc.CollisionLineTable) {
                    var point1 = mc.CollisionPointTable[line.Point1Index];
                    var point2 = mc.CollisionPointTable[line.Point2Index];

                    Vector3 GetPointPosition(CollisionPoint point) {
                        var x = point.X /  32.0f + GeneralResources.ModelOffsetX;
                        var y = groundY;
                        var z = point.Y / -32.0f - GeneralResources.ModelOffsetZ;

                        var surfaceX = x + 32f;
                        var surfaceY = -z + 32f;

                        var tileX = (int) surfaceX;
                        var tileY = (int) surfaceY;

                        pos++;

                        if (tileX > 0 && tileX < 64 && tileY > 0 && tileY < 64) {
                            var tile = mpdFile.Tiles[tileX, tileY];
                            var xInTile = 1.00f - (surfaceX - tileX);
                            var yInTile = 1.00f - (surfaceY - tileY);

                            var heights1 = tile.GetMoveHeightmap(CornerType.BottomLeft) * xInTile + tile.GetMoveHeightmap(CornerType.BottomRight) * (1.0f - xInTile);
                            var heights2 = tile.GetMoveHeightmap(CornerType.TopLeft)    * xInTile + tile.GetMoveHeightmap(CornerType.TopRight)    * (1.0f - xInTile);
                            var height = heights1 * yInTile + heights2 * (1.0f - yInTile);

                            y = Math.Max(y, height);
                        }

                        return new Vector3(x, y, z);
                    }

                    var pos1 = GetPointPosition(point1);
                    var pos2 = GetPointPosition(point2);

                    var poly = new POLYGON([
                        new VECTOR(pos1.X, pos1.Y,        pos1.Z),
                        new VECTOR(pos1.X, pos1.Y + 1.0f, pos1.Z),
                        new VECTOR(pos2.X, pos2.Y + 1.0f, pos2.Z),
                        new VECTOR(pos2.X, pos2.Y,        pos2.Z)
                    ]);

                    var normal = poly.GetNormal(POLYGON_NormalCalculationMethod.AverageOfAllTriangles);

                    var color = new Vector3(normal.X.Float * 0.25f + 0.75f, 0.75f, normal.Z.Float * 0.25f + 0.75f);
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
            }

            FullModel = new QuadModel(fullQuads.ToArray());
        }

        public DisposableList<QuadModel> IndividualModels { get; } = [];
        public QuadModel FullModel { get; private set; } = null;
    }
}
