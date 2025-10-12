using System;
using CommonLib.Types;

namespace CommonLib.Logging {
    public class ConsoleLogger : ILogger {
        public void StartLine(LogType logType = LogType.Info) {
            if (LogLineStarted == true)
                return;
            LogLineStarted = true;
            _currentLogType = logType;

            // Set color for warnings/errors
            switch (logType) {
                case LogType.Warning:
                    _lastColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    _lastColor = ConsoleColor.Red;
                    break;
            }

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
            switch (_currentLogType) {
                case LogType.Warning:
                    WarningCount++;
                    break;

                case LogType.Error:
                    ErrorCount++;
                    break;
            }
            _currentLogType = null;
        }

        public int Indentation { get; set; }
        public bool LogLineStarted { get; private set; } = false;
        public int WarningCount { get; private set; } = 0;
        public int ErrorCount { get; private set; } = 0;

        private ConsoleColor? _lastColor = null;
        private LogType? _currentLogType = null;
    }
}
