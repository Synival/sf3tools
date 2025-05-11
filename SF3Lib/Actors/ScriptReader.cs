using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Types;
using SF3.Utils;

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
            CommandsRead++;
            string comment = "???";
            List<uint> data = new List<uint>() { command };

            // Get known commands
            bool isRealCommand = Enum.IsDefined(typeof(ActorCommandType), (int) command);
            if (isRealCommand) {
                var commandType = (ActorCommandType) command;
                var commandParamNames = EnumHelpers.GetAttributeOfType<ActorCommandParams>(commandType).Params;
                var commandParams = commandParamNames.Select(x => ReadInt()).ToArray();
                foreach (var p in commandParams)
                    data.Add(p);

                // Add comments for known commands
                comment = ActorScriptUtils.GetCommentForCommand(commandType, commandParams);

                // Does this command end the script?
                Done = ActorScriptUtils.CommandEndsScript(commandType, commandParams, commandPos, LabelPositions);

                // Does this command appear to be a valid one?
                if (ActorScriptUtils.CommandIsValid(commandType, CommandsRead, commandParams))
                    ValidCommands++;
            }
            // Get labels
            else if (command >= 0x80000000u && command <= 0x80100000u) {
                comment = $"(label 0x{(command & 0x0FFFFFFF):X7})";
                if (command != 0x80000000u) { // this is unlikely; it's usually (or always?) 0x8001
                    isRealCommand = true;
                    ValidCommands++;
                }
                LabelPositions[command & ~0x80000000u] = commandPos;
            }

            // Don't allow exceeding the file length.
            if (Address + Position * 4 - 1 >= Data.Length)
                Done = Aborted = true;
            // If this is the first command and it already looks bogus, just abort now.
            // (there actually are some scripts that do this, though...)
            else if (CommandsRead == 1 && ValidCommands == 0)
                Done = Aborted = true;
            // If this command was a bogus one, abort.
            else if (!isRealCommand)
                Done = Aborted = true;

            // Build the final command and return it.
            var scriptCommand = new ScriptCommand(data.ToArray(), comment);
            Text += scriptCommand.FullLine + "\r\n";

            return scriptCommand;
        }

        public void ReadUntilDoneDetected(int? maxCommands = null) {
            while (!Done && (!maxCommands.HasValue || Position < maxCommands.Value))
                _ = ReadCommand();
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
    }
}
