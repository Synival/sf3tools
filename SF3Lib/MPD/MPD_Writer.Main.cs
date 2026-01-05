using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteMain(ScenarioType scenario, IMPD_File mpd) {
            // Placeholder for a pointer to the header with 8 bytes of padding.
            WriteBytes(new byte[0x0C]);

            var lightPalettePos      = WriteTableOrNull(mpd.LightPalette);
            var lightPositionPos     = WriteTableOrNull(mpd.LightPosition);
            var unknown1Pos          = WriteTableOrNull(mpd.Unknown1Table);
            var modelSwitchGroupsPos = WriteTableOrNull(mpd.ModelSwitchGroupsTable);
            var animationsPos        = WriteTableOrNull(mpd.Animations, mpd.Settings);
            var unknown2Pos          = WriteTableOrNull(mpd.Unknown2Table);
            WriteToAlignTo(4);
            var groundAnimationPos   = WriteTableOrNull(mpd.GroundAnimationTable);
            var boundariesPos        = WriteTableOrNull(mpd.BoundariesTable);
            var skipTexturesPos      = WriteTableOrNull(mpd.SkipTextures, mpd.Settings);
            var palette1Pos          = WriteTableOrNull(mpd.GroundPaletteColorTable?.Length >= 1 ? mpd.GroundPaletteColorTable : null);
            var palette2Pos          = WriteTableOrNull(mpd.SkyPaletteColorTable?.Length >= 2 ? mpd.SkyPaletteColorTable : null);

            WriteToAlignTo(4);
            var headerPos = CurrentOffset;
            WriteHeader(
                scenario,
                mpd.Settings,
                mpd.Flags,
                mpd.Planes,
                lightPalettePos,
                lightPositionPos,
                unknown1Pos,
                modelSwitchGroupsPos,
                animationsPos,
                unknown2Pos,
                groundAnimationPos,
                skipTexturesPos,
                palette1Pos,
                palette2Pos,
                boundariesPos,
                out var chestModelsPosPtr,
                out var lockedChestModelsPosPtr,
                out var barrelModelsPosPtr
            );

            // Write a pointer to the header.
            var headerPtrPos = CurrentOffset;
            WriteUInt((uint) (headerPos + 0x290000));

            // Write the chest/barrel models, if available, and update the main header pointers.
            IMPD_ModelCollection chestChunk       = null;
            IMPD_ModelCollection lockedChestChunk = null;
            IMPD_ModelCollection barrelChunk      = null;

            var chestModelsPos       = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Chest,       out chestChunk)       == true) ? chestChunk       : null);
            var lockedChestModelsPos = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.LockedChest, out lockedChestChunk) == true) ? lockedChestChunk : null);
            var barrelModelsPos      = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Barrel,      out barrelChunk)      == true) ? barrelChunk      : null);

            if (chestModelsPos.HasValue && chestChunk?.IsUnreferenced != true)
                AtOffset(chestModelsPosPtr, _ => WriteUInt(chestModelsPos.Value + 0x290000));
            if (lockedChestModelsPos.HasValue && lockedChestChunk?.IsUnreferenced != true)
                AtOffset(lockedChestModelsPosPtr, _ => WriteUInt(lockedChestModelsPos.Value + 0x290000));
            if (barrelModelsPos.HasValue && barrelChunk?.IsUnreferenced != true)
                AtOffset(barrelModelsPosPtr, _ => WriteUInt(barrelModelsPos.Value + 0x290000));

            // Write a *double pointer* to the header at the start of the file.
            AtOffset(0, _ => WriteUInt((uint) (headerPtrPos + 0x290000)));
        }

        public void WriteHeader(
            ScenarioType scenario,
            IMPD_Settings settings,
            IMPD_AllFlags flags,
            IMPD_Planes planes,
            uint? lightPalettePos,
            uint? lightPositionPos,
            uint? unknown1Pos,
            uint? modelSwitchGroupsPos,
            uint? animationsPos,
            uint? unknown2Pos,
            uint? groundAnimationPos,
            uint? skipTexturesPos,
            uint? palette1Pos,
            uint? palette2Pos,
            uint? boundariesPos,
            out uint chestModelsPosPtr,
            out uint lockedChestModelsPosPtr,
            out uint barrelModelsPosPtr
        ) {
            var headerAddr = (uint) CurrentOffset;

            // TODO: determine proper map flags
            WriteUShort(flags.GetHeaderFlags(scenario));
            WriteMPDPointer(lightPalettePos);
            WriteMPDPointer(lightPositionPos);
            WriteMPDPointer(unknown1Pos);
            WriteUShort(settings.ModelsViewDistance);
            WriteMPDPointer(modelSwitchGroupsPos);
            WriteMPDPointer(animationsPos);
            WriteMPDPointer(unknown2Pos);
            WriteMPDPointer(groundAnimationPos);

            // These are written afterwards; provide the pointer address so it can be updated
            chestModelsPosPtr = (uint) CurrentOffset;
            WriteMPDPointer(null); 
            lockedChestModelsPosPtr = (uint) CurrentOffset;
            WriteMPDPointer(null);
            barrelModelsPosPtr = (uint) CurrentOffset;
            WriteMPDPointer(null);

            WriteShort(new CompressedFIXED(settings.ModelsYRotation / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(settings.ModelsViewAngleMin / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(settings.ModelsViewAngleMax / 180.0f, 0).RawShort);
            WriteMPDPointer(skipTexturesPos);
            WriteMPDPointer(palette1Pos ?? headerAddr);
            WriteMPDPointer(palette2Pos ?? headerAddr);
            WriteShort(planes.GroundX);
            WriteShort(planes.GroundY);
            WriteShort(planes.GroundZ);
            WriteShort(new CompressedFIXED(planes.GroundXRotation / 180.0f, 0).RawShort);
            WriteShort(settings.UnknownHeaderSetting);
            WriteShort(planes.BackgroundX);
            WriteShort(planes.BackgroundY);
            WriteMPDPointer(boundariesPos);
        }
    }
}
