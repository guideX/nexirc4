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
        /// Raw
        /// </summary>
        public string Raw {
            get {
                return _raw;
            }
        }
        /// <summary>
        /// From
        /// </summary>
        public string From { get; }
        /// <summary>
        /// User
        /// </summary>
        public string User { get; }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefixData"></param>
        public IRCPrefix(string prefixData) {
            _raw = prefixData;
            From = prefixData;

            if (prefixData.Contains("@")) {
                var splitedPrefix = prefixData.Split('@');
                From = splitedPrefix[0];
                Host = splitedPrefix[1];
            } else {
                Host = "";
            }

            if (From.Contains("!")) {
                var splittedFrom = From.Split('!');
                From = splittedFrom[0];
                User = splittedFrom[1];
            }
        }

        public override string ToString() {
            return Raw;
        }
    }
}