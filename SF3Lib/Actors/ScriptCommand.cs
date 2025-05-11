using System.Linq;

namespace SF3.Actors {
    public class ScriptCommand {
        public ScriptCommand(uint[] data, string comment) {
            Data       = data;
            DataString = string.Join(", ", data.Select(x => $"0x{x:X8}")) + ",";
            Comment    = comment;

            var paramCount = data.Length - 1;
            var paramsMissing = (paramCount < 3) ? (3 - paramCount) : 0;
            FullLine = DataString + new string(' ', paramsMissing * 12) + $" // {comment}";
        }

        public uint[] Data { get; }
        public string DataString { get; }
        public string Comment { get; }
        public string FullLine { get; }
    }
}
