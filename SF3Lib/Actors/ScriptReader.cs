using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Actors {
    public class ScriptReader {
        public ScriptReader(IByteData data, int address) {
            Data    = data;
            Address = address;
        }

        private uint ReadInt() {
            var addr = Address + Position * 4;
            var i = (addr + 3 >= Data.Length) ? 0xFFFFFFFFu : (uint) Data.GetDouble(addr);
            Position++;
            ScriptData.Add(i);
            return i;
        }

        public ScriptCommand ReadCommand() {
            int commandPos = Position;
            var command = ReadInt();
            List<uint> data = new List<uint>() { command };

            // Get known commands
            if (Enum.IsDefined(typeof(ActorCommandType), (int) command)) {
                var commandType = (ActorCommandType) command;
                var commandParamNames = EnumHelpers.GetAttributeOfType<ActorCommandParams>(commandType).Params;
                var commandParams = commandParamNames.Select(x => ReadInt()).ToArray();
                foreach (var p in commandParams)
                    data.Add(p);
            }

            // Build a command out of this data.
            var scriptCommand = new ScriptCommand(commandPos, data.ToArray(), LabelPositions);
            Commands.Add(scriptCommand);

            // Update the state of the script based on the command.
            if (scriptCommand.Label.HasValue)
                LabelPositions[scriptCommand.Label.Value] = scriptCommand.Position;
            if (scriptCommand.IsValid)
                ValidCommands++;
            if (!scriptCommand.IsValid || Address + Position * 4 - 1 >= Data.Length)
                Aborted = true;
            if (Aborted || scriptCommand.EndsScript)
                Done = true;
            Text += scriptCommand.FullLine + "\r\n";

            // Return the command read.
            CommandsRead++;
            return scriptCommand;
        }

        public ScriptCommand[] ReadUntilDoneDetected(int? maxCommands = null) {
            var commands = new List<ScriptCommand>();
            while (!Done && (!maxCommands.HasValue || Position < maxCommands.Value))
                commands.Add(ReadCommand());
            return commands.ToArray();
        }

        public IByteData Data { get; }
        public int Address { get; }
        public int Position { get; private set; } = 0;
        public List<uint> ScriptData { get; } = new List<uint>();
        public bool Done { get; private set; } = false;
        public string Text { get; private set; } = "";
        public int CommandsRead { get; private set; } = 0;
        public int ValidCommands { get; private set; } = 0;
        public Dictionary<uint, int> LabelPositions { get; } = new Dictionary<uint, int>();
        public bool Aborted { get; private set; } = false;
        public float PercentValidCommands => (float) ValidCommands / CommandsRead;
        public List<ScriptCommand> Commands { get; } = new List<ScriptCommand>();
    }
}
