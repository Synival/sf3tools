using System.Collections.Generic;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;

namespace SF3.Models.Structs.X8PC {
    public class PC_XPDataStruct : SGL_Model_XPDataStruct {
        public PC_XPDataStruct(IByteData data, int id, string name, int address, PolyChar polyChar) : base(data, id, name, address) {
            PolyChar = polyChar;
        }

        public override int ModelCollectionID => 0;
        public override int ModelID => ID;
        public override int LevelOfDetail => 0;

        public override IReadOnlyList<VECTOR> Vertices        => PolyChar.VertexTablesByOffset.TryGetValue((int) VerticesOffset, out var table) ? table : null;
        public override IReadOnlyList<PolygonStruct> Polygons => PolyChar.PolygonTablesByOffset.TryGetValue((int) PolygonsOffset, out var table) ? table : null;
        public override IReadOnlyList<AttrStruct> Attributes  => PolyChar.AttrTablesByOffset.TryGetValue((int) AttributesOffset, out var table) ? table : null;
        public override IReadOnlyList<VECTOR> VertexNormals   => PolyChar.VertexNormalTablesByOffset.TryGetValue((int) VertexNormalsOffset, out var table) ? table : null;

        public PolyChar PolyChar { get; }
    }
}
