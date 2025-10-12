using CommonLib.Types;

namespace CommonLib.Logging {
    /// <summary>
    /// Any kind of logger to be registered with the static Logger.
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// Begins a new log line with a specific log type. Indentation and other line formatting is applied here.
        /// Does nothing if the line has already been started.
        /// </summary>
        /// <param name="logType">Type of message to write.</param>
        void StartLine(LogType logType = LogType.Info);

        /// <summary>
        /// Appends to the current log line. Does nothing if a line has not been started.
        /// </summary>
        /// <param name="message">Message to write.</param>
        void WriteOnLine(string message);

        /// <summary>
        /// Finishes the current log line. Does nothing if a line has not been started.
        /// </summary>
        void FinishLine();

        /// <summary>
        /// The amount of line indentation, in tabs.
        /// </summary>
        int Indentation { get; set; }

        /// <summary>
        /// When true, a log line has been started with StartLine().
        /// </summary>
        bool LogLineStarted { get; }

        /// <summary>
        /// The number of warning lines logged.
        /// </summary>
        int WarningCount { get; }

        /// <summary>
        /// The number of error lines logged.
        /// </summary>
        int ErrorCount { get; }
    }
}
