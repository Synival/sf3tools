using System.Collections.Generic;
using CommonLib.Types;

namespace CommonLib.Logging {
    public static class Logger {
        private static List<ILogger> s_loggers = new List<ILogger>() {
            new ConsoleLogger()
        };

        /// <summary>
        /// Writes a message without a trailing newline by executing ILogger.Write() for all registered loggers.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        public static void Write(string message, LogType logType = LogType.Info)
            => s_loggers.ForEach(x => x.Write(message, logType));

        /// <summary>
        /// Writes a message with a trailing newline by executing ILogger.WriteLine() for all registered loggers.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        public static void WriteLine(string message, LogType logType = LogType.Info)
            => s_loggers.ForEach(x => x.WriteLine(message, logType));
    }
}
