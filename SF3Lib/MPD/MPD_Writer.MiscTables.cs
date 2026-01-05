using System;
using CommonLib.SGL;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.Animation;
using SF3.Models.Tables.MPD.Main;
using SF3.Models.Tables.Shared;

namespace SF3.MPD {
    public partial class MPD_Writer {
        private uint? WriteTableOrNull<T>(T data, IMPD_Settings settings = null) where T : class {
            if (data == null)
                return null;
            var pos = (uint) CurrentOffset;

            switch (data) {
                case LightPosition lp:           WriteLightPosition(lp);       break;
                case UnknownUInt32Table ui32:    WriteUInt32Table(ui32);       break;
                case UnknownUInt16Table ui16:    WriteUInt16Table(ui16);       break;
                case UnknownUInt8Table ui8:      WriteUInt8Table(ui8);         break;
                case ModelSwitchGroupsTable msg: WriteModelSwitchGroups(msg);  break;
                case AnimationTable ta:          WriteAnimations(ta, settings?.ShortEmptyAnimationTable ?? false); break;
                case BoundaryTable bt:           WriteBoundaries(bt);          break;
                case TextureIDTable tid:         WriteTextureIDs(tid, settings?.LongEmptyAltAnimationTable ?? false); break;
                case MissingModelChunk mmc:      return null;
                case ModelChunk mc:              WriteHeaderModels(mc.Models, mc.ModelInstances, out pos); break;
                default:
                    throw new ArgumentException($"Unhandled type '{data.GetType().Name}' for {nameof(WriteTableOrNull)}()");
            }

            WriteToAlignTo(2);
            return pos;
        }

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

        public void WriteAnimations(AnimationTable animations, bool shortEmptyTable) {
            // TODO: Write the things
            if (shortEmptyTable)
                WriteUShort(0xFFFF);
            else {
                for (int i = 0; i < 2; i++) {
                    if (animations.Is32Bit)
                        WriteUInt(animations.TextureEndId);
                    else
                        WriteUShort((ushort) animations.TextureEndId);
                }
            }
        }

        public void WriteBoundaries(BoundaryTable boundaries) {
            foreach (var boundary in boundaries) {
                WriteShort(boundary.X1);
                WriteShort(boundary.Z1);
                WriteShort(boundary.X2);
                WriteShort(boundary.Z2);
            }
        }

        public void WriteTextureIDs(TextureIDTable textureIds, bool writeLongEmptyData) {
            foreach (var textureId in textureIds)
                WriteUShort(textureId.TextureID);

            if (writeLongEmptyData && textureIds.Length == 0) {
                WriteUShort(0xFFFF);
                WriteUShort(0xFFFF);
            }
            else
                WriteUShort(0xFFFF);
        }
    }
}
