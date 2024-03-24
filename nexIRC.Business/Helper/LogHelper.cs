namespace nexIRC.Business.Helper {
    /// <summary>
    /// Log Helper
    /// </summary>
    public static class LogHelper {
        /// <summary>
        /// Log Activity
        /// </summary>
        /// <param name="activity"></param>
        public static void LogActivity(string activity) {
            System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "matrixirclog.txt", activity + Environment.NewLine);
        }
    }
}