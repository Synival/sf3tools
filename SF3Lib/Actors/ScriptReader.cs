using System.Collections.Generic;
using SF3.ByteData;

namespace SF3.Actors {
    public class ScriptReader {
        public ScriptReader(IByteData data, int address) {
            Data     =   data;
            Address    = address;
            Position   = 0;
            ScriptData = new List<uint>();
        }

        public uint ReadInt() {
            var addr = Address + Position * 4;
            var i = (addr + 3 >= Data.Length) ? 0xFFFFFFFFu : (uint) Data.GetDouble(addr);
            Position++;
            ScriptData.Add(i);
            return i;
        }

        public IByteData Data { get; }
        public int Address { get; }
        public int Position { get; private set; }
        public List<uint> ScriptData { get; }
    }
}
