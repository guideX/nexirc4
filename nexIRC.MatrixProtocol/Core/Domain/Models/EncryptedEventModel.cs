using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace nexIRC.MatrixProtocol.Core.Domain.Models {
    /// <summary>
    /// Encrypted Event Model
    /// </summary>
    public class EncryptedEventModel {
        /// <summary>
        /// Algorythm
        /// </summary>
        public string algorithm { get; set; }
        /// <summary>
        /// Cipher Text
        /// </summary>
        public string ciphertext { get; set; }
        /// <summary>
        /// Device ID
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// Sender Key
        /// </summary>
        public string sender_key { get; set; }
        /// <summary>
        /// Session ID
        /// </summary>
        public string session_id { get; set; }
    }
}