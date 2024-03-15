using System.Net;
namespace team_nexgen.core.Helpers {
    /// <summary>
    /// Socket Helper
    /// </summary>
    public static class SocketHelper {
        /// <summary>
        /// Convert From Ip Address to Integer
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static uint ConvertIPToLong(string ipAddress) {
            return ConvertIPObjToLong(IPAddress.Parse(ipAddress));
        }
        /// <summary>
        /// Convert IP Obj To Long
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static uint ConvertIPObjToLong(IPAddress ip) {
            byte[] b = ip.GetAddressBytes();
            if (BitConverter.IsLittleEndian) Array.Reverse(b);
            return BitConverter.ToUInt32(b, 0);
        }
        /// <summary>
        /// Get Any IP
        /// </summary>
        /// <returns></returns>
        public static uint GetAnyIpForListening() {
            return ConvertIPObjToLong(IPAddress.Any);
        }
        /// <summary>
        /// Convert From Integer to IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string ConvertLongToIP(uint ipAddress) {
            byte[] b = BitConverter.GetBytes(ipAddress);
            if (BitConverter.IsLittleEndian) Array.Reverse(b);
            return new IPAddress(b).ToString();
        }
    }
}