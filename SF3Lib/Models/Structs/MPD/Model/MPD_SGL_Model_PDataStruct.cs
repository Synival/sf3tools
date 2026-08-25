using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.Shared.SGL;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Model {
    public class MPD_SGL_Model_PDataStruct : SGL_Model_PDataStruct, IMPD_ModelLoD {
        public MPD_SGL_Model_PDataStruct(IByteData data, int id, string name, int address,
            MPD_CollectionType collection, IMPD_File mpdFile, int? chunkIndex, int modelId, int lod, int refs
        ) : base(data, id, name, address) {
            Collection    = collection;
            MPD_File      = mpdFile;
            ChunkIndex    = chunkIndex;
            ModelID       = modelId;
            LevelOfDetail = lod;
            Refs          = refs;
        }

        public override int ModelCollectionID => (int) Collection;

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
        public override int ModelID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -2.14f)]
        public override int LevelOfDetail { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.2f)]
        public int Refs { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f, isPointer: true, isReadOnly: true)]
        public uint RamAddress { get; set; }

        public override IReadOnlyList<VECTOR> Vertices        => (Chunk?.VertexTablesByMemoryAddress?.TryGetValue(VerticesOffset, out var vertices) == true) ? vertices : null;
        public override IReadOnlyList<PolygonStruct> Polygons => (Chunk?.PolygonTablesByMemoryAddress?.TryGetValue(PolygonsOffset, out var polygons) == true) ? polygons : null;
        public override IReadOnlyList<AttrStruct> Attributes  => (Chunk?.AttrTablesByMemoryAddress?.TryGetValue(AttributesOffset, out var attributes) == true) ? attributes : null;
    }
}
