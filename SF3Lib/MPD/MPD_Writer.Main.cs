using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Geometry;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.Utils;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteMain(ScenarioType scenario, IMPD_File mpd, out byte[] chunk3Data) {
            // Placeholder for a pointer to the header with 8 bytes of padding.
            WriteBytes(new byte[0x0C]);

            _ = mpd.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var pmc);
            var animations = pmc?.Textures?.Where(x => x.Animation != null && !x.Animation.IsIgnored)?.ToArray();
            var ignoredTextureIds = pmc?.Textures?.Where(x => x.IsIgnored).Select(x => (ushort) x.ID)?.ToArray() ?? null;

            var lightPalettePos      = WritePaletteOrNull(mpd.Lighting?.Palette);
            var lightPositionPos     = WriteLightPosition(mpd.Lighting);
            var unknown1Pos          = WriteTableOrNull(mpd.Unknown1Table);
            var modelSwitchGroupsPos = WriteModelSwitchGroupsOrNull(mpd.ModelSwitchGroups);
            var animationsPos        = WriteAnimations(animations, mpd.Settings.ShortEmptyAnimationTable, out chunk3Data);
            var unknown2Pos          = WriteTableOrNull(mpd.Unknown2Table);
            WriteToAlignTo(4);
            var groundAnimationPos   = WriteTableOrNull(mpd.GroundAnimationTable);
            var boundariesPos        = WriteBoundariesTableOrNull(mpd.CameraBoundaries, mpd.BattleCursorBoundaries);
            var ignoredTexturesPos   = WriteIgnoredTexturesTableOrNull(ignoredTextureIds, mpd.Settings.LongEmptyIgnoredTextureTable);
            var groundPalettePos     = WritePaletteOrNull(mpd.Planes?.GroundPalette?.Channels?.Length >= 1 ? mpd.Planes.GroundPalette : null);
            var skyPalettePos        = WritePaletteOrNull(mpd.Planes?.SkyPalette?.Channels?.Length >= 1    ? mpd.Planes.SkyPalette    : null);

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
                ignoredTexturesPos,
                groundPalettePos,
                skyPalettePos,
                boundariesPos,
                out var chestModelsPosPtr,
                out var lockedChestModelsPosPtr,
                out var barrelModelsPosPtr
            );

            // Write a pointer to the header.
            var headerPtrPos = CurrentOffset;
            WriteMPDPointer((int) headerPos);

            // Write the chest/barrel models, if available, and update the main header pointers.
            IMPD_ModelCollection chestChunk       = null;
            IMPD_ModelCollection lockedChestChunk = null;
            IMPD_ModelCollection barrelChunk      = null;

            var chestModelsPos       = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Chest,       out chestChunk)       == true) ? chestChunk       : null);
            var lockedChestModelsPos = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.LockedChest, out lockedChestChunk) == true) ? lockedChestChunk : null);
            var barrelModelsPos      = WriteTableOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Barrel,      out barrelChunk)      == true) ? barrelChunk      : null);

            if (chestModelsPos.HasValue && chestChunk?.IsUnreferenced != true)
                AtOffset(chestModelsPosPtr, _ => WriteMPDPointer(chestModelsPos.Value));
            if (lockedChestModelsPos.HasValue && lockedChestChunk?.IsUnreferenced != true)
                AtOffset(lockedChestModelsPosPtr, _ => WriteMPDPointer(lockedChestModelsPos.Value));
            if (barrelModelsPos.HasValue && barrelChunk?.IsUnreferenced != true)
                AtOffset(barrelModelsPosPtr, _ => WriteMPDPointer(barrelModelsPos.Value));

            // Write a *double pointer* to the header at the start of the file.
            AtOffset(0, _ => WriteMPDPointer((int) headerPtrPos));
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
            uint? ignoredTexturesPos,
            uint? groundPalettePos,
            uint? skyPalettePos,
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
            WriteMPDPointer(ignoredTexturesPos);
            WriteMPDPointer(groundPalettePos ?? headerAddr);
            WriteMPDPointer(skyPalettePos ?? (groundPalettePos.HasValue ? groundPalettePos.Value + 0x200 : headerAddr));
            WriteShort(planes.GroundX);
            WriteShort(planes.GroundY);
            WriteShort(planes.GroundZ);
            WriteShort(new CompressedFIXED(planes.GroundXRotation / 180.0f, 0).RawShort);
            WriteShort(settings.UnknownHeaderSetting);
            WriteShort(planes.BackgroundX);
            WriteShort(planes.BackgroundY);
            WriteMPDPointer(boundariesPos);
        }

        public uint? WritePaletteOrNull(Palette palette)
            => WriteObjectOrNull(() => palette != null, () => WritePalette(palette));

        public void WritePalette(Palette palette) {
            foreach (var channel in palette.Channels)
                WriteUShort(channel.ToABGR1555());
        }

        public uint WriteLightPosition(IMPD_Lighting lighting) {
            WriteToAlignTo(2);
            var pos = (uint) CurrentOffset;

            WriteShort(new CompressedFIXED(lighting.Pitch / 180.0f, 0).RawShort);
            WriteShort(new CompressedFIXED(lighting.Yaw / 180.0f, 0).RawShort);

            return pos;
        }

        public uint? WriteModelSwitchGroupsOrNull(IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup> switchGroups) {
            if (switchGroups == null)
                return null;
            return WriteModelSwitchGroups(switchGroups);
        }

        public uint WriteModelSwitchGroups(IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup> switchGroups) {
            var offPositions = new uint[switchGroups.Length];
            var onPositions = new uint[switchGroups.Length];

            for (int i = 0; i < switchGroups.Length; i++) {
                var switchGroup = switchGroups[i];

                offPositions[i] = (uint) CurrentOffset;
                foreach (var modelId in switchGroup.ModelInstancesVisibleWhenOff)
                    WriteUShort((ushort) modelId);
                WriteUShort(0xFFFF);

                onPositions[i] = (uint) CurrentOffset;
                foreach (var modelId in switchGroup.ModelInstancesVisibleWhenOn)
                    WriteUShort((ushort) modelId);
                WriteUShort(0xFFFF);
            }

            WriteToAlignTo(4);
            var pos = (uint) CurrentOffset;
            for (int i = 0; i < switchGroups.Length; i++) {
                var switchGroup = switchGroups[i];
                WriteUInt((uint) switchGroup.Flag);
                WriteMPDPointer(offPositions[i]);
                WriteMPDPointer(onPositions[i]);
                WriteUInt(0);
            }
            WriteUInt(0xFFFFFFFF);

            return pos;
        }

        public uint WriteAnimations(IMPD_AnimatableTexture[] textures, bool shortEmptyTable, out byte[] chunk3Data) {
            var pos = (uint) CurrentOffset;
            textures = (textures ?? new IMPD_AnimatableTexture[0]).Where(x => x.Animation != null && !x.IsIgnored).ToArray();

            var chunk3DataArray = textures.Length > 0 ? new ByteArray(0x1000) : null;
            var chunk3BytesWritten = 0;

            // Special case for a few very specific files.
            foreach (var tex in textures) {
                WriteUShort((ushort) tex.ID);
                WriteUShort((ushort) tex.Width);
                WriteUShort((ushort) tex.Height);
                WriteUShort((ushort) tex.Animation.FrameTimerStart);

                foreach (var frame in tex.Animation.Frames) {
                    var compressedTexture = Compression.CompressLZSS(frame.ImageData16Bit.To1DArrayTransposed().ToByteArray());
                    var chunk3WritePos = chunk3BytesWritten;
                    chunk3BytesWritten += compressedTexture.Length;
                    if (chunk3BytesWritten > chunk3DataArray.Length)
                        chunk3DataArray.Resize(chunk3DataArray.Length + 0x1000);
                    chunk3DataArray.SetDataAtTo(chunk3WritePos, compressedTexture.Length, compressedTexture);

                    WriteUShort((ushort) chunk3WritePos);
                    WriteUShort((ushort) frame.Duration);
                }

                WriteUShort(0xFFFE);
            }

            WriteUShort(0xFFFF);
            if (!shortEmptyTable)
                WriteUShort(0xFFFF);

            if (chunk3DataArray == null)
                chunk3Data = null;
            else {
                if (chunk3BytesWritten % 4 != 0)
                    chunk3BytesWritten += 4 - (chunk3BytesWritten % 4);
                chunk3Data = chunk3DataArray.GetDataCopyAt(0, chunk3BytesWritten);
            }

            return pos;
        }

        public uint? WriteIgnoredTexturesTableOrNull(ushort[] textureIds, bool writeLongEmptyData)
            => WriteObjectOrNull(() => textureIds != null, () => WriteIgnoredTexturesTable(textureIds, writeLongEmptyData));

        public void WriteIgnoredTexturesTable(ushort[] textureIds, bool writeLongEmptyData) {
            foreach (var textureId in textureIds.OrderBy(x => x).ToArray())
                WriteUShort(textureId);

            if (writeLongEmptyData && textureIds.Length == 0) {
                WriteUShort(0xFFFF);
                WriteUShort(0xFFFF);
            }
            else
                WriteUShort(0xFFFF);
        }

        public uint? WriteBoundariesTableOrNull(IRectangleShort cameraBoundaries, IRectangleShort battleBoundaries)  
            => WriteObjectOrNull(() => cameraBoundaries != null && battleBoundaries != null, () => WriteBoundariesTable(cameraBoundaries, battleBoundaries));

        public void WriteBoundariesTable(IRectangleShort cameraBoundaries, IRectangleShort battleBoundaries) {
            WriteShort(cameraBoundaries.X1);
            WriteShort(cameraBoundaries.Y1);
            WriteShort(cameraBoundaries.X2);
            WriteShort(cameraBoundaries.Y2);

            WriteShort(battleBoundaries.X1);
            WriteShort(battleBoundaries.Y1);
            WriteShort(battleBoundaries.X2);
            WriteShort(battleBoundaries.Y2);
        }
    }
}
