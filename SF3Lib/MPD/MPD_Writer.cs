using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.SGL;
using SF3.Files;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public class MPD_Writer : BinaryFileWriter {
        public MPD_Writer(Stream stream, ScenarioType scenario) : base(stream) {
            Scenario = scenario;
        }

        public override void Finish() {
            // TODO: do what's necessary to finish.
        }

        /// <summary>
        /// Writes an entire MPD_File's contents to the stream.
        /// </summary>
        /// <param name="mpd">The MPD_File to write to the stream.</param>
        public void Write(MPD_File mpd) {
            // VOID.MPD tables:
            // X 0x0000: Offset of pointer to header (int), followed by 8 bytes of 0x00
            // X 0x000C: Light palette
            // X 0x004C: Light positions
            // X 0x0050: Unknown1
            // X 0x0090: Model switch groups
            // X 0x0094: Texture animations
            // X 0x0096: Unknown2
            // X 0x0098: Ground animation
            // X 0x009A: Boundaries
            // X 0x00AA: Texture anim alt
            // X 0x00AC: Palette 1 (actually header)
            // X 0x00AC: Palette 2 (also actually header)
            // X 0x00AC: Header
            // X 0x0104: Pointer to header
            // X 0x2000: Chunk table
            // - 0x2100: Model chunk
            // - 0x2898: Surface (compressed)
            // - 0x2C3C: Textures 1
            // - 0x32B0: Textures 2
            // - 0x32B8: Textures 3
            // - 0x32C0: Textures 4
            // - 0x32C8: Textures 5
            // - 0x32D0: Chest 1 Textures
            // - 0x3DB0: Chest 2 Textures
            // - 0x46F4: Barrel Textures

            // Placeholder for a pointer to the header with 8 bytes of padding.
            WriteBytes(new byte[0x0C]);

            var lightPalettePos      = WriteDataOrNull(mpd.LightPalette);
            var lightPositionPos     = WriteDataOrNull(mpd.LightPosition);
            var unknown1Pos          = WriteDataOrNull(mpd.Unknown1Table);
            var modelSwitchGroupsPos = WriteDataOrNull(mpd.ModelSwitchGroupsTable);
            var textureAnimationsPos = WriteDataOrNull(mpd.TextureAnimations);
            var unknown2Pos          = WriteDataOrNull(mpd.Unknown2Table);
            var groundAnimationPos   = WriteDataOrNull(mpd.GroundAnimationTable);
            var boundariesPos        = WriteDataOrNull(mpd.BoundariesTable);
            var textureAnimAltPos    = WriteDataOrNull(mpd.TextureAnimationsAlt);
            var palette1Pos          = WriteDataOrNull(mpd.PaletteTables?.Length >= 1 ? mpd.PaletteTables[0] : null);
            var palette2Pos          = WriteDataOrNull(mpd.PaletteTables?.Length >= 2 ? mpd.PaletteTables[1] : null);

            var headerPos = CurrentOffset;
            WriteMPDHeader(
                mpd.MPDHeader,
                mpd.MPDFlags,
                lightPalettePos,
                lightPositionPos,
                unknown1Pos,
                modelSwitchGroupsPos,
                textureAnimationsPos,
                unknown2Pos,
                groundAnimationPos,
                textureAnimAltPos,
                palette1Pos,
                palette2Pos,
                boundariesPos
            );

            // Write a pointer to the header.
            var headerPtrPos = CurrentOffset;
            WriteUInt((uint) (headerPos + 0x290000));

            // Write a *double pointer* to the header at the start of the file.
            AtOffset(0, _ => WriteUInt((uint) (headerPtrPos + 0x290000)));

            // Write a blank chunk table. We're going to flesh it out as we write chunks.
            WriteBytes(new byte[0x2100 - CurrentOffset]);

            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // TODO: check for this, and get memory mapping stuff!!
            // Chunk[1] is always models if it exists.
            var mc = mpd.ModelCollections.FirstOrDefault(x => x?.CollectionType == ModelCollectionType.PrimaryModels);
            if (mc == null)
                WriteEmptyChunk();
            else
                WriteModelChunk(mc.GetSGLModels(), mc.GetModelInstances());

            // TODO: actual chunks!!
            int chunkTableSize = 20;
            for (int i = 2; i < chunkTableSize; i++)
                WriteEmptyChunk();

            Finish();
        }

        private void WriteMPDPointer(int? offset)
            => WriteInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        private void WriteMPDPointer(uint? offset)
            => WriteUInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        public void WriteColorTable(ColorTable colorTable) {
            foreach (var color in colorTable)
                WriteUShort(color.ColorABGR1555);
        }

        public void WriteLightPosition(LightPosition lightPosition) {
            WriteShort(new CompressedFIXED(lightPosition.Pitch / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(lightPosition.Yaw / 180.0f, 0).RawShort);
        }

        public void WriteUInt8Table(UnknownUInt8Table table) {
            foreach (var value in table)
                WriteByte(value.Value);
            if (table.ReadUntil.HasValue)
                WriteByte(table.ReadUntil.Value);
        }

        public void WriteUInt16Table(UnknownUInt16Table table) {
            foreach (var value in table)
                WriteUShort(value.Value);
            if (table.ReadUntil.HasValue)
                WriteUShort(table.ReadUntil.Value);
        }

        public void WriteUInt32Table(UnknownUInt32Table table) {
            foreach (var value in table)
                WriteUInt(value.Value);
            if (table.ReadUntil.HasValue)
                WriteInt(table.ReadUntil.Value);
        }

        public void WriteModelSwitchGroups(ModelSwitchGroupsTable modelSwitchGroups) {
            // TODO: Write the things
            WriteUInt(0xFFFFFFFF);
        }

        public void WriteTextureAnimations(TextureAnimationTable textureAnimations) {
            // TODO: Write the things
            if (textureAnimations.Is32Bit)
                WriteUInt(textureAnimations.TextureEndId);
            else
                WriteUShort((ushort) textureAnimations.TextureEndId);
        }

        public void WriteBoundaries(BoundaryTable boundaries) {
            foreach (var boundary in boundaries) {
                WriteShort(boundary.X1);
                WriteShort(boundary.Z1);
                WriteShort(boundary.X2);
                WriteShort(boundary.Z2);
            }
        }

        public void WriteTextureIDs(TextureIDTable textureIds) {
            foreach (var textureId in textureIds)
                WriteUShort(textureId.TextureID);
            WriteUShort(0xFFFF);
        }

        public void WriteMPDHeader(
            MPDHeaderModel header,
            IMPD_Flags flags,
            uint? lightPalettePos,
            uint? lightPositionPos,
            uint? unknown1Pos,
            uint? modelSwitchGroupsPos,
            uint? textureAnimationsPos,
            uint? unknown2Pos,
            uint? groundAnimationPos,
            uint? textureAnimAltPos,
            uint? palette1Pos,
            uint? palette2Pos,
            uint? boundariesPos
        ) {
            var headerAddr = (uint) CurrentOffset;

            // TODO: determine proper map flags
            WriteUShort(header.MapFlags);
            WriteMPDPointer(lightPalettePos);
            WriteMPDPointer(lightPositionPos);
            WriteMPDPointer(unknown1Pos);
            WriteUShort(header.ViewDistance);
            WriteMPDPointer(modelSwitchGroupsPos);
            WriteMPDPointer(textureAnimationsPos);
            WriteMPDPointer(unknown2Pos);
            WriteMPDPointer(groundAnimationPos);
            // TODO: mesh1pos
            WriteMPDPointer(null);
            // TODO: mesh2pos
            WriteMPDPointer(null);
            // TODO: mesh3pos
            WriteMPDPointer(null);
            WriteShort(new CompressedFIXED(header.ModelsPreYRotation / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(header.ModelsViewAngleMin / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(header.ModelsViewAngleMax / 180.0f, 0).RawShort);
            WriteMPDPointer(textureAnimAltPos);
            WriteMPDPointer(palette1Pos ?? headerAddr);
            WriteMPDPointer(palette2Pos ?? headerAddr);
            WriteShort(header.GroundX);
            WriteShort(header.GroundY);
            WriteShort(header.GroundZ);
            WriteShort(new CompressedFIXED(header.GroundAngle, 0).RawShort);
            WriteShort(header.Unknown1);
            WriteShort(header.BackgroundX);
            WriteShort(header.BackgroundY);
            WriteMPDPointer(boundariesPos);
        }

        public void WriteEmptyChunk() {
            AtOffset(0x2000 + Chunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));
            Chunks++;
        }

        private uint? WriteDataOrNull<T>(T data) where T : class {
            if (data == null)
                return null;
            var pos = (uint) CurrentOffset;

            switch (data) {
                case ColorTable ct:              WriteColorTable(ct);   break;
                case LightPosition lp:           WriteLightPosition(lp);   break;
                case UnknownUInt32Table ui32:    WriteUInt32Table(ui32); break;
                case UnknownUInt16Table ui16:    WriteUInt16Table(ui16); break;
                case UnknownUInt8Table ui8:      WriteUInt8Table(ui8);  break;
                case ModelSwitchGroupsTable msg: WriteModelSwitchGroups(msg);  break;
                case TextureAnimationTable ta:   WriteTextureAnimations(ta);   break;
                case BoundaryTable bt:           WriteBoundaries(bt);   break;
                case TextureIDTable tid:         WriteTextureIDs(tid);  break;
            }

            WriteToAlignTo(2);
            return pos;
        }

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
                    WriteInstance(inst, fileChunkAddr, ramChunkAddr);
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

        public void WriteInstance(IMPD_ModelInstance instance, int fileChunkAddr, int ramChunkAddr) {
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

        public ScenarioType Scenario { get; }
        public int Chunks { get; private set; } = 0;

        private Dictionary<int, List<long>> _pdataIdToOffsetPtrMap = new Dictionary<int, List<long>>();
    }
}
