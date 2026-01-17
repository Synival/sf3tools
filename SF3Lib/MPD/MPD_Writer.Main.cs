using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Geometry;
using CommonLib.Imaging;
using CommonLib.SGL;
using CommonLib.Utils;
using SF3.Extensions;
using SF3.Imaging;
using SF3.Types;

namespace SF3.MPD {
    public partial class MPD_Writer {
        public void WriteMain(IMPD mpd, out byte[] chunk3Data) {
            // Placeholder for a pointer to the header with 8 bytes of padding.
            WriteBytes(new byte[0x0C]);

            _ = mpd.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var pmc);
            var animations = pmc?.Textures?.Where(x => x.Animation != null && !x.Animation.IsIgnored)?.ToArray();
            var ignoredTextureIds = pmc?.Textures?.Where(x => x.IsIgnored).Select(x => (ushort) x.ID)?.ToArray() ?? null;

            var lightPalettePos      = WritePaletteOrNull(mpd.Lighting?.Palette);
            var lightPositionPos     = WriteLightPosition(mpd.Lighting);

            // An "unknown 1" table in Scenario 1 was replaced with a "palette adjustment" struct in Scenario 2+.
            uint? unknown1OrPaletteAdjustPos = null;
            if (Scenario < ScenarioType.Scenario2)
                unknown1OrPaletteAdjustPos = WriteUnknownUInt16TableOrNull(mpd.Scenario1UnknownTable1);
            else
                unknown1OrPaletteAdjustPos = WritePaletteAdjustmentOrNull(mpd.Settings);
            WriteToAlignTo(2);

            // Some Scenario 2 maps have some junk after the "palette adjustment" struct.
            if (mpd.UnreferencedDataAfterPaletteAdjustment != null)
                WriteBytes(mpd.UnreferencedDataAfterPaletteAdjustment.ToArray());

            var modelSwitchGroupsPos = WriteModelSwitchGroupsOrNull(mpd.ModelSwitchGroups);

            var animationsPos = WriteAnimations(animations, mpd.Settings.ShortEmptyAnimationTable, out chunk3Data,
                is32Bit: Scenario >= ScenarioType.Scenario3,
                allowIndexed: Scenario >= ScenarioType.Scenario3
            );

            // An "unknown 2" table in Scenario 1 was replaced with a "gradient" struct in Scenario 2+.
            uint? unknown2OrGradientPos = null;
            if (Scenario < ScenarioType.Scenario2)
                unknown2OrGradientPos = WriteUnknownUInt16TableOrNull(mpd.Scenario1UnknownTable2);
            else {
                // Always set, whether it exists or not. It's structured like a table for some reason.
                unknown2OrGradientPos = (uint) CurrentOffset;
                if (mpd.Gradient != null)
                    WriteGradient(mpd.Gradient);
                WriteUShort(0xFFFF);
            }

            WriteToAlignTo(4);

            var groundAnimationPos   = WriteGroundAnimationOrNull(mpd.GroundAnimation);
            var boundariesPos        = WriteBoundariesTableOrNull(mpd.CameraBoundaries, mpd.BattleCursorBoundaries);
            var ignoredTexturesPos   = WriteIgnoredTexturesTableOrNull(ignoredTextureIds, mpd.Settings.LongEmptyIgnoredTextureTable);

            uint? groundPalettePos   = null;
            uint? skyPalettePos      = null;
            uint? texturePalettePos  = null;
            uint? indexedTexturesPos = null;

            // Scenario 2- has two palettes that may or may not exist (and the logic for placing them is weird).
            if (Scenario < ScenarioType.Scenario3) {
                groundPalettePos = WritePaletteOrNull(mpd.Planes?.GroundPalette?.Channels?.Length >= 1 ? mpd.Planes.GroundPalette : null);

                skyPalettePos = mpd.Settings.SkyPaletteSharesGroundPalette
                    ? groundPalettePos
                    : WritePaletteOrNull(mpd.Planes?.SkyPalette?.Channels?.Length >= 1 ? mpd.Planes.SkyPalette : null);
            }
            // Scenario 3+ has three palettes with different placement logic and an extra table.
            // All three palettes are guaranteed to exist in Scenario 3.
            else {
                groundPalettePos = WritePaletteOrNull(mpd.Planes?.GroundPalette?.Channels?.Length >= 1 ? mpd.Planes.GroundPalette : null) ?? (uint) CurrentOffset;

                skyPalettePos = mpd.Settings.SkyPaletteSharesGroundPalette
                    ? groundPalettePos
                    : WritePaletteOrNull(mpd.Planes?.SkyPalette?.Channels?.Length >= 1 ? mpd.Planes.SkyPalette : null) ?? groundPalettePos;

                texturePalettePos = mpd.Settings.TexturePaletteSharesSkyPalette
                    ? skyPalettePos
                    : WritePaletteOrNull(mpd.TexturePalette?.Channels?.Length >= 1 ? mpd.TexturePalette : null) ?? skyPalettePos;

                var indexedTextureIds = pmc?.Textures?.Where(x => x.BytesPerPixel == 1).Select(x => (ushort) x.ID)?.ToArray() ?? null;
                indexedTexturesPos = WriteIndexedTexturesTableOrNull(indexedTextureIds);
            }

            WriteToAlignTo(4);
            var headerPos = CurrentOffset;
            WriteHeader(
                mpd,
                lightPalettePos,
                lightPositionPos,
                unknown1OrPaletteAdjustPos,
                modelSwitchGroupsPos,
                animationsPos,
                unknown2OrGradientPos,
                groundAnimationPos,
                ignoredTexturesPos,
                groundPalettePos,
                skyPalettePos,
                texturePalettePos,
                indexedTexturesPos,
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

            var chestModelsPos       = WriteHeaderModelsOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Chest,       out chestChunk)       == true) ? chestChunk       : null);
            var lockedChestModelsPos = WriteHeaderModelsOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.LockedChest, out lockedChestChunk) == true) ? lockedChestChunk : null);
            var barrelModelsPos      = WriteHeaderModelsOrNull((mpd.ModelCollections?.TryGetValue(MPD_CollectionType.Barrel,      out barrelChunk)      == true) ? barrelChunk      : null);

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
            IMPD mpd,
            uint? lightPalettePos,
            uint? lightPositionPos,
            uint? unknown1OrPaletteAdjustPos,
            uint? modelSwitchGroupsPos,
            uint? animationsPos,
            uint? unknown2OrGradientPos,
            uint? groundAnimationPos,
            uint? ignoredTexturesPos,
            uint? groundPalettePos,
            uint? skyPalettePos,
            uint? texturePalettePos,
            uint? indexedTexturesPos,
            uint? boundariesPos,
            out uint chestModelsPosPtr,
            out uint lockedChestModelsPosPtr,
            out uint barrelModelsPosPtr
        ) {
            var settings = mpd.Settings;
            var planes   = mpd.Planes;

            var headerAddr = (uint) CurrentOffset;

            WriteUShort(mpd.GetHeaderFlags(Scenario));
            WriteMPDPointer(lightPalettePos);
            WriteMPDPointer(lightPositionPos);
            WriteMPDPointer(unknown1OrPaletteAdjustPos);
            WriteUShort(settings.ModelsViewDistance);
            WriteMPDPointer(modelSwitchGroupsPos);
            WriteMPDPointer(animationsPos);
            WriteMPDPointer(unknown2OrGradientPos);
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

            // Scenario 3+ has extra tables.
            if (Scenario >= ScenarioType.Scenario3) {
                WriteMPDPointer(texturePalettePos);
                WriteMPDPointer(indexedTexturesPos);
            }

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

        public uint? WriteGroundAnimationOrNull(IEnumerable<byte> table)
            => WriteObjectOrNull(() => table != null, () => WriteGroundAnimation(table));

        public void WriteGroundAnimation(IEnumerable<byte> table) {
            foreach (var value in table)
                WriteByte(value);
            WriteUShort(0xFF00);
        }

        public uint? WriteUnknownUInt16TableOrNull(IEnumerable<ushort> table)
            => WriteObjectOrNull(() => table != null, () => WriteUnknownUInt16Table(table));

        public void WriteUnknownUInt16Table(IEnumerable<ushort> table) {
            foreach (var value in table)
                WriteUShort(value);
        }

        public uint? WritePaletteAdjustmentOrNull(IMPD_Settings settings)
            => WriteObjectOrNull(() => settings.LightPaletteAdjustment != null, () => WritePaletteAdjustment(settings));

        public void WritePaletteAdjustment(IMPD_Settings settings) {
            WriteShort(settings.LightPaletteAdjustment?.R ?? 0);
            WriteShort(settings.LightPaletteAdjustment?.G ?? 0);
            WriteShort(settings.LightPaletteAdjustment?.B ?? 0);

            if (Scenario >= ScenarioType.Scenario3 && !settings.PaletteAdjustmentIsTruncated) {
                WriteShort(settings.GroundPaletteAdjustment?.R ?? 0);
                WriteShort(settings.GroundPaletteAdjustment?.G ?? 0);
                WriteShort(settings.GroundPaletteAdjustment?.B ?? 0);
                WriteShort(settings.ShadowTransparency);
            }
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

        public uint WriteAnimations(IMPD_AnimatableTexture[] textures, bool shortEmptyTable, out byte[] chunk3Data, bool is32Bit, bool allowIndexed) {
            var pos = (uint) CurrentOffset;
            textures = (textures ?? new IMPD_AnimatableTexture[0]).Where(x => x.Animation != null && !x.IsIgnored).ToArray();

            var chunk3DataArray = textures.Length > 0 ? new ByteArray(0x10000) : null;
            var chunk3Size = 0;

            var textureDataByPos = new Dictionary<ushort, byte[]>();

            ushort AddTextureToChunk3(byte[] textureData) {
                var existingEntry = textureDataByPos.FirstOrDefault(x => Enumerable.SequenceEqual(textureData, x.Value));
                if (existingEntry.Value != null)
                    return existingEntry.Key;

                var compressedTextureData = Compression.CompressLZSS(textureData);

                var chunk3WritePos = (ushort) chunk3Size;
                chunk3Size = chunk3WritePos + compressedTextureData.Length;
                if (chunk3Size % 4 != 0)
                    chunk3Size += 4 - (chunk3Size % 4);

                while (chunk3Size > chunk3DataArray.Length)
                    chunk3DataArray.Resize(chunk3Size + 0x10000);

                chunk3DataArray.SetDataAtTo(chunk3WritePos, compressedTextureData.Length, compressedTextureData);
                textureDataByPos.Add(chunk3WritePos, textureData);

                return chunk3WritePos;
            }

            // Special case for a few very specific files.
            foreach (var tex in textures) {
                if (is32Bit) {
                    WriteUInt((uint) (tex.ID | ((allowIndexed && tex.BytesPerPixel == 1) ? 0x100 : 0)));
                    WriteUInt((uint) tex.Width);
                    WriteUInt((uint) tex.Height);
                    WriteUInt((uint) tex.Animation.FrameTimerStart);
                }
                else {
                    WriteUShort((ushort) tex.ID);
                    WriteUShort((ushort) tex.Width);
                    WriteUShort((ushort) tex.Height);
                    WriteUShort((ushort) tex.Animation.FrameTimerStart);
                }

                foreach (var frame in tex.Animation.Frames) {
                    var imageData = (frame.BytesPerPixel == 2)
                        ? frame.ImageData16Bit.To1DArrayTransposed().ToByteArray()
                        : frame.ImageData8Bit.To1DArrayTransposed();

                    if (frame.BytesPerPixel == 1 && !allowIndexed)
                        imageData = imageData.ConvertIndexedToABGR1555(frame.Palette).ToByteArray();

                    var chunk3FramePos = AddTextureToChunk3(imageData);

                    if (is32Bit) {
                        WriteUInt((uint) chunk3FramePos);
                        WriteUInt((uint) frame.Duration);
                    }
                    else {
                        WriteUShort((ushort) chunk3FramePos);
                        WriteUShort((ushort) frame.Duration);
                    }
                }

                if (is32Bit)
                    WriteUInt(0xFFFFFFFE);
                else
                    WriteUShort(0xFFFE);
            }

            if (is32Bit) {
                WriteUInt(0xFFFFFFFF);
                WriteUInt(0xFFFFFFFF);
            }
            else {
                WriteUShort(0xFFFF);
                if (!shortEmptyTable)
                    WriteUShort(0xFFFF);
            }

            if (chunk3DataArray == null)
                chunk3Data = null;
            else {
                if (chunk3Size % 4 != 0)
                    chunk3Size += 4 - (chunk3Size % 4);
                chunk3Data = chunk3DataArray.GetDataCopyAt(0, chunk3Size);
            }

            return pos;
        }

        public void WriteGradient(IMPD_Gradient gradient) {
            if (gradient.IsDummiedOut)
                WriteUShort(0xFFFF);

            WriteUShort((ushort) Math.Round(gradient.TopPosition * 255.00f));
            WriteUShort((ushort) Math.Round(gradient.BottomPosition * 255.00f));

            WriteUShort(gradient.TopColor.R);
            WriteUShort(gradient.TopColor.G);
            WriteUShort(gradient.TopColor.B);

            WriteUShort(gradient.BottomColor.R);
            WriteUShort(gradient.BottomColor.G);
            WriteUShort(gradient.BottomColor.B);

            var partsAffectedBits = (ushort) (
                (gradient.AffectsGround           ? 0x01 : 0) |
                (gradient.AffectsSky              ? 0x02 : 0) |
                (gradient.AffectsModelsAndSurface ? 0x04 : 0)
            );

            WriteUShort(partsAffectedBits);

            WriteUShort((ushort) Math.Round(gradient.GroundIntensity * 0x1F));
            WriteUShort((ushort) Math.Round(gradient.SkyIntensity * 0x1F));
            WriteUShort((ushort) Math.Round(gradient.ModelsAndSurfaceIntensity * 0x1F));
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

        public uint? WriteIndexedTexturesTableOrNull(ushort[] textureIds)
            => WriteObjectOrNull(() => textureIds != null, () => WriteIndexedTexturesTable(textureIds));

        public void WriteIndexedTexturesTable(ushort[] textureIds) {
            foreach (var textureId in textureIds.OrderBy(x => x).ToArray())
                WriteUShort(textureId);
            WriteUInt(0xFFFFFFFF);
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
