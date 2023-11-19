namespace nexIRC.IrcProtocol {
    /// <summary>
    /// Parsed IRC Message
    /// </summary>
    public class ParsedIRCMessage {
        /// <summary>
        /// Raw
        /// </summary>
        public string Raw { get; }
        /// <summary>
        /// Trailing Prefix
        /// </summary>
        private readonly static char[] TrailingPrefix = { ' ', ':' };
        /// <summary>
        /// Space
        /// </summary>
        private readonly static char[] Space = { ' ' };
        /// <summary>
        /// The prefix of the message
        /// </summary>
        public IRCPrefix? Prefix { get; private set; }
        /// <summary>
        /// Command
        /// </summary>
        public string? Command { get; private set; }
        /// <summary>
        /// Parameters
        /// </summary>
        public string[]? Parameters { get; private set; }
        /// <summary>
        /// Trailing
        /// </summary>
        public string Trailing => Parameters != null ? Parameters[Parameters.Length - 1] : string.Empty;
        /// <summary>
        /// IRC Command
        /// </summary>
        public IRCCommand IRCCommand { get; private set; }
        /// <summary>
        /// Numeric Reply
        /// </summary>
        public IrcNumericReplyEnum NumericReply { get; private set; }
        /// <summary>
        /// Is Numeric
        /// </summary>
        public bool IsNumeric => NumericReply != IrcNumericReplyEnum.UNKNOWN;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rawData">Raw data to be parsed</param>
        public ParsedIRCMessage(string rawData) {
            Raw = rawData;
            Parse(rawData.AsSpan());
            ParseIrcEnums();
        }
        /// <summary>
        /// Parse Irc Enums
        /// </summary>
        private void ParseIrcEnums() {
            if (string.IsNullOrEmpty(Command)) {
                return;
            }
            if (IsNumericReply(Command)) {
                System.Enum.TryParse(Command, out IrcNumericReplyEnum numericReply);
                NumericReply = numericReply;
                if (IsNumericReply(numericReply.ToString())) NumericReply = IrcNumericReplyEnum.UNKNOWN;
            } else if (System.Enum.TryParse(Command, out IRCCommand ircCommand)) {
                IRCCommand = ircCommand;
            }
        }

        private void Parse(ReadOnlySpan<char> rawData) {
            var trailing = string.Empty;
            var indexOfNextSpace = 0;

            if (RawDataHasPrefix) {
                indexOfNextSpace = rawData.IndexOf(Space);
                var prefixData = rawData.Slice(1, indexOfNextSpace - 1);
                Prefix = new IRCPrefix(prefixData.ToString());
                rawData = rawData.Slice(indexOfNextSpace + 1);
            }

            var indexOfTrailingStart = rawData.IndexOf(TrailingPrefix);
            if (indexOfTrailingStart > -1) {
                trailing = rawData.Slice(indexOfTrailingStart + 2).Trim().ToString();
                rawData = rawData.Slice(0, indexOfTrailingStart);
            }

            if (DataDoesNotContainSpaces(rawData)) {
                Command = rawData.ToString();

                if (!string.IsNullOrEmpty(trailing)) {
                    Parameters = new[] { trailing };
                }

                return;
            }

            indexOfNextSpace = rawData.IndexOf(Space);
            Command = rawData.Slice(0, indexOfNextSpace).ToString();
            rawData = rawData.Slice(indexOfNextSpace + 1);

            var parameters = new List<string>();

            while ((indexOfNextSpace = rawData.IndexOf(Space)) > -1) {
                parameters.Add(rawData.Slice(0, indexOfNextSpace).ToString());
                rawData = rawData.Slice(indexOfNextSpace + 1);
            }

            if (!rawData.IsWhiteSpace()) {
                parameters.Add(rawData.ToString());
            }

            if (!string.IsNullOrEmpty(trailing)) {
                parameters.Add(trailing);
            }

            Parameters = parameters.ToArray();
        }

        private bool RawDataHasPrefix => Raw.StartsWith(":");

        private bool DataDoesNotContainSpaces(ReadOnlySpan<char> data) => !data.Contains(Space, StringComparison.InvariantCultureIgnoreCase);
        /// <summary>
        /// Is Numeric Reply
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IsNumericReply(string command) => command.Length == 3 && command.ToCharArray().All(char.IsDigit);
        /// <summary>
        /// Returns a string that represents the parsed IRC message
        /// </summary>
        /// <returns>String that represents the parsed IRC message</returns>
        public override string ToString() {
            var paramsDescription = Parameters != null ? "{ " + string.Join(", ", Parameters) + " }" : string.Empty;
            return $"Prefix: {Prefix}, Command: {Command}, Params: {paramsDescription}, Trailing: {Trailing}";
        }
    }
}