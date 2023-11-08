namespace nexIRC.IrcProtocol {
    /// <summary>
    /// IRC Prefix
    /// </summary>
    public class IRCPrefix {
        /// <summary>
        /// Raw
        /// </summary>
        private readonly string _raw;
        /// <summary>
        /// From
        /// </summary>
        private readonly string _from;
        /// <summary>
        /// User
        /// </summary>
        private readonly string _user;
        /// <summary>
        /// Host
        /// </summary>
        private readonly string _host;
        /// <summary>
        /// Raw
        /// </summary>
        public string Raw { get { return _raw; } }
        /// <summary>
        /// From
        /// </summary>
        public string From { get { return _from; } }
        /// <summary>
        /// User
        /// </summary>
        public string User { get { return _user; } }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get { return _host; } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefixData"></param>
        public IRCPrefix(string prefixData) {
            _user = "";
            _raw = prefixData;
            _from = prefixData;
            if (prefixData.Contains("@")) {
                var splitedPrefix = prefixData.Split('@');
                _from = splitedPrefix[0];
                _host = splitedPrefix[1];
            } else {
                _host = "";
            }
            if (From.Contains("!")) {
                var splittedFrom = From.Split('!');
                _from = splittedFrom[0];
                _user = splittedFrom[1];
            }
        }
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Raw;
        }
    }
}