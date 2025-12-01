using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteMain(IMPD_File mpd) {
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

            WriteToAlignTo(4);
            var headerPos = CurrentOffset;
            WriteHeader(
                mpd.MPDHeader,
                mpd.Settings,
                mpd.Flags,
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
        }

        public void WriteHeader(
            MPD_HeaderModel header,
            IMPD_Settings settings,
            IMPD_AllFlags flags,
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
            WriteUShort(settings.ModelsViewDistance);
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
            WriteShort(new CompressedFIXED(settings.ModelsYRotation / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(settings.ModelsViewAngleMin / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(settings.ModelsViewAngleMax / 180.0f, 0).RawShort);
            WriteMPDPointer(textureAnimAltPos);
            WriteMPDPointer(palette1Pos ?? headerAddr);
            WriteMPDPointer(palette2Pos ?? headerAddr);
            WriteShort(settings.GroundX);
            WriteShort(settings.GroundY);
            WriteShort(settings.GroundZ);
            WriteShort(new CompressedFIXED(settings.GroundXRotation / 180.0f, 0).RawShort);
            WriteShort(header.Unknown1);
            WriteShort(settings.BackgroundX);
            WriteShort(settings.BackgroundY);
            WriteMPDPointer(boundariesPos);
        }
    }
}
