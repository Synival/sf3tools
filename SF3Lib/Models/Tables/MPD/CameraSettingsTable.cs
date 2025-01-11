using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class CameraSettingsTable : Table<CameraSettings> {
        protected CameraSettingsTable(IByteData data, int address) : base(data, address) { }

        public static CameraSettingsTable Create(IByteData data, int address) {
            var newTable = new CameraSettingsTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new CameraSettings(Data, id, "CameraSettings", address));

        public override int? MaxSize => 1;
    }
}
