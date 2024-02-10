using System.Runtime.InteropServices;
using System.Text;
namespace nexIRC.Business.Methods {
    /// <summary>
    /// Native Methods
    /// </summary>
    public static class NativeMethods {
        /// <summary>
        /// Read Ini Files
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="Default"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// Get Private Profile String A
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="Default"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetPrivateProfileStringA(string section, string key, string Default, StringBuilder retVal, int size, string filePath) {
            return GetPrivateProfileString(section, key, Default, retVal, size, filePath);
        }
        /// <summary>
        /// Write Ini Files
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpString"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        /// <summary>
        /// Write Private Profile String
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpString"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        public static bool WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName) {
            return WritePrivateProfileString(lpAppName, lpKeyName, lpString, lpFileName);
        }
    }
}