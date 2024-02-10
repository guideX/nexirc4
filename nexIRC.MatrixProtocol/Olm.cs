using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static nexIRC.MatrixProtocol.Olm;
using System.Collections.Generic;
namespace nexIRC.MatrixProtocol {
    struct test_case {
        public string msghex;
        public string expected_error;
    }
    internal class Olm {
        static void Main(string[] args) {
            
            test_case[] test_cases = new test_case[]
            {
            new test_case { msghex = "41776f", expected_error = "BAD_MESSAGE_FORMAT" },
            new test_case { msghex = "7fff6f0101346d671201", expected_error = "BAD_MESSAGE_FORMAT" },
            new test_case { msghex = "ee776f41496f674177804177778041776f6716670a677d6f670a67c2677d", expected_error = "BAD_MESSAGE_FORMAT" },
            new test_case { msghex = "e9e9c9c1e9e9c9e9c9c1e9e9c9c1", expected_error = "BAD_MESSAGE_FORMAT" }
            };

            string session_data =
                "E0p44KO2y2pzp9FIjv0rud2wIvWDi2dx367kP4Fz/9JCMrH+aG369HGymkFtk0+PINTLB9lQRt" +
                "ohea5d7G/UXQx3r5y4IWuyh1xaRnojEZQ9a5HRZSNtvmZ9NY1f1gutYa4UtcZcbvczN8b/5Bqg" +
                "e16cPUH1v62JKLlhoAJwRkH1wU6fbyOudERg5gdXA971btR+Q2V8GKbVbO5fGKL5phmEPVXyMs" +
                "rfjLdzQrgjOTxN8Pf6iuP+WFPvfnR9lDmNCFxJUVAdLIMnLuAdxf1TGcS+zzCzEE8btIZ99mHF" +
                "dGvPXeH8qLeNZA";

            byte[] DecodeHex(string input) {
                List<byte> output = new List<byte>();
                for (int i = 0; i < input.Length; i += 2) {
                    string hex = input.Substring(i, 2);
                    byte value = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                    output.Add(value);
                }
                return output.ToArray();
            }

            byte[] sessionDataBytes = DecodeHex(session_data);
        }

        static byte[] DecodeHex(string input) {
            List<byte> output = new List<byte>();
            for (int i = 0; i < input.Length; i += 2) {
                string hex = input.Substring(i, 2);
                byte value = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                output.Add(value);
            }
            return output.ToArray();
        }

        void decrypt_case(int message_type, const test_case* test_case) {
            List<byte> session_memory = new List<byte>(OlmSession.Size());
            OlmSession session = new OlmSession(session_memory.ToArray());
            List<byte> pickled = new List<byte>(session_data.Length);
            Array.Copy(session_data, pickled.ToArray(), pickled.Count);
            if (OlmError() != OlmSession.Unpickle(session, "", 0, pickled.ToArray(), pickled.Count)) {
                throw new Exception("Error unpickling session");
            }
            int message_length = test_case->msghex.Length / 2;
            byte[] message = new byte[message_length];
            decode_hex(test_case->msghex, message, message_length);
            int max_length = OlmSession.DecryptMaxPlaintextLength(session, message_type, message, message_length);
            if (test_case->expected_error) {
                if (OlmError() != max_length) {
                    throw new Exception("Error decrypting message");
                    if (test_case->expected_error != OlmSession.LastError(session)) {
                        throw new Exception("Error decrypting message");
                    }
                    free(message);
                    return;
                }
                if (OlmError() == max_length) {
                    throw new Exception("Error decrypting message");
                }
                byte[] plaintext = new byte[max_length];
                decode_hex(test_case->msghex, message, message_length);
                OlmSession.Decrypt(session, message_type, message, message_length, plaintext, max_length);
                free(message);
            }
            foreach (var test_case in test_cases) {
                CAPTURE(test_case.msghex);
                decrypt_case(0, test_case);
            }
        }
    }
}