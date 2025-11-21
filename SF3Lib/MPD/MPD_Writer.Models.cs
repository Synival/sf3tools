using System.Collections.Generic;
using CommonLib.SGL;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteModelChunk(SGL_Model[] models, IMPD_ModelInstance[] instances /* TODO: collision line data */) {
            // Update the address of this new chunk.
            AtOffset(0x2000 + Chunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));

            // Chunks are stored either in low memory (current offset + 0x290000) or high memory (0x060A000 - chunk start).
            // We'll need to pass this information along to the writers so they write the pointers correctly.
            var fileChunkAddr = (int) CurrentOffset;
            // TODO: support high-memory
            var ramChunkAddr = 0x290000 + fileChunkAddr;

            // Write header. Collision-related offsets will be written later.
            var collisionLinesHeaderOffset = CurrentOffset;
            WriteMPDPointer(null);
            var collisionBlocksOffset = CurrentOffset;
            WriteMPDPointer(null);
            WriteUShort((ushort) (instances?.Length ?? 0));

            // Model instances immediately follow the header.
            if (instances != null)
                foreach (var inst in instances)
                    WriteModelInstance(inst, fileChunkAddr, ramChunkAddr);
            WriteUInt(0);

            // PDATAs, POINTs, POLYGONs, and ATTRs follow after that.
            if (models != null)
                foreach (var model in models)
                    WriteModel(model, eightPDatas: true, fileChunkAddr, ramChunkAddr);

            // The collision data is at the end.
            WriteCollisionLinesHeader(collisionLinesHeaderOffset, fileChunkAddr, ramChunkAddr);
            WriteCollisionBlocks(collisionBlocksOffset, fileChunkAddr, ramChunkAddr);

            // Write size
            var endOffset = CurrentOffset;
            AtOffset(0x2000 + Chunks * 0x08 + 0x04, curOffset => WriteUInt((uint) (endOffset - fileChunkAddr)));
            Chunks++;

            WriteToAlignTo(4);
        }

        public void WriteModelInstance(IMPD_ModelInstance instance, int fileChunkAddr, int ramChunkAddr) {
            // Placeholder pointers to be populated later.
            for (int i = 0; i < 8; i++) {
                int pdataId = instance.ModelID * 10 + i;
                if (!_pdataIdToOffsetPtrMap.ContainsKey(pdataId))
                    _pdataIdToOffsetPtrMap.Add(pdataId, new List<long>());
                _pdataIdToOffsetPtrMap[pdataId].Add(CurrentOffset);
                WriteMPDPointer(null);
            }

            WriteShort(instance.PositionX);
            WriteShort(instance.PositionY);
            WriteShort(instance.PositionZ);

            WriteShort(new CompressedFIXED(instance.AngleX, 0).RawShort);
            WriteShort(new CompressedFIXED(instance.AngleY, 0).RawShort);
            WriteShort(new CompressedFIXED(instance.AngleZ, 0).RawShort);

            WriteInt(new FIXED(instance.ScaleX, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleY, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleZ, 0).RawInt);

            WriteUShort(instance.Tag);
            WriteUShort(instance.Flags);
        }

        public void WriteModel(SGL_Model model, bool eightPDatas, int fileChunkAddr, int ramChunkAddr) {
            var pdataCount = eightPDatas ? 8 : 1;

            // Track where the pointers to the various tables will be.
            var pointsPtrs   = new long[pdataCount];
            var polygonsPtrs = new long[pdataCount];
            var attrsPtrs    = new long[pdataCount];

            // Write PDATAs.
            uint addr;
            for (int i = 0; i < pdataCount; i++) {
                var pdataId = model.ID + i;
                if (_pdataIdToOffsetPtrMap.TryGetValue(pdataId, out var ptrs)) {
                    addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
                    AtOffsets(ptrs.ToArray(), _ => WriteUInt(addr));
                }

                // Write placeholders for the tables to write and their counts.
                pointsPtrs[i] = CurrentOffset;
                WriteMPDPointer(null);
                WriteInt(model.Vertices.Count);
                polygonsPtrs[i] = CurrentOffset;
                WriteMPDPointer(null);
                WriteInt(model.Faces.Count);
                attrsPtrs[i] = CurrentOffset;
                WriteMPDPointer(null);
            }

            addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
            AtOffsets(pointsPtrs, _ => WriteUInt(addr));
            WritePOINTs(model);

            addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
            AtOffsets(polygonsPtrs, _ => WriteUInt(addr));
            WritePOLYGONs(model);

            for (var i = 0; i < pdataCount; i++) {
                addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
                AtOffset(attrsPtrs[i], _ => WriteUInt(addr));
                WriteATTRs(model, i);
            }
        }

        public void WritePOINTs(SGL_Model model) {
            foreach (var vertex in model.Vertices)
                WritePOINT(vertex);
        }

        public void WritePOINT(VECTOR vertex) {
            WriteInt(vertex.X.RawInt);
            WriteInt(vertex.Y.RawInt);
            WriteInt(vertex.Z.RawInt);
        }

        public void WritePOLYGONs(SGL_Model model) {
            foreach (var face in model.Faces)
                WritePOLYGON(face);
        }

        public void WritePOLYGON(SGL_ModelFace face) {
            WriteInt(face.Normal.X.RawInt);
            WriteInt(face.Normal.Y.RawInt);
            WriteInt(face.Normal.Z.RawInt);
            WriteUShort((ushort) face.VertexIndices[0]);
            WriteUShort((ushort) face.VertexIndices[1]);
            WriteUShort((ushort) face.VertexIndices[2]);
            WriteUShort((ushort) face.VertexIndices[3]);
        }

        public void WriteATTRs(SGL_Model model, int lodIndex) {
            foreach (var face in model.Faces)
                WriteATTR(face.Attributes, lodIndex);
        }

        public void WriteATTR(IATTR attr, int lodIndex) {
            WriteByte(attr.Plane);
            WriteByte(attr.SortAndOptions);
            WriteUShort(attr.TextureNo);
            WriteUShort((ushort) (attr.Mode | ((lodIndex > 0) ? 0x1000 : 0x0000)));
            WriteUShort(attr.ColorNo);
            WriteUShort((ushort) (attr.GouraudShadingTable + lodIndex));
            WriteUShort(attr.Dir);
        }

        public void WriteCollisionLinesHeader(long ptrToOffset, int fileChunkAddr, int ramChunkAddr) {
            AtOffset(ptrToOffset, curAddr => WriteUInt((uint) (curAddr - fileChunkAddr + ramChunkAddr)));

            // TODO: actually write the real lines!
            WriteUInt(0);
            WriteUInt(0);
        }

        public void WriteCollisionBlocks(long ptrToOffset, int fileChunkAddr, int ramChunkAddr) {
            AtOffset(ptrToOffset, curAddr => WriteUInt((uint) (curAddr - fileChunkAddr + ramChunkAddr)));

            // TODO: actually write the real blocks!
            // 16 * 16 pointers
            var blockAddr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr) + 0x400;
            for (var i = 0; i < 0x100; i++) {
                WriteUInt(blockAddr);
                blockAddr += 2;
            }

            // 16 * 16 tables, terminated by 0xFFFF.
            for (var i = 0; i < 0x100; i++)
                WriteUShort(0xFFFF);
        }
    }
}
