using System;
using SF3.Actors;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class ActorScript : Struct {
        public ActorScript(IByteData data, int id, string name, int address, int sizeInBytes, string scriptName)
        : base(data, id, name, address, sizeInBytes) {
            if (sizeInBytes % 4 != 0)
                throw new ArgumentException($"{nameof(sizeInBytes)} must be divisible by 4");

            ScriptLength = sizeInBytes / 4;
            ScriptName = scriptName;

            var scriptReader = new ScriptReader(data, address);
            // TODO: truncate commands that exceed ScriptLength
            while (scriptReader.Position < ScriptLength)
                _ = scriptReader.ReadCommand();

            Text = scriptReader.Text;
        }

        public int ScriptLength { get; }
        public string Text { get; }

        private string _scriptName = "";
        public string ScriptName {
            get => _scriptName;
            set => _scriptName = value ?? "";
        }

        public uint[] GetScriptDataCopy() {
            var dataCopy = new uint[ScriptLength];
            for (int i = 0; i < ScriptLength; i++)
                dataCopy[i] = GetScriptData(i);
            return dataCopy;
        }

        public uint GetScriptData(int position) {
            return position < 0 || position >= ScriptLength
                ? throw new ArgumentOutOfRangeException(nameof(position))
                : (uint) Data.GetDouble(Address + position * 4);
        }

        public void SetScriptData(int position, uint value) {
            if (position < 0 || position >= ScriptLength)
                throw new ArgumentOutOfRangeException(nameof(position));
            Data.SetDouble(Address + position * 4, (int) value);
        }
    }
}
