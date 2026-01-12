using System;
using SF3.Models.Files.MPD;
using SF3.Models.Tables;

namespace SF3.MPD {
    public partial class MPD_Writer {
        private uint? WriteTableOrNull<T>(T data, IMPD_Settings settings = null) where T : class {
            if (data == null)
                return null;
            var pos = (uint) CurrentOffset;

            switch (data) {
                case UnknownUInt32Table ui32:    WriteUInt32Table(ui32);       break;
                case UnknownUInt16Table ui16:    WriteUInt16Table(ui16);       break;
                case UnknownUInt8Table ui8:      WriteUInt8Table(ui8);         break;
                case MissingModelChunk mmc:      return null;
                case ModelChunk mc:              WriteHeaderModels(mc.Models, mc.ModelInstances, out pos); break;
                default:
                    throw new ArgumentException($"Unhandled type '{data.GetType().Name}' for {nameof(WriteTableOrNull)}()");
            }

            WriteToAlignTo(2);
            return pos;
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
    }
}
