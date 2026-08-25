using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Models.Structs.Shared.SGL {
    public abstract class SGL_Model_XPDataStruct : XPDataStruct, ISGL_Model {
        protected SGL_Model_XPDataStruct(IByteData data, int id, string name, int address) : base(data, id, name, address) {
            var faceCount = FaceCount;
            var mockFaces = Enumerable.Range(0, faceCount).Select(x => new MockFace(this, x)).ToArray();
            Faces = new MockFaceEnumerable(mockFaces);
        }

        private class MockFace : ISGL_ModelFace {
            public MockFace(SGL_Model_XPDataStruct sglModel, int index) {
                SGL_Model = sglModel;
                Index = index;
            }

            public IReadOnlyList<int> VertexIndices => Polygon?.Vertices;

            public VECTOR Normal {
                get {
                    var poly = Polygon;
                    return poly == null ? new VECTOR() : new VECTOR(poly.NormalX, poly.NormalY, poly.NormalZ);
                }
                set {
                    var poly = Polygon;
                    if (poly != null && value != null) {
                        poly.NormalX = value.X.Float;
                        poly.NormalY = value.Y.Float;
                        poly.NormalZ = value.Z.Float;
                    }
                }
            }

            public IATTR Attributes {
                get {
                    var attr = AttributeStruct;
                    return attr == null ? new ATTR() : (IATTR) attr;
                }
                set {
                    var attr = AttributeStruct;
                    if (attr != null && value != null) {
                        attr.Plane          = value.Plane;
                        attr.SortAndOptions = value.SortAndOptions;
                        attr.TextureNo      = value.TextureNo;
                        attr.Mode           = value.Mode;
                        attr.ColorNo        = value.ColorNo;
                        attr.GouraudShadingTable = value.GouraudShadingTable;
                        attr.Dir            = value.Dir;
                    }
                }
            }

            public readonly SGL_Model_XPDataStruct SGL_Model;
            public readonly int Index;

            public PolygonStruct Polygon => ((Index < SGL_Model.Polygons?.Count) == true) ? SGL_Model.Polygons[Index] : null;
            public AttrStruct AttributeStruct => ((Index < SGL_Model.Attributes?.Count) == true) ? SGL_Model.Attributes[Index] : null;
        }

        private class MockFaceEnumerable : IReadOnlyList<ISGL_ModelFace> {
            public MockFaceEnumerable(MockFace[] faces) {
                Faces = faces;
            }

            public MockFace[] Faces { get; }

            public int Count => Faces.Length;
            public ISGL_ModelFace this[int index] => Faces[index];
            public ISGL_ModelFace[] AsArray() => Faces;

            public IEnumerator<ISGL_ModelFace> GetEnumerator() => ((IEnumerable<ISGL_ModelFace>) Faces).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public abstract int ModelCollectionID { get; }
        public abstract int ModelID { get; }
        public abstract int LevelOfDetail { get; }

        public abstract IReadOnlyList<VECTOR> Vertices { get; }
        public abstract PolygonTable Polygons { get; }
        public abstract AttrTable Attributes { get; }
        public abstract IReadOnlyList<VECTOR> VertexNormals { get; }

        public IReadOnlyList<ISGL_ModelFace> Faces { get; }
    }
}
