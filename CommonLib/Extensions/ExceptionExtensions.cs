using System;

namespace CommonLib.Extensions {
    public static class ExceptionExtensions {
        public static string GetTypeAndMessage(this Exception e)
            => $"{e.GetType().Name}: {e.Message}";
    }
}
