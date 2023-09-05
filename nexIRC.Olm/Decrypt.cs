namespace nexIRC.Olm {
    [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern string olm_decrypt(IntPtr session, IntPtr message_type, IntPtr message, IntPtr message_length);
    public static class Decrypt {
        public static string Decrypt(string session, string message_type, string message, int message_length) {
            return olm_decrypt(session, message_type, message, message_length);
        }
    }
}