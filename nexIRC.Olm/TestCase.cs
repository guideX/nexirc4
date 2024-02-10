namespace nexIRC.Olm {
    /// <summary>
    /// Pickle Helper
    /// </summary>
    public static class PickleHelper {
        /// <summary>
        /// Unpickle
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="end"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte Unpickle(byte pos, byte end, olm.Account value) {
            uint pickle_version;
            pos = olm.unpickle(pos, end, pickle_version);
            UNPICKLE_OK(pos);
            switch (pickle_version) {
                case ACCOUNT_PICKLE_VERSION:
                case 3:
                case 2:
                    break;
                case 1:
                    value.last_error = OlmErrorCode.OLM_BAD_LEGACY_ACCOUNT_PICKLE;
                    return null;
                default:
                    value.last_error = OlmErrorCode.OLM_UNKNOWN_PICKLE_VERSION;
                    return null;
            }
            pos = olm.unpickle(pos, end, value.identity_keys);
            UNPICKLE_OK(pos);
            pos = olm.unpickle(pos, end, value.one_time_keys);
            UNPICKLE_OK(pos);
            if (pickle_version <= 2) {
                value.num_fallback_keys = 0;
            } else if (pickle_version == 3) {
                pos = olm.unpickle(pos, end, value.current_fallback_key);
                UNPICKLE_OK(pos);
                pos = olm.unpickle(pos, end, value.prev_fallback_key);
                UNPICKLE_OK(pos);
                if (value.current_fallback_key.published) {
                    if (value.prev_fallback_key.published) {
                        value.num_fallback_keys = 2;
                    } else {
                        value.num_fallback_keys = 1;
                    }
                } else {
                    value.num_fallback_keys = 0;
                }
            } else {
                pos = olm.unpickle(pos, end, value.num_fallback_keys);
                UNPICKLE_OK(pos);
                if (value.num_fallback_keys >= 1) {
                    pos = olm.unpickle(pos, end, value.current_fallback_key);
                    UNPICKLE_OK(pos);
                    if (value.num_fallback_keys >= 2) {
                        pos = olm.unpickle(pos, end, value.prev_fallback_key);
                        UNPICKLE_OK(pos);
                        if (value.num_fallback_keys >= 3) {
                            value.last_error = OlmErrorCode.OLM_CORRUPTED_PICKLE;
                            return null;
                        }
                    }
                }
            }
            pos = olm.unpickle(pos, end, value.next_one_time_key_id);
            UNPICKLE_OK(pos);
            return pos;
        }
    }

}
