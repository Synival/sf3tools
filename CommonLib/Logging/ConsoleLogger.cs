using System;
using CommonLib.Types;

namespace CommonLib.Logging {
    public class ConsoleLogger : ILogger {
        public void StartLine(LogType logType = LogType.Info) {
            if (LogLineStarted == true)
                return;
            LogLineStarted = true;

            // Set color for warnings/errors
            if (logType == LogType.Warning)
                _lastColor = ConsoleColor.Yellow;
            else if (logType == LogType.Error)
                _lastColor = ConsoleColor.Red;
            if (_lastColor.HasValue)
                Console.ForegroundColor = _lastColor.Value;

            if (Indentation > 0)
                Console.Write(new string(' ', Indentation * 2));
        }

        public void WriteOnLine(string message) {
            if (LogLineStarted == false || message == null || message == "")
                return;
            Console.Write(message);
        }

        public void FinishLine() {
            if (LogLineStarted == false)
                return;

            if (_lastColor.HasValue) {
                Console.ResetColor();
                _lastColor = null;
            }
            Console.WriteLine();
            LogLineStarted = false;
        }

        public int Indentation { get; set; }
        public bool LogLineStarted { get; private set; } = false;

        private ConsoleColor? _lastColor = null;
    }
}
