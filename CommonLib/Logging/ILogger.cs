using CommonLib.Types;

namespace CommonLib.Logging {
    /// <summary>
    /// Any kind of logger to be registered with the static Logger.
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// Writes a message without a trailing newline.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        void Write(string message, LogType logType = LogType.Info);

        /// <summary>
        /// Writes a message with a trailing newline.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logType">Type of message to write.</param>
        void WriteLine(string message, LogType logType = LogType.Info);
    }
}
