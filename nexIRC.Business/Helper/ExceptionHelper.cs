using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace nexIRC.Business.Helper {
    /// <summary>
    /// Exception Helper
    /// </summary>
    public static class ExceptionHelper {
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="exception"></param>
        public static void HandleException(Exception exception, string? methodName) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                using (var eventLog = new EventLog("Application")) {
                    eventLog.Source = "Application";
                    var sb = new StringBuilder();
                    sb.AppendLine("Message: " + System.Environment.NewLine + exception.Message + System.Environment.NewLine + System.Environment.NewLine);
                    sb.AppendLine("Stack Trace: " + System.Environment.NewLine + exception.StackTrace + System.Environment.NewLine + System.Environment.NewLine);
                    if (exception.InnerException != null) sb.AppendLine("Inner Exception: " + System.Environment.NewLine + exception.InnerException!.ToString() + System.Environment.NewLine + System.Environment.NewLine);
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error , 2278, 1);
                }
        }
    }
}