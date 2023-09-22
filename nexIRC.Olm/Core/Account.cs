using System.Text;
using System.Runtime.InteropServices;
namespace nexIRC.Olm.Core {
    /// <summary>
    /// Account
    /// </summary>
    public class Account {
        /// <summary>
        /// Last Error
        /// </summary>
        private ErrorsEnum _lastError;
        /// <summary>
        /// Memory Copy
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemoryCopy(IntPtr dest, IntPtr src, UIntPtr count);
        /// <summary>
        /// Key Json Ed25519
        /// </summary>
        public static byte[] KEY_JSON_ED25519 = Encoding.ASCII.GetBytes("\"ed25519\":");
        /// <summary>
        /// Curve 25519
        /// </summary>
        public static byte[] KEY_JSON_CURVE25519 = Encoding.ASCII.GetBytes("\"curve25519\":");
        /// <summary>
        /// New Account
        /// </summary>
        /// <param name="random"></param>
        /// <param name="random_length"></param>
        /// <returns></returns>
        public bool NewAccount(in byte random, uint random_length) {
            if (random_length < new_account_random_length()) {
                _lastError = ErrorsEnum.NOT_ENOUGH_RANDOM;
                return false;
            }
            _olm_crypto_ed25519_generate_key(random, identity_keys.ed25519_key);
            random += ED25519_RANDOM_LENGTH;
            _olm_crypto_curve25519_generate_key(random, identity_keys.curve25519_key);
            return true;
        }
        internal static byte write_string<T>(ref byte pos, in T value) {
            MemoryCopy(pos, value, sizeof(T) - 1);
            return pos + (sizeof(T) - 1);
        }
    }
}