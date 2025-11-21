using CommonLib.SGL;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;

namespace SF3.MPD {
    public partial class MPD_Writer {
        private uint? WriteTableOrNull<T>(T data) where T : class {
            if (data == null)
                return null;
            var pos = (uint) CurrentOffset;

            switch (data) {
                case ColorTable ct:              WriteColorTable(ct);          break;
                case LightPosition lp:           WriteLightPosition(lp);       break;
                case UnknownUInt32Table ui32:    WriteUInt32Table(ui32);       break;
                case UnknownUInt16Table ui16:    WriteUInt16Table(ui16);       break;
                case UnknownUInt8Table ui8:      WriteUInt8Table(ui8);         break;
                case ModelSwitchGroupsTable msg: WriteModelSwitchGroups(msg);  break;
                case TextureAnimationTable ta:   WriteTextureAnimations(ta);   break;
                case BoundaryTable bt:           WriteBoundaries(bt);          break;
                case TextureIDTable tid:         WriteTextureIDs(tid);         break;
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
    }
}
