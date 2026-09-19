using OSCLoader.Core;

namespace UnityEngine
{
    /// <summary>
    /// Repilcates the UnityEngine.Debug class as accurately as possible
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">The message</param>
        public static void Log(object message)
        {
            if (!File.Exists(Main.CurrentVRCLogFile)) return;
            Write($"{DateTime.Now:yyyy.MM.dd HH:mm:ss} Debug      -  {message}{Environment.NewLine}");
        }
        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">The message</param>
        public static void LogWarning(object message)
        {
            if (!File.Exists(Main.CurrentVRCLogFile)) return;
            Write($"{DateTime.Now:yyyy.MM.dd HH:mm:ss} Warning      -  {message}{Environment.NewLine}");
        }
        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="message">The message</param>
        public static void LogError(object message)
        {
            if (!File.Exists(Main.CurrentVRCLogFile)) return;
            Write($"{DateTime.Now:yyyy.MM.dd HH:mm:ss} Error      -  {message}{Environment.NewLine}");
        }
        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="exception">The exception</param>
        public static void LogException(Exception exception)
        {
            LogError(exception);
        }

        internal static void Write(string text)
        {
            using var stream = new FileStream(Main.CurrentVRCLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(text);
            writer.Flush();
            stream.Flush(true);
        }
    }
}