namespace nexIRC.Business.Helper {
    using System.Runtime.InteropServices;
        public static class OlmHelper {
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string olm_decrypt(string session, string message_type, string message, int message_length);
        public static string Decrypt(string session, string message_type, string message, int message_length) {
            return olm_decrypt(session, message_type, message, message_length);
        }

    }
}