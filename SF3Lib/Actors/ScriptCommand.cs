using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Utils;
using SF3.Types;
using SF3.Utils;

namespace SF3.Actors {
    public class ScriptCommand {
        public ScriptCommand(int position, uint[] data, Dictionary<uint, int> labelPositions) {
            Position = position;
            Data     = data;

            // Read commands
            var command = data[0];
            if (Enum.IsDefined(typeof(ActorCommandType), (int) command)) {
                var commandType = (ActorCommandType) command;
                var commandParams = data.Skip(1).ToArray();
                var expectedParams = EnumHelpers.GetAttributeOfType<ActorCommandParams>(commandType).Params;

                if (commandParams.Length == expectedParams.Length) {
                    Comment    = ActorScriptUtils.GetCommentForCommand(commandType, commandParams);
                    EndsScript = ActorScriptUtils.CommandEndsScript(commandType, commandParams, position, labelPositions);
                    IsValid    = ActorScriptUtils.CommandIsValid(commandType, position == 0, commandParams);
                }
                else {
                    var paramString = string.Join(", ", commandParams);
                    Comment = $"Wrong parameter count; expected: {commandType}({paramString})";
                }
            }
            // Read labels (TODO: is 0x80000000 a valid label?)
            else if (command >= 0x80000000u && command <= 0x80100000u) {
                Comment = $"(label 0x{(command & 0x0FFFFFFF):X7})";
                IsValid = (command != 0x80000000u); // this is unlikely; it's usually (or always?) 0x8001
                Label   = command & ~0x80000000u;
            }
            // No clue what this could be
            else
                Comment = "????";

            var paramCount = data.Length - 1;
            var paramsMissing = (paramCount < 3) ? (3 - paramCount) : 0;
            DataString = string.Join(", ", data.Select(x => $"0x{x:X8}")) + ",";
            FullLine = DataString + new string(' ', paramsMissing * 12) + $" // {Comment}";
        }

        public int Position { get; }
        public uint[] Data { get; }
        public string DataString { get; }
        public string Comment { get; }
        public string FullLine { get; }
        public bool EndsScript { get; } = false;
        public bool IsValid { get; } = false;
        public uint? Label { get; } = null;
    }
}
