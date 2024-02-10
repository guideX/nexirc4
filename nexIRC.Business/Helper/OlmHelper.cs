namespace nexIRC.Business.Helper {
    using System.Runtime.InteropServices;
    /// <summary>
    /// Olm Helper
    /// </summary>
    public static class OlmHelper {
        /// <summary>
        /// Olm Decrypt
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message_type"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string olm_decrypt(string session, string message_type, string message, int message_length);
        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message_type"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <returns></returns>
        public static string Decrypt(string session, string message_type, string message, int message_length) {
            return olm_decrypt(session, message_type, message, message_length);
        }
    }
}