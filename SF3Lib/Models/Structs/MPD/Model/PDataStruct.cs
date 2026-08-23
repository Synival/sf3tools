using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.Model;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class PDataStruct : Shared.PDataStruct, IMPD_ModelLoD {
        public PDataStruct(IByteData data, int id, string name, int address,
            MPD_CollectionType collection, IMPD_File mpdFile, int? chunkIndex, int modelId, int lod, int refs
        ) : base(data, id, name, address, 0x14) {
            Collection    = collection;
            MPD_File      = mpdFile;
            ChunkIndex    = chunkIndex;
            ModelID       = modelId;
            LevelOfDetail = lod;
            Refs          = refs;

            var faceCount = FaceCount;
            var mockFaces = Enumerable.Range(0, faceCount).Select(x => new MockFace(this, x)).ToArray();
            Faces = new MockFaceEnumerable(mockFaces);
        }

        public int ModelCollectionID => (int) Collection;

        [TableViewModelColumn(addressField: null, displayOrder: -2.66f, displayName: "Collection", minWidth: 110)]
        public MPD_CollectionType Collection { get; }

        public IMPD_File MPD_File { get; }

        private ModelChunk _modelChunk = null;
        public ModelChunk Chunk {
            get {
                if (_modelChunk == null)
                    _modelChunk = (ModelChunk) MPD_File.ModelCollections?.Values?.FirstOrDefault(x => x is ModelChunk mc && mc.Collection == Collection);
                return _modelChunk;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: -2.33f, displayName: "Chunk #")]
        public int? ChunkIndex { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -2.15f, displayFormat: "X2")]
        public int ModelID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -2.14f)]
        public int LevelOfDetail { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.2f)]
        public int Refs { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f, isPointer: true, isReadOnly: true)]
        public uint RamAddress { get; set; }

        private class MockFace : ISGL_ModelFace {
            public MockFace(PDataStruct pdata, int index) {
                PData = pdata;
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

            public readonly PDataStruct PData;
            public readonly int Index;

            public PolygonStruct Polygon => ((Index < PData.Polygons?.Count) == true) ? PData.Polygons[Index] : null;
            public AttrStruct AttributeStruct => ((Index < PData.Attributes?.Count) == true) ? PData.Attributes[Index] : null;
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

        public PolygonTable Polygons => (Chunk?.PolygonTablesByMemoryAddress?.TryGetValue(PolygonsOffset, out var polygons) == true) ? polygons : null;
        public AttrTable Attributes => (Chunk?.AttrTablesByMemoryAddress?.TryGetValue(AttributesOffset, out var attributes) == true) ? attributes : null;

        public IReadOnlyList<VECTOR> Vertices => (Chunk?.VertexTablesByMemoryAddress?.TryGetValue(VerticesOffset, out var vertices) == true) ? vertices : null;
        public IReadOnlyList<ISGL_ModelFace> Faces { get; }
    }
}
