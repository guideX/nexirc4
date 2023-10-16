using Newtonsoft.Json;
namespace nexIRC.Business.Helper {
    /// <summary>
    /// Exception Helper
    /// </summary>
    public static class ExceptionHelper {
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="exception"></param>
        public static void HandleException(Exception exception, string? methodName, string? appPath) {
            var msg = appPath + "exceptionlog.ini";
            var n = IniFileHelper.ReadIniInt(msg, "Settings", "Count", 0) + 1;
            IniFileHelper.WriteIni(msg, "Settings", "Count", n.ToString());
            if (methodName != null) {
                IniFileHelper.WriteIni(msg, n.ToString(), "MethodName", methodName);
            }
            IniFileHelper.WriteIni(msg, n.ToString(), "Message", exception.Message);
            IniFileHelper.WriteIni(msg, n.ToString(), "StackTrace", exception.StackTrace!.ToString());
            IniFileHelper.WriteIni(msg, n.ToString(), "Timestamp", DateTime.Now.ToString());
            IniFileHelper.WriteIni(msg, n.ToString(), "Exception", JsonConvert.SerializeObject(exception));
        }
    }
}