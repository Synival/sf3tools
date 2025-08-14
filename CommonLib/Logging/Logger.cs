using System;
using CommonLib.Types;

namespace CommonLib.Logging {
    public static class Logger {
        public static void Write(string message, LogType logType = LogType.Info) {
            // TODO: write to a context
            Console.Write(message);
        }

        public static void WriteLine(string message, LogType logType = LogType.Info) {
            // TODO: write to a context
            Console.WriteLine(message);
        }
    }
}
