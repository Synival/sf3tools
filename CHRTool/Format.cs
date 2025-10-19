using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CHRTool {
    public static class Format {
        public static int Run(string[] args) {
            // Fetch the directory with the game data.
            var file = args.Length >= 1 ? args[0] : null;
            if (file == null) {
                Logger.WriteLine("No file path provided", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'depends' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            if (Path.GetExtension(file).ToLower() != ".sf3sprite") {
                Logger.WriteLine($"File '{file}' unsupported. Only SF3Sprite files can be formatted at this time.", LogType.Error);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            var text = File.ReadAllText(file);
            FormatSF3Sprite(text);
            return 0;
        }

        private static void FormatSF3Sprite(string text) {
            var formatter = new SF3.Sprites.Formatter();
            Console.Write(formatter.Format(text));
        }
    }
}