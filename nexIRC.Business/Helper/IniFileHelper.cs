using System.Text;
using System.Globalization;
using nexIRC.Business.Methods;
namespace nexIRC.Business.Helper {
    /// <summary>
    /// Ini File Helper
    /// </summary>
    public class IniFileHelper {
        /// <summary>
        /// Ini File Helper
        /// </summary>
        public IniFileHelper() {
        }
        /// <summary>
        /// Read Ini
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public string? ReadIni(string file, string section, string key, string? _default = null) {
            try {
                var sb = new StringBuilder(500);
                return NativeMethods.GetPrivateProfileStringA(section, key, "", sb, sb.Capacity, file) == 0 ?
                    _default :
                    sb.ToString().Trim();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.Readini");
                return null;
            }
        }
        /// <summary>
        /// Writing of INI Files
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteIni(string file, string section, string key, string value) {
            try {
                NativeMethods.WritePrivateProfileStringA(section, key, value, file);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.WriteIni");
            }
        }
        /// <summary>
        /// Read Ini Int
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int? ReadIniInt(string file, string section, string key, int? def = null) {
            try {
                return int.TryParse(ReadIni(file, section, key, def.ToString()!), out int n) ? n : def;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.WriteIni");
                return null;
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
        public DateTime? ReadIniDateTime(string file, string section, string key, DateTime? def = null) {
            try {
                return DateTime.TryParse(ReadIni(file, section, key, def.ToString()!), out DateTime dt) ? dt : def;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.ReadIniDateTime");
                return null;
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
        public decimal? ReadIniDecimal(string file, string section, string key, decimal? def = null) {
            try {
                return decimal.TryParse(ReadIni(file, section, key, def?.ToString(CultureInfo.InvariantCulture)!), out decimal d) ? d : def;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.ReadIniDecimal");
                return null;
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
        public bool? ReadIniBool(string file, string section, string key, bool? def = null) {
            try {
                return bool.TryParse(ReadIni(file, section, key, def.ToString()), out bool b) ? b : def;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.ReadIniBool");
                return null;
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
        public double? ReadIniDouble(string file, string section, string key, double? def = 0.0) {
            try {
                return double.TryParse(ReadIni(file, section, key, def?.ToString(CultureInfo.InvariantCulture)), out double res) ? res : null;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.ReadIniFloat");
                return null;
            }
        }
        /// <summary>
        /// Read From Ini Float
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public float? ReadIniFloat(string file, string section, string key, float? def = null) {
            try {
                return float.TryParse(ReadIni(file, section, key, def?.ToString(CultureInfo.InvariantCulture)), out float res) ? res : null;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.Business.Helper.IniFileHelper.ReadIniFloat");
                return null;
            }
        }
    }
}