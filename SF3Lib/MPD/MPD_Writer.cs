using System;
using System.IO;
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
            // - 0x00AC: Header
            // - 0x0104: Pointer to header
            // - 0x2000: Chunk table
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

            Write(new byte[0x0C]);

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
            WriteHeader(
                mpd.MPDHeader,
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
            Write((uint) (headerPos + 0x290000));

            // Write a *double pointer* to the header at the start of the file.
            AtOffset(0, _ => Write((uint) (headerPtrPos + 0x290000)));

            // Write a blank chunk table. We're going to flesh it out as we write chunks.
            Write(new byte[0x2100 - CurrentOffset]);

            // Chunk[0] is always empty.
            WriteEmptyChunk();

            // TODO: actual chunks!!
            int chunkTableSize = 20;
            for (int i = 1; i < chunkTableSize; i++)
                WriteEmptyChunk();

            Finish();
        }

        public void Write(byte value)
            => Write(new byte[] { value });

        public void Write(ushort[] values) {
            foreach (var value in values)
                Write(value);
        }

        public void Write(ushort value) {
            WriteToAlignTo(2);
            Write(new byte[] {
                (byte) (value >> 8),
                (byte) value
            });
        }

        public void Write(uint[] values) {
            foreach (var value in values)
                Write(value);
        }

        public void Write(short value)
            => Write((ushort) value);

        public void Write(short[] values) {
            foreach (var value in values)
                Write(value);
        }

        public void Write(uint value) {
            WriteToAlignTo(4);
            Write(new byte[] {
                (byte) (value >> 24),
                (byte) (value >> 16),
                (byte) (value >> 8),
                (byte) value
            });
        }

        public void Write(int[] values) {
            foreach (var value in values)
                Write(value);
        }

        public void Write(int value)
            => Write((uint) value);

        void WritePointer(uint? offset)
            => Write((uint) (offset.HasValue ? (offset.Value + 0x290000) : 0));

        public void Write(ColorTable colorTable) {
            foreach (var color in colorTable)
                Write(color.ColorABGR1555);
        }

        public void Write(LightPosition lightPosition) {
            Write(lightPosition.Pitch);
            Write(lightPosition.Yaw);
        }

        public void Write(UnknownUInt8Table table) {
            foreach (var value in table)
                Write(value.Value);
            if (table.ReadUntil.HasValue)
                Write(table.ReadUntil.Value);
        }

        public void Write(UnknownUInt16Table table) {
            foreach (var value in table)
                Write(value.Value);
            if (table.ReadUntil.HasValue)
                Write(table.ReadUntil.Value);
        }

        public void Write(UnknownUInt32Table table) {
            foreach (var value in table)
                Write(value.Value);
            if (table.ReadUntil.HasValue)
                Write(table.ReadUntil.Value);
        }

        public void Write(ModelSwitchGroupsTable modelSwitchGroups) {
            // TODO: Write the things
            Write(0xFFFFFFFF);
        }

        public void Write(TextureAnimationTable textureAnimations) {
            // TODO: Write the things
            if (textureAnimations.Is32Bit)
                Write((uint) textureAnimations.TextureEndId);
            else
                Write((ushort) textureAnimations.TextureEndId);
        }

        public void Write(BoundaryTable boundaries) {
            foreach (var boundary in boundaries) {
                Write(boundary.X1);
                Write(boundary.Z1);
                Write(boundary.X2);
                Write(boundary.Z2);
            }
        }

        public void Write(TextureIDTable textureIds) {
            foreach (var textureId in textureIds)
                Write(textureId.TextureID);
            Write((ushort) 0xFFFF);
        }

        public void WriteHeader(
            MPDHeaderModel header,
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
            Write((ushort) header.MapFlags);
            WritePointer(lightPalettePos);
            WritePointer(lightPositionPos);
            WritePointer(unknown1Pos);
            Write((ushort) header.ViewDistance);
            WritePointer(modelSwitchGroupsPos);
            WritePointer(textureAnimationsPos);
            WritePointer(unknown2Pos);
            WritePointer(groundAnimationPos);
            // TODO: mesh1pos
            WritePointer(null);
            // TODO: mesh2pos
            WritePointer(null);
            // TODO: mesh3pos
            WritePointer(null);
            Write((ushort) new CompressedFIXED(header.ModelsPreYRotation, 0).RawShort);
            Write((ushort) new CompressedFIXED(header.ModelsViewAngleMin, 0).RawShort);
            Write((ushort) new CompressedFIXED(header.ModelsViewAngleMax, 0).RawShort);
            WritePointer(textureAnimAltPos);
            WritePointer(palette1Pos ?? headerAddr);
            WritePointer(palette2Pos ?? headerAddr);
            Write((ushort) header.GroundX);
            Write((ushort) header.GroundY);
            Write((ushort) header.GroundZ);
            Write((ushort) new CompressedFIXED(header.GroundAngle, 0).RawShort);
            Write((ushort) header.Unknown1);
            Write((ushort) header.BackgroundX);
            Write((ushort) header.BackgroundY);
            WritePointer(boundariesPos);
        }

        public void WriteEmptyChunk() {
            AtOffset(0x2000 + Chunks * 0x08, curOffset => WritePointer((uint) curOffset));
            Chunks++;
        }

        private uint? WriteDataOrNull<T>(T data) where T : class {
            if (data == null)
                return null;
            var pos = (uint) CurrentOffset;

            switch (data) {
                case ColorTable ct:              Write(ct);   break;
                case LightPosition lp:           Write(lp);   break;
                case UnknownUInt32Table ui32:    Write(ui32); break;
                case UnknownUInt16Table ui16:    Write(ui16); break;
                case UnknownUInt8Table ui8:      Write(ui8);  break;
                case ModelSwitchGroupsTable msg: Write(msg);  break;
                case TextureAnimationTable ta:   Write(ta);   break;
                case BoundaryTable bt:           Write(bt);   break;
                case TextureIDTable tid:         Write(tid);  break;
            }

            WriteToAlignTo(2);
            return pos;
        }

        public ScenarioType Scenario { get; }
        public int Chunks { get; private set; } = 0;
    }
}
