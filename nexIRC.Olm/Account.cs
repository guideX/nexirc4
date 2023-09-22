namespace nexIRC.Olm {
    /// <summary>
    /// Account 
    /// </summary>
    public class Account {
        public static byte[] KEY_JSON_ED25519 = "\"ed25519\":";
        public static byte[] KEY_JSON_CURVE25519 = "\"curve25519\":";
        //private olm.Account.Account(): num_fallback_keys(0), next_one_time_key_id(0), last_error(OlmErrorCode.OLM_SUCCESS) {
        //}
        /// <summary>
        /// Lookup Key
        /// </summary>
        /// <param name="public_key"></param>
        /// <returns></returns>
        public int lookup_key(in _olm_curve25519_public_key public_key) {
            foreach (olm.OneTimeKey key in one_time_keys) {
                if (olm.array_equal(key.key.public_key.public_key, public_key.public_key)) {
                    return &key;
                }
            }
            if (num_fallback_keys >= 1 && olm.array_equal(current_fallback_key.key.public_key.public_key, public_key.public_key)) {
                return &current_fallback_key;
            }
            if (num_fallback_keys >= 2 && olm.array_equal(prev_fallback_key.key.public_key.public_key, public_key.public_key)) {
                return &prev_fallback_key;
            }
            return 0;
        }
        /// <summary>
        /// Remove Key
        /// </summary>
        /// <param name="public_key"></param>
        /// <returns></returns>
        public uint remove_key(in _olm_curve25519_public_key public_key) {
            OneTimeKey* i = new OneTimeKey();
            for (i = one_time_keys.begin(); i != one_time_keys.end(); ++i) {
                if (olm.array_equal(i.key.public_key.public_key, public_key.public_key)) {
                    uint id = i.id;
                    one_time_keys.erase(i);
                    return id;
                }
            }
            if (num_fallback_keys >= 1 && olm.array_equal(current_fallback_key.key.public_key.public_key, public_key.public_key)) {
                return current_fallback_key.id;
            }
            if (num_fallback_keys >= 2 && olm.array_equal(prev_fallback_key.key.public_key.public_key, public_key.public_key)) {
                return prev_fallback_key.id;
            }
            return (uint)-1;
        }
        public uint new_account_random_length() {
            return ED25519_RANDOM_LENGTH + CURVE25519_RANDOM_LENGTH;
        }

    }
}
