using nexIRC.Business.Methods;
using System.Globalization;
using System.Text;
namespace nexIRC.Business.Helper {
    /// <summary>
    /// Ini File Helper
    /// </summary>
    public static class IniFileHelper {
        /// <summary>
        /// Read Ini
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static string ReadIni(string file, string section, string key, string _default = "") {
            var msg = new StringBuilder(500);
            if (NativeMethods.GetPrivateProfileStringA(section, key, "", msg, msg.Capacity, file) == 0) {
                return _default;
            } else {
                return msg.ToString().Trim();
            }
        }
        /// <summary>
        /// Writing of INI Files
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteIni(string file, string section, string key, string value) {
            NativeMethods.WritePrivateProfileStringA(section, key, value, file);
        }
        /// <summary>
        /// Read Ini Int
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int ReadIniInt(string file, string section, string key, int def = 0) {
            int n;
            if (int.TryParse(ReadIni(file, section, key, def.ToString()), out n)) {
                return n;
            } else {
                return def;
            }
        }
        /// <summary>
        /// Read Ini Date Time
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DateTime? ReadIniDateTime(string file, string section, string key, DateTime? def = null) {
            DateTime dt;
            if (DateTime.TryParse(ReadIni(file, section, key, def.ToString()), out dt)) {
                return dt;
            } else {
                return def;
            }
        }
        /// <summary>
        /// Read INI Decimal
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal ReadIniDecimal(string file, string section, string key, decimal def = decimal.Zero) {
            decimal d;
            if (decimal.TryParse(ReadIni(file, section, key, def.ToString(CultureInfo.InvariantCulture)), out d)) {
                return d;
            } else {
                return def;
            }
        }
        /// <summary>
        /// Read INI Bool
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool ReadIniBool(string file, string section, string key, bool def = false) {
            bool b;
            if (bool.TryParse(ReadIni(file, section, key, def.ToString()), out b)) {
                return b;
            } else {
                return def;
            }
        }
        /// <summary>
        /// Read Ini Double
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static double ReadIniDouble(string file, string section, string key, double def = 0.0) {
            var msg = ReadIni(file, section, key, def.ToString(CultureInfo.InvariantCulture));
            double result;
            double.TryParse(msg, out result);
            return result;
        }
        /// <summary>
        /// Read From Ini Float
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static float ReadIniFloat(string file, string section, string key, float def = 0.0f) {
            var msg = ReadIni(file, section, key, def.ToString(CultureInfo.InvariantCulture));
            float result;
            float.TryParse(msg, out result);
            return result;
        }
    }
}