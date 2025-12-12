using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.SGL;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteModelChunk(IEnumerable<ISGL_Model> models, IEnumerable<IMPD_ModelInstance> instances, IMPD_Collisions collisions, bool isHighMemory)
            => WriteUncompressedChunk(writer => writer.WriteModelChunkContent(models, instances, collisions, isHighMemory));

        public void WriteModelChunkContent(IEnumerable<ISGL_Model> models, IEnumerable<IMPD_ModelInstance> instances, IMPD_Collisions collisions, bool isHighMemory) {
            // Chunks are stored either in low memory (current offset + 0x290000) or high memory (0x060A000 - chunk start).
            // We'll need to pass this information along to the writers so they write the pointers correctly.
            var fileChunkAddr = (int) CurrentOffset;
            var ramChunkAddr = (isHighMemory) ? 0x060A0000 : (0x00290000 + fileChunkAddr);

            // Write header. Collision-related offsets will be written later.
            var collisionLinesHeaderOffset = CurrentOffset;
            WriteMPDPointer(null);
            var collisionBlocksOffset = CurrentOffset;
            WriteMPDPointer(null);

            // Last part of the header: the number of model instances.
            WriteUShort((ushort) (instances?.Count() ?? 0));

            // Model instances immediately follow the header.
            if (instances != null)
                foreach (var inst in instances)
                    WriteModelChunkInstance(inst);
            WriteUInt(0);

            // PDATAs, POINTs, POLYGONs, and ATTRs follow after that.
            if (models != null)
                foreach (var model in models)
                    WriteModelChunkModel(model, fileChunkAddr, ramChunkAddr);

            // The collision data is at the end.
            WriteCollisionLinesSection(collisions, collisionLinesHeaderOffset, fileChunkAddr, ramChunkAddr);
            WriteCollisionBlocks(collisionBlocksOffset, fileChunkAddr, ramChunkAddr);
        }

        public void WriteModelChunkInstance(IMPD_ModelInstance instance) {
            // Placeholder pointers to be populated later.
            for (int i = 0; i < 8; i++) {
                int pdataId = instance.ModelID + i;
                if (!_pdataIdToOffsetPtrMap.ContainsKey(pdataId))
                    _pdataIdToOffsetPtrMap.Add(pdataId, new List<long>());
                _pdataIdToOffsetPtrMap[pdataId].Add(CurrentOffset);
                WriteMPDPointer(null);
            }

            WriteShort(instance.PositionX);
            WriteShort(instance.PositionY);
            WriteShort(instance.PositionZ);

            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleX / 180.0f * 0x8000)).RawShort);
            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleY / 180.0f * 0x8000)).RawShort);
            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleZ / 180.0f * 0x8000)).RawShort);

            WriteInt(new FIXED(instance.ScaleX, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleY, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleZ, 0).RawInt);

            WriteUShort(instance.Tag);
            WriteUShort(instance.Flags);
        }

        public void WriteModelChunkModel(ISGL_Model model, int fileChunkAddr, int ramChunkAddr) {
            const int c_pdataCount = 8;

            // Track where the pointers to the various tables will be.
            var pointsPtrs   = new long[c_pdataCount];
            var polygonsPtrs = new long[c_pdataCount];
            var attrsPtrs    = new long[c_pdataCount];

            // Write PDATAs.
            uint addr;
            for (int i = 0; i < c_pdataCount; i++) {
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

            for (var i = 0; i < c_pdataCount; i++) {
                addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
                AtOffset(attrsPtrs[i], _ => WriteUInt(addr));
                WriteATTRs(model, i);
            }
        }

        public void WriteHeaderModels(IEnumerable<ISGL_Model> models, IEnumerable<IMPD_ModelInstance> instances, out uint instanceTableOffset) {
            var pdataPosByInstanceIndex = new Dictionary<int, uint>();
            int index = -1;

            // Write all models. Do it based on instances so we have a 1:1 models-to-instances relationship.
            // TODO: (It doesn't really need to be 1:1 models to instances)
            foreach (var instance in instances) {
                index++;
                var model = models.FirstOrDefault(x => x.ID == instance.ModelID);
                if (model == null)
                    continue;

                // Write tables necessary for the PDATA
                var verticesPos = (int) CurrentOffset;
                WritePOINTs(model);
                var polygonsPos = (int) CurrentOffset;
                WritePOLYGONs(model);
                var attributesPos = (int) CurrentOffset;
                WriteATTRs(model, 0);

                // Now write the PDATA
                pdataPosByInstanceIndex[instance.ID] = (uint) CurrentOffset;
                WriteMPDPointer(verticesPos);
                WriteInt(model.Vertices.Count);
                WriteMPDPointer(polygonsPos);
                WriteInt(model.Faces.Count);
                WriteMPDPointer(attributesPos);
            }

            // Write all instances.
            instanceTableOffset = (uint) CurrentOffset;
            index = -1;
            foreach (var instance in instances) {
                index++;
                if (pdataPosByInstanceIndex.TryGetValue(index, out var pdataPos))
                    WriteHeaderModelInstance(instance, pdataPos);
            }

            // Terminate with -- for some reason -- 28 empty bytes.
            WriteBytes(new byte[0x1B]);
        }

        public void WriteHeaderModelInstance(IMPD_ModelInstance instance, uint pdataOffset) {
            WriteMPDPointer(pdataOffset);

            WriteShort(instance.PositionX);
            WriteShort(instance.PositionY);
            WriteShort(instance.PositionZ);

            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleX / 180.0f * 0x8000)).RawShort);
            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleY / 180.0f * 0x8000)).RawShort);
            WriteShort(new CompressedFIXED((short) Math.Round(instance.AngleZ / 180.0f * 0x8000)).RawShort);

            WriteInt(new FIXED(instance.ScaleX, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleY, 0).RawInt);
            WriteInt(new FIXED(instance.ScaleZ, 0).RawInt);
        }

        public void WritePOINTs(ISGL_Model model) {
            foreach (var vertex in model.Vertices)
                WritePOINT(vertex);
        }

        public void WritePOINT(VECTOR vertex) {
            WriteInt(vertex.X.RawInt);
            WriteInt(vertex.Y.RawInt);
            WriteInt(vertex.Z.RawInt);
        }

        public void WritePOLYGONs(ISGL_Model model) {
            foreach (var face in model.Faces)
                WritePOLYGON(face);
        }

        public void WritePOLYGON(ISGL_ModelFace face) {
            WriteInt(face.Normal.X.RawInt);
            WriteInt(face.Normal.Y.RawInt);
            WriteInt(face.Normal.Z.RawInt);
            WriteUShort((ushort) face.VertexIndices[0]);
            WriteUShort((ushort) face.VertexIndices[1]);
            WriteUShort((ushort) face.VertexIndices[2]);
            WriteUShort((ushort) face.VertexIndices[3]);
        }

        public void WriteATTRs(ISGL_Model model, int lodIndex) {
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

        public void WriteCollisionLinesSection(IMPD_Collisions collisions, long ptrToOffset, int fileChunkAddr, int ramChunkAddr) {
            AtOffset(ptrToOffset, curAddr => WriteUInt((uint) (curAddr - fileChunkAddr + ramChunkAddr)));

            // Write a header. The points come first, then the lines.
            var pointsAddr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr) + 0x08;
            var linesAddr = pointsAddr + (uint) collisions.Points.Count() * 0x04;

            WriteUInt(pointsAddr);
            WriteUInt(linesAddr);

            WriteCollisionPoints(collisions.Points);
            WriteCollisionLines(collisions.Points, collisions.Lines);
        }

        public void WriteCollisionPoints(IEnumerable<IMPD_CollisionPoint> points) {
            // Just (X, Y) coordinates!
            foreach (var point in points) {
                WriteShort(point.X);
                WriteShort(point.Y);
            }
        }

        public void WriteCollisionLines(IEnumerable<IMPD_CollisionPoint> points, IEnumerable<IMPD_CollisionLine> lines) {
            // Map to retrieve indices from point references.
            var indicesForPoints = points
                .Select((x, i) => (Point: x, Index: i))
                .ToDictionary(x => x.Point, x => (ushort) x.Index);

            foreach (var line in lines) {
                // Don't bother writing lines that don't have valid points or indices.
                var index1 = indicesForPoints.TryGetValue(line.Point1, out var index1Out) ? (ushort?) index1Out: null;
                var index2 = indicesForPoints.TryGetValue(line.Point2, out var index2Out) ? (ushort?) index2Out: null;
                if (!index1.HasValue || !index2.HasValue)
                    continue;

                WriteUShort(index1.Value);
                WriteUShort(index2.Value);
                WriteShort(new CompressedFIXED(line.Angle / 180.0f, 0).RawShort);
                WriteByte(line.Tag);
                WriteByte((line.FlagToDisable.HasValue && line.FlagToDisable.Value >= 0x201 && line.FlagToDisable.Value <= 0x2FF) ? (byte) (line.FlagToDisable & 0xFF) : (byte) 0);
            }
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

        private Dictionary<int, List<long>> _pdataIdToOffsetPtrMap = new Dictionary<int, List<long>>();
    }
}
