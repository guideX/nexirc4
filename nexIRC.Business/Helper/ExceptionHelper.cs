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
            if (appPath != null) {
                var iniFileHelper = new IniFileHelper(appPath);
                var msg = appPath + "exceptionlog.ini";
                var n = iniFileHelper.ReadIniInt(msg, "Settings", "Count", 0) + 1;
                if (n != null) {
                    iniFileHelper.WriteIni(msg, "Settings", "Count", n.Value.ToString());
                    if (methodName != null) iniFileHelper.WriteIni(msg, n.Value.ToString(), "MethodName", methodName);
                    iniFileHelper.WriteIni(msg, n.Value.ToString(), "Message", exception.Message);
                    if (exception.StackTrace != null) iniFileHelper.WriteIni(msg, n.Value.ToString(), "StackTrace", exception.StackTrace);
                    iniFileHelper.WriteIni(msg, n.Value.ToString(), "Timestamp", DateTime.Now.ToString());
                    iniFileHelper.WriteIni(msg, n.Value.ToString(), "Exception", JsonConvert.SerializeObject(exception));
                }
            }
        }
    }
}