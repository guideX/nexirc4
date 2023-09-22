using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nexIRC.Olm.Core {
    public class Crypto {
        private void _olm_crypto_ed25519_generate_key(byte random_32_bytes, _olm_ed25519_key_pair key_pair) {
            Globals.ed25519_create_keypair(key_pair.public_key.public_key, key_pair.private_key.private_key, random_32_bytes);
        }
    }
}
