using nexIRC.Olm.Core;
using System.Runtime.InteropServices;
using System.Text;
namespace nexIRC.Olm {
    /// <summary>
    /// Group Api
    /// </summary>
    public static class Api {
        /// <summary>
        /// Olm Group Decrypt Max Plaintext Length
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_group_decrypt_max_plaintext_length(
            IntPtr session,
            byte[] message,
            uint message_length
        );
        /// <summary>
        /// Olm Init Inbound Group Session
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_init_inbound_group_session(
            IntPtr session, 
            string session_key, 
            uint session_key_length
        );
        /// <summary>
        /// Olm Inbound Group Session Is Verified
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_inbound_group_session_is_verified(
            IntPtr session
        );
        /// <summary>
        /// Olm Unpickle Outbound Group Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="key_length"></param>
        /// <param name="pickled"></param>
        /// <param name="pickled_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_unpickle_outbound_group_session(
            IntPtr session,
            byte[] key,
            uint key_length,
            byte[] pickled,
            uint pickled_length
        );
        /// <summary>
        /// Olm Pickle Outbound Group Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="key_length"></param>
        /// <param name="pickled"></param>
        /// <param name="pickled_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_pickle_outbound_group_session(
            IntPtr session,
            byte[] key,
            uint key_length,
            byte[] pickled,
            uint pickled_length
        );
        /// <summary>
        /// Olm Pickle Output Group Session Length
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_pickle_outbound_group_session_length(
            IntPtr session
        );
        /// <summary>
        /// Olm Outbound Group Session
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr olm_outbound_group_session(
            byte[] memory
        );
        /// <summary>
        /// Olm Outbound Group Session State
        /// </summary>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_outbound_group_session_size(
        );
        /// <summary>
        /// Olm Group Decrypt
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <param name="plaintext"></param>
        /// <param name="max_plaintext_length"></param>
        /// <param name="message_index"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_group_decrypt(
            IntPtr session,
            byte[] message,
            uint message_length,
            byte[] plaintext,
            uint max_plaintext_length,
            uint message_index
        );
        /// <summary>
        /// Olm Inbound Group Session Size
        /// </summary>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_inbound_group_session_size(

        );
        /// <summary>
        /// Olm Inbound Group Session
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr olm_inbound_group_session(
            byte[] memory
        );
        /// <summary>
        /// Olm Decrypt
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message_type"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <param name="plaintext"></param>
        /// <param name="max_plaintext_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_decrypt(
            IntPtr session,
            uint message_type,
            byte[] message,
            uint message_length,
            byte[] plaintext,
            uint max_plaintext_length
        );
        /// <summary>
        /// Get Last Error
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr olm_session_last_error(
            IntPtr session
        );
        /// <summary>
        /// Olm Unpickle Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="key_length"></param>
        /// <param name="pickled"></param>
        /// <param name="pickled_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_unpickle_session(
            IntPtr session,
            byte[] key,
            uint key_length,
            byte[] pickled,
            uint pickled_length
        );
        /// <summary>
        /// Olm Session
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr olm_session(
            byte[] memory
        );
        /// <summary>
        /// Olm Decrypt Max Plaintext Length
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message_type"></param>
        /// <param name="message"></param>
        /// <param name="message_length"></param>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint olm_decrypt_max_plaintext_length(
            IntPtr session,
            uint message_type,
            string message,
            uint message_length
        );
        /// <summary>
        /// Session Size
        /// </summary>
        /// <returns></returns>
        [DllImport("Olm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr olm_session_size(

        );
    }
    /// <summary>
    /// Encryption Decryption Helper
    /// </summary>
    public static class OlmHelper {
        /*
        /// <summary>
        /// Get Session
        /// </summary>
        /// <returns></returns>
        private static IntPtr GetSession(uint sessionSize) {
            return Api.olm_outbound_group_session(
                new List<byte>(new byte[sessionSize])
            );
        }
        /// <summary>
        /// Get Pickle
        /// </summary>
        /// <param name="key"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private static PickleResultModel GetPickle(int keyLength, byte[] keyBytes, IntPtr session) {
            var result = new PickleResultModel();
            result.Length = Api.olm_pickle_outbound_group_session_length(session);
            result.Bytes = new byte[result.PickleLength];
            result.Result = Api.olm_pickle_outbound_group_session(
                session,
                keyBytes,
                keyLength,
                pickleBytes,
                result.Length
            );
            return result;
        }
        /// <summary>
        /// Get Unpickle
        /// </summary>
        /// <returns></returns>
        private static UnpickleResultModel GetUnpickle() { 
        }
        /// <summary>
        /// Get Session Size
        /// </summary>
        /// <returns></returns>
        private static uint GetSessionSize() { 
            return Api.olm_outbound_group_session_size();
        }
        */
        /// <summary>
        /// Olm Group Decrypt
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OlmByteArrayResultModel GroupDecrypt(string sessionKey, string message) {
            var result = new OlmByteArrayResultModel();
            try {
                if (!string.IsNullOrWhiteSpace(message)) {
                    var msglen = message.Length;
                    var bs = sizeof(byte) - 1;
                    var size = Api.olm_inbound_group_session_size();
                    if (size != 0) {
                        byte[] sessionMemory1 = new byte[size];
                        var session1 = Api.olm_inbound_group_session(
                            sessionMemory1
                        );
                        var groupSessionVerified = Api.olm_inbound_group_session_is_verified(
                            session1
                        );
                        var msgcopy = new byte[bs];
                        var initInboundGroupSession = Api.olm_init_inbound_group_session(
                            session1,
                            sessionKey,
                            (uint)bs
                        );
                        var size2 = Api.olm_group_decrypt_max_plaintext_length(
                            session1,
                            msgcopy,
                            (uint)bs
                        );
                        if (size2 != 0) {
                            var plainTextBuffer = new byte[byte.MaxValue];
                            uint message_index = 0;
                            var res = Api.olm_group_decrypt(
                                session1,
                                msgcopy,
                                (uint)msglen,
                                plainTextBuffer,
                                size2,
                                message_index
                            );
                            result.Success = true;
                        }
                    }
                }
            } catch (Exception ex) {
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static OlmByteArrayResultModel Decrypt(string key, string message, uint messageType = 1) {
            var result = new OlmByteArrayResultModel();
            try {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(message)) {
                    var session = Api.olm_session(new byte[Api.olm_session_size()]);
                    var pickled = Encoding.ASCII.GetBytes(message);
                    var messageLength = (uint)message.Length;
                    var messageBytes = Encoding.ASCII.GetBytes(message);
                    uint maxLength = Api.olm_decrypt_max_plaintext_length(
                        session,
                        messageType,
                        message,
                        messageLength
                    );
                    var plainText = new List<byte>(new byte[messageLength]).ToArray();
                    var decryptResult = Api.olm_decrypt(
                        session,
                        messageType,
                        messageBytes,
                        messageLength,
                        plainText,
                        maxLength
                    );
                    if (decryptResult != uint.MaxValue && plainText != null) {
                        result.Bytes = plainText;
                        result.Success = true;
                    } else {
                        result.ErrorStr = Marshal.PtrToStringAnsi(Api.olm_session_last_error(session));
                        switch (result.ErrorStr) {
                            case "BAD_MESSAGE_FORMAT":
                                result.Error = ErrorsEnum.BAD_MESSAGE_FORMAT;
                                result.LongErrorStr = "Message could not be decoded";
                                break;
                            case "OUTPUT_BUFFER_TOO_SMALL":
                                result.Error = ErrorsEnum.OUTPUT_BUFFER_TOO_SMALL;
                                result.LongErrorStr = "The plain-text buffer is smaller than *olm_decrypt_max_plaintext_length()";
                                break;
                            case "INVALID_BASE64":
                                result.Error = ErrorsEnum.INVALID_BASE64;
                                result.LongErrorStr = "Base64 could not be decoded";
                                break;
                            case "BAD_MESSAGE_VERSION":
                                result.Error = ErrorsEnum.BAD_MESSAGE_VERSION;
                                result.LongErrorStr = "Message is for an unsupported version of the protocol";
                                break;
                            case "BAD_MESSAGE_MAC":
                                result.Error = ErrorsEnum.BAD_MESSAGE_MAC;
                                result.LongErrorStr = "The MAC on the message was invalid";
                                break;
                            default:
                                result.Error = ErrorsEnum.UNKNOWN;
                                break;
                        }
                    }
                }
            } catch (Exception ex) {
                //result.Message = ex.Message;
            }
            return result;
        }
    }
}