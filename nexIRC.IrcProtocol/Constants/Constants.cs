namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants {
        /// <summary>
        /// User Statuses
        /// </summary>
        public static char[] UserStatuses {
            get {
                return new[] { '~', '&', '@', '%', '+' };
            }
        }
    }
}