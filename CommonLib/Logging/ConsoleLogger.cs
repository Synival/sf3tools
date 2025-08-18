using System;
using CommonLib.Types;

namespace CommonLib.Logging {
    public class ConsoleLogger : ILogger {
        public void Write(string message, LogType logType = LogType.Info) {
            ConsoleColor? color = null;

            // Set color for warnings/errors
            if (logType == LogType.Warning)
                color = ConsoleColor.Yellow;
            else if (logType == LogType.Error)
                color = ConsoleColor.Red;

            bool addAnotherNewline = false;
            if (color.HasValue) {
                Console.ForegroundColor = color.Value;
                if (message.EndsWith("\n")) {
                    addAnotherNewline = true;
                    message = message.Substring(0, message.Length - 1);
                }
            }
            Console.Write(message);

            // Reset color if set earlier
            if (color.HasValue)
                Console.ResetColor();
            if (addAnotherNewline == true)
                Console.Write('\n');
        }

        public void WriteLine(string message, LogType logType = LogType.Info) {
            Write(message + "\n", logType);
        }
    }
}
