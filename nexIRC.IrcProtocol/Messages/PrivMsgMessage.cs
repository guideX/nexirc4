using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Ctcp;
using System.Text;
namespace nexIRC.IrcProtocol.Messages {
    /// <summary>
    /// Priv Msg Message
    /// </summary>
    public class PrivMsgMessage : IRCMessage, IServerMessage, IClientMessage, ISplitClientMessage {
        /// <summary>
        /// Max Message Byte Size
        /// </summary>
        public const int MaxMessageByteSize = 400;
        /// <summary>
        /// From
        /// </summary>
        public string From { get; }
        /// <summary>
        /// Prefix
        /// </summary>
        public IRCPrefix Prefix { get; }
        /// <summary>
        /// To
        /// </summary>
        public string To { get; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Is Channel Message
        /// </summary>
        public bool IsChannelMessage { get; }
        /// <summary>
        /// Is Ctcp
        /// </summary>
        public bool IsCtcp { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parsedMessage"></param>
        public PrivMsgMessage(ParsedIRCMessage parsedMessage) {
            From = parsedMessage.Prefix!.From;
            Prefix = parsedMessage.Prefix;
            To = parsedMessage.Parameters![0];
            Message = parsedMessage.Trailing;
            IsChannelMessage = To[0] == '#';
            IsCtcp = Message.Contains(CtcpCommands.CtcpDelimiter);
        }
        /// <summary>
        /// Priv Msg Message
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        public PrivMsgMessage(string target, string text) {
            To = target;
            Message = !text.Contains(" ") ? $":{text}" : text;
            From = "";
            Prefix = new IRCPrefix("");
        }
        /// <summary>
        /// Tokens
        /// </summary>
        public IEnumerable<string> Tokens => Enumerable.Empty<string>();
        /// <summary>
        /// Line Split Tokens
        /// </summary>
        public IEnumerable<string[]> LineSplitTokens => BuildTokensFromMessageChunks();
        /// <summary>
        /// Build Tokens From message Chunks
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string[]> BuildTokensFromMessageChunks() {
            using var reader = new StringReader(Message);
            string line;
            while ((line = reader.ReadLine()!) != null) {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var utf8Text = Encoding.UTF8.GetBytes(Message);
                var index = 0;
                var size = 0;
                var chunkStart = 0;
                while (index < utf8Text.Length) {
                    if (size >= MaxMessageByteSize) {
                        var messageChunk = Encoding.UTF8.GetString(utf8Text.Skip(chunkStart).Take(size).ToArray());
                        yield return GetTokens(messageChunk);
                        chunkStart = index;
                        size = 0;
                    }
                    int length = GetUtf8CharLength(utf8Text[index]);
                    index += length;
                    size += length;
                    if (index == utf8Text.Length) {
                        var messageChunk = Encoding.UTF8.GetString(utf8Text.Skip(chunkStart).ToArray());
                        yield return GetTokens(messageChunk);
                    }
                }
            }
            int GetUtf8CharLength(byte b) {
                if (b < 0x80) return 1;
                else if ((b & 0xE0) == 0xC0) return 2;
                else if ((b & 0xF0) == 0xE0) return 3;
                else if ((b & 0xF8) == 0xF0) return 4;
                else if ((b & 0xfc) == 0xf8) return 5;
                else return 6;
            }
        }
        /// <summary>
        /// Get Tokens
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string[] GetTokens(string message) => new[] { "PRIVMSG", To, message };
    }
}