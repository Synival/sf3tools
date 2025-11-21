using System.Collections.Generic;
using System.IO;
using System.Linq;
using SF3.Files;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Performs the writing of binary data to an MPD file.
    /// </summary>
    public partial class MPD_Writer : BinaryFileWriter {
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
        public void WriteMPD(MPD_File mpd) {
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
            // X 0x2100: Model chunk
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

            var lightPalettePos      = WriteTableOrNull(mpd.LightPalette);
            var lightPositionPos     = WriteTableOrNull(mpd.LightPosition);
            var unknown1Pos          = WriteTableOrNull(mpd.Unknown1Table);
            var modelSwitchGroupsPos = WriteTableOrNull(mpd.ModelSwitchGroupsTable);
            var textureAnimationsPos = WriteTableOrNull(mpd.TextureAnimations);
            var unknown2Pos          = WriteTableOrNull(mpd.Unknown2Table);
            var groundAnimationPos   = WriteTableOrNull(mpd.GroundAnimationTable);
            var boundariesPos        = WriteTableOrNull(mpd.BoundariesTable);
            var textureAnimAltPos    = WriteTableOrNull(mpd.TextureAnimationsAlt);
            var palette1Pos          = WriteTableOrNull(mpd.PaletteTables?.Length >= 1 ? mpd.PaletteTables[0] : null);
            var palette2Pos          = WriteTableOrNull(mpd.PaletteTables?.Length >= 2 ? mpd.PaletteTables[1] : null);

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

        public void WriteEmptyChunk() {
            AtOffset(0x2000 + Chunks * 0x08, curOffset => WriteMPDPointer((uint) curOffset));
            Chunks++;
        }

        private void WriteMPDPointer(int? offset)
            => WriteInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        private void WriteMPDPointer(uint? offset)
            => WriteUInt(offset.HasValue ? (offset.Value + 0x290000) : 0);

        public ScenarioType Scenario { get; }
        public int Chunks { get; private set; } = 0;

        private Dictionary<int, List<long>> _pdataIdToOffsetPtrMap = new Dictionary<int, List<long>>();
    }
}
