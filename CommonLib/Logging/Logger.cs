using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Types;

namespace CommonLib.Logging {
    public static class Logger {
        public static ILogger DefaultLogger = new ConsoleLogger();

        private static List<ILogger> s_loggers = new List<ILogger>() {
            DefaultLogger
        };

        /// <summary>
        /// Finishes any log lines in progress.
        /// </summary>
        public static void FinishLine()
            => s_loggers.ForEach(x => x.FinishLine());

        /// <summary>
        /// Writes text to registered loggers without ending any lines unless newlines characters ('\n') are provided.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        public static void Write(string message, LogType logType = LogType.Info) {
            while (message != null && message != "") {
                var newLineIndex = message.IndexOf('\n');
                var currentMsg = (newLineIndex == -1) ? message : message.Substring(0, newLineIndex);
                message = (newLineIndex == -1) ? null : message.Substring(newLineIndex + 1);

                foreach (var logger in s_loggers) {
                    if (!logger.LogLineStarted)
                        logger.StartLine(logType);
                    if (currentMsg != "")
                        logger.WriteOnLine(currentMsg);
                    if (newLineIndex != -1)
                        logger.FinishLine();
                }
            }
        }

        /// <summary>
        /// Writes one or more new log lines to all registered loggers. If there is a log line in progress,
        /// it is automatically finished.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        public static void WriteLine(string message, LogType logType = LogType.Info) {
            FinishLine();
            Write(message + "\n", logType);
        }

        /// <summary>
        /// Logs an exception on its own line with predefined formatting.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(Exception exception)
            => WriteLine(exception.GetTypeAndMessage(), LogType.Error);

        /// <summary>
        /// Indents all registered loggers by the number of tabs provided.
        /// </summary>
        /// <param name="tabs">Number of tabs to indent.</param>
        public static void Indent(int tabs = 1)
            => s_loggers.ForEach(x => x.Indentation += tabs);

        /// <summary>
        /// Unindents (unapplies indentation of) all registered loggers by the number of tabs provided.
        /// </summary>
        /// <param name="tabs">Number of tabs to unindent.</param>
        public static void Unindent(int tabs = 1)
            => s_loggers.ForEach(x => x.Indentation -= tabs);

        /// <summary>
        /// Creates a scoped indentation that will indent upon creation and unindent when disposed.
        /// </summary>
        /// <param name="tabs">Number of tabs to indent and unindent.</param>
        /// <returns></returns>
        public static ScopeGuard IndentedSection(int tabs = 1)
            => new ScopeGuard(() => Indent(tabs), () => Unindent(tabs));

        /// <summary>
        /// Returns the number of warnings reported during the entire lifespan of the Logger.
        /// </summary>
        public static int TotalWarningCount => DefaultLogger.WarningCount;

        /// <summary>
        /// Returns the number of errors reported during the entire lifespan of the Logger.
        /// </summary>
        public static int TotalErrorCount => DefaultLogger.ErrorCount;
    }
}
