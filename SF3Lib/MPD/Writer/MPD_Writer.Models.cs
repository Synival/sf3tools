using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.SGL;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Writer {
    public partial class MPD_Writer {
        public void WriteModelChunk(IEnumerable<IMPD_Model> models, IEnumerable<IMPD_ModelInstance> instances, IMPD_Collisions collisions, bool isHighMemory, IIndexedEnumerableWithLength<byte> dataAfterInstances)
            => WriteUncompressedChunk(writer => writer.WriteModelChunkContent(models, instances, collisions, isHighMemory, dataAfterInstances));

        public void WriteModelChunkContent(IEnumerable<IMPD_Model> models, IEnumerable<IMPD_ModelInstance> instances, IMPD_Collisions collisions, bool isHighMemory, IIndexedEnumerableWithLength<byte> dataAfterInstances) {
            // Chunks are stored either in low memory (current offset + 0x290000) or high memory (0x060A000 - chunk start).
            // We'll need to pass this information along to the writers so they write the pointers correctly.
            var fileChunkAddr = (int) CurrentOffset;
            var ramChunkAddr = (isHighMemory) ? 0x060A0000 : 0x00292100;

            // Write header. Collision-related offsets will be written later.
            var collisionLinesHeaderOffset = CurrentOffset;
            WriteMPDPointer(null);
            var collisionBlocksOffset = CurrentOffset;
            WriteMPDPointer(null);

            // Last part of the header: the number of model instances.
            WriteUShort((ushort) (instances?.Count() ?? 0));

            var pdataIdToOffsetPtrMap = new Dictionary<int, List<List<long>>>();

            // Model instances immediately follow the header.
            if (instances != null)
                foreach (var inst in instances)
                    WriteModelChunkInstance(inst, pdataIdToOffsetPtrMap);
            WriteUInt(0);

            if (dataAfterInstances != null)
                WriteBytes(dataAfterInstances.ToArray());
            WriteToAlignTo(4);

            // PDATAs, POINTs, POLYGONs, and ATTRs follow after that.
            if (models != null)
                foreach (var model in models)
                    WriteModelChunkModel(model, fileChunkAddr, ramChunkAddr, pdataIdToOffsetPtrMap);

            // The collision data is at the end.
            AtOffset(collisionLinesHeaderOffset, pos => WriteUInt((uint) (pos - fileChunkAddr + ramChunkAddr)));
            WriteCollisionLinesSection(collisions, fileChunkAddr, ramChunkAddr, out var linesWritten);
            AtOffset(collisionBlocksOffset, pos => WriteUInt((uint) (pos - fileChunkAddr + ramChunkAddr)));
            WriteCollisionBlocks(linesWritten, fileChunkAddr, ramChunkAddr);
        }

        public void WriteModelChunkInstance(IMPD_ModelInstance instance, Dictionary<int, List<List<long>>> pdataIdToOffsetPtrMap) {
            var modelId = instance.ModelID;
            if (!pdataIdToOffsetPtrMap.ContainsKey(modelId))
                pdataIdToOffsetPtrMap.Add(modelId, new List<List<long>>());
            var offsetPtrMap = pdataIdToOffsetPtrMap[modelId];

            // Placeholder pointers to be populated later.
            var levelsOfDetail = instance.LevelsOfDetail;
            for (int i = 0; i < 8; i++) {
                if (i < levelsOfDetail) {
                    while (offsetPtrMap.Count <= i)
                        offsetPtrMap.Add(new List<long>());
                    offsetPtrMap[i].Add(CurrentOffset);
                }
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

        public void WriteModelChunkModel(IMPD_Model model, int fileChunkAddr, int ramChunkAddr, Dictionary<int, List<List<long>>> pdataIdToOffsetPtrMap) {
            // Don't write models that don't have instances.
            // (This matches SF3's own MPD files)
            var offsetPtrMap = pdataIdToOffsetPtrMap.TryGetValue(model.ModelID, out var offsetPtrMapVal) ? offsetPtrMapVal : null;
            if (offsetPtrMap == null || offsetPtrMap.Count == 0)
                return;

            // Track where the pointers to the various tables will be.
            var levelsOfDetail = model.LevelsOfDetail;
            var pointsPtrs   = new long[levelsOfDetail];
            var polygonsPtrs = new long[levelsOfDetail];
            var attrsPtrs    = new long[levelsOfDetail];

            // Write PDATAs.
            uint addr;
            for (int i = 0; i < 8; i++) {
                if (i < levelsOfDetail) {
                    var modelLoD = model.ModelLoDs[i];

                    var ptrs = offsetPtrMap[i];
                    addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
                    AtOffsets(ptrs.ToArray(), _ => WriteUInt(addr));

                    // Write placeholders for the tables to write and their counts.
                    pointsPtrs[i] = CurrentOffset;
                    WriteMPDPointer(null);
                    WriteInt(modelLoD.Vertices.Length);
                    polygonsPtrs[i] = CurrentOffset;
                    WriteMPDPointer(null);
                    WriteInt(modelLoD.Faces.Length);
                    attrsPtrs[i] = CurrentOffset;
                    WriteMPDPointer(null);
                }
                else
                    WriteBytes(new byte[0x14]);
            }

            var modelLoD0 = model.ModelLoDs[0];

            addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
            AtOffsets(pointsPtrs, _ => WriteUInt(addr));
            WritePOINTs(modelLoD0);

            addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
            AtOffsets(polygonsPtrs, _ => WriteUInt(addr));
            WritePOLYGONs(modelLoD0);

            for (var i = 0; i < levelsOfDetail; i++) {
                addr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr);
                AtOffset(attrsPtrs[i], _ => WriteUInt(addr));
                WriteATTRs(model.ModelLoDs[i]);
            }
        }

        public uint? WriteHeaderModelsOrNull(IMPD_ModelCollection collection) {
            uint outPos = 0;
            if (WriteObjectOrNull(() => collection != null && !collection.HasMissingModels, () => WriteHeaderModels(collection.Models, collection.ModelInstances, out outPos)).HasValue)
                return outPos;
            else
                return null;
        }

        public void WriteHeaderModels(IEnumerable<IMPD_Model> models, IEnumerable<IMPD_ModelInstance> instances, out uint instanceTableOffset) {
            var pdataPosByInstanceIndex = new Dictionary<int, uint>();
            int index = -1;

            // Write all models. Do it based on instances so we have a 1:1 models-to-instances relationship.
            // TODO: (It doesn't really need to be 1:1 models to instances)
            foreach (var instance in instances) {
                index++;
                var modelSet = models.FirstOrDefault(x => x.ModelID == instance.ModelID);
                if (modelSet == null || modelSet.LevelsOfDetail < 1)
                    continue;
                var model = modelSet.ModelLoDs[0];

                // Write tables necessary for the PDATA
                var verticesPos = (int) CurrentOffset;
                WritePOINTs(model);
                var polygonsPos = (int) CurrentOffset;
                WritePOLYGONs(model);
                var attributesPos = (int) CurrentOffset;
                WriteATTRs(model);

                // Now write the PDATA
                pdataPosByInstanceIndex[instance.ID] = (uint) CurrentOffset;
                WriteMPDPointer(verticesPos);
                WriteInt(model.Vertices.Length);
                WriteMPDPointer(polygonsPos);
                WriteInt(model.Faces.Length);
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
            WriteToAlignTo(2);
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

        public void WriteATTRs(ISGL_Model model) {
            foreach (var face in model.Faces)
                WriteATTR(face.Attributes);
        }

        public void WriteATTR(IATTR attr) {
            WriteByte(attr.Plane);
            WriteByte(attr.SortAndOptions);
            WriteUShort(attr.TextureNo);
            WriteUShort(attr.Mode);
            WriteUShort(attr.ColorNo);
            WriteUShort(attr.GouraudShadingTable);
            WriteUShort(attr.Dir);
        }

        public void WriteCollisionLinesSection(IMPD_Collisions collisions, int fileChunkAddr, int ramChunkAddr, out IMPD_CollisionLine[] linesWritten) {
            // Write a header. The points come first, then the lines.
            // If there are no points or lines, just write zeroes.
            if (collisions == null || collisions.Points == null || collisions.Lines == null ||
                collisions.Points.Count() == 0 && collisions.Lines.Count() == 0
            ) {
                WriteUInt(0);
                WriteUInt(0);
                linesWritten = new IMPD_CollisionLine[0];
                return;
            }

            // Predict what the offsets of the next tables will be and write the header.
            var pointsAddr = (uint) (CurrentOffset - fileChunkAddr + ramChunkAddr) + 0x08;
            var linesAddr = pointsAddr + (uint) collisions.Points.Count() * 0x04;
            WriteUInt(pointsAddr);
            WriteUInt(linesAddr);

            // Write the actual data.
            WriteCollisionPoints(collisions.Points);
            WriteCollisionLines(collisions.Points, collisions.Lines, out var linesWritten2);

            // Handy dandy output parameters.
            linesWritten = linesWritten2;
        }

        public void WriteCollisionPoints(IEnumerable<IMPD_CollisionPoint> points) {
            // Just (X, Y) coordinates!
            foreach (var point in points) {
                WriteShort(point.X);
                WriteShort(point.Y);
            }
        }

        public void WriteCollisionLines(IEnumerable<IMPD_CollisionPoint> points, IEnumerable<IMPD_CollisionLine> lines, out IMPD_CollisionLine[] linesWritten) {
            // Map to retrieve indices from point references.
            var indicesForPoints = points
                .Select((x, i) => (Point: x, Index: i))
                .ToDictionary(x => x.Point, x => (ushort) x.Index);

            var linesWrittenList = new List<IMPD_CollisionLine>();
            foreach (var line in lines) {
                // Don't bother writing lines that don't have valid points or indices.
                var index1 = indicesForPoints.TryGetValue(line.Point1, out var index1Out) ? (ushort?) index1Out : null;
                var index2 = indicesForPoints.TryGetValue(line.Point2, out var index2Out) ? (ushort?) index2Out : null;
                if (!index1.HasValue || !index2.HasValue)
                    continue;

                WriteUShort(index1.Value);
                WriteUShort(index2.Value);
                WriteShort(new CompressedFIXED(line.Angle / 180.0f, 0).RawShort);
                WriteByte(line.Tag);
                WriteByte((line.FlagToDisable.HasValue && line.FlagToDisable.Value >= 0x201 && line.FlagToDisable.Value <= 0x2FF) ? (byte) (line.FlagToDisable & 0xFF) : (byte) 0);

                linesWrittenList.Add(line);
            }

            linesWritten = linesWrittenList.ToArray();
        }

        public void WriteCollisionBlocks(IEnumerable<IMPD_CollisionLine> lines, int fileChunkAddr, int ramChunkAddr) {
            // Write placeholder pointers for 16 * 16 blocks.
            var blocksPtr = (uint) CurrentOffset;
            for (var i = 0; i < 0x100; i++)
                WriteUInt(0);

            // We'll need the index for every line we want to write.
            var linesWithIndex = lines.Select((x, i) => (Line: x, Index: (ushort) i)).ToArray();

            // 16 * 16 tables, each terminated by 0xFFFF.
            for (var i = 0; i < 0x100; i++) {
                var blockX = i % 16;
                var blockY = i / 16;
                var linesIndicesForBlock = linesWithIndex
                    .Where(x => x.Line.BlockShouldCheck(blockX, blockY))
                    .Select(x => x.Index)
                    .ToArray();

                AtOffset(blocksPtr + (i * 0x04), pos => WriteUInt((uint) (pos - fileChunkAddr + ramChunkAddr)));
                foreach (var lineIndex in linesIndicesForBlock)
                    WriteUShort(lineIndex);

                WriteUShort(0xFFFF);
            }
        }
    }
}
