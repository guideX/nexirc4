namespace nexIRC.MatrixProtocol.Wrapper {
    /// <summary>
    /// Message Helper Model
    /// </summary>
    public class MessageHelperModel {
        /// <summary>
        /// Reply To Nickname
        /// </summary>
        public string? ReplyToNickname { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Raw Message
        /// </summary>
        public string? RawMessage { get; set; }
        /// <summary>
        /// Sender UserID
        /// </summary>
        public string? SenderUserID { get; set; }
        /// <summary>
        /// Matrix Channel
        /// </summary>
        public string? MatrixChannel { get; set; }
        /// <summary>
        /// IRC Channel
        /// </summary>
        public string? IrcChannel { get; set; }
        /// <summary>
        /// Double Relay Detected
        /// </summary>
        public bool DoubleRelayDetected { get; set; }
        /// <summary>
        /// Is Mention
        /// </summary>
        public bool IsMention { get; set; }
        /// <summary>
        /// Mention To
        /// </summary>
        public string? MentioningTo { get; set; }
        /// <summary>
        /// Send Message
        /// </summary>
        public bool SendMessage { get; set; }
    }
    /// <summary>
    /// Message Helper
    /// </summary>
    public static class MessageHelper {
        /// <summary>
        /// Get Message Details
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static MessageHelperModel GetMessageDetails(string matrixRoomID, string defaultMatrixRoomID, string ircChannel, string message, string senderUserID, bool encrypted) {
            var result = new MessageHelperModel {
                MatrixChannel = matrixRoomID,
                IrcChannel = ircChannel
            };
            if (!string.IsNullOrWhiteSpace(message) && !encrypted) {
                if (message.Contains("[l]") && message.Contains(" ")) {
                    var splt = message.Split(' ');
                    if (splt[0].Contains("[l]")) result.DoubleRelayDetected = true;
                }
                if (matrixRoomID == defaultMatrixRoomID && !result.DoubleRelayDetected && !string.IsNullOrWhiteSpace(senderUserID)) {
                    result.SendMessage = true;
                    result.SenderUserID = senderUserID.Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "") + "[m]";
                    var splt = message.Split(' ');
                    if (message.Contains("<") && message.Contains(">")) {
                        try {
                            if (splt[1] == ">" && splt[2].StartsWith("<") && splt[2].Contains(">")) {
                                result.IsMention = true;
                                result.MentioningTo = splt[2].Replace("<", "").Replace(">", "").Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "");
                            }
                        } catch {
                        }
                    }
                    if (result.IsMention) {
                        result.Message = result.SenderUserID + " to " + result.MentioningTo + ": " + message;
                        result.RawMessage = message;
                    } else {
                        if (message.Substring(0, 3) == "> <") {
                            var msg2 = message.Substring(2, message.Length - 2);
                            var splt2 = msg2.Split(' ');
                            result.ReplyToNickname = splt2[0].Replace("<", "").Replace(">", "").Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "") + "[m]";
                            var splt3 = msg2.Split("\n\n");
                            result.Message = result.SenderUserID + ": " + result.ReplyToNickname + ": " + splt3[1];
                            result.RawMessage = splt3[1];
                        } else {
                            result.Message = result.SenderUserID + ": " + message;
                            result.RawMessage = message;
                        }
                    }
                }
                if (string.IsNullOrEmpty(result.Message)) result.SendMessage = false;
            }
            return result;
        }
    }
}