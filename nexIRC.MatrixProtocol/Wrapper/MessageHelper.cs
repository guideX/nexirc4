using nexIRC.Business.Helper;
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
        public static MessageHelperModel GetMessageDetails(string matrixRoomID, string defaultMatrixRoomID, string ircChannel, string message, string senderUserID, bool encrypted, string settings_MatrixNick, string settings_IrcNick) {
            var result = new MessageHelperModel {
                MatrixChannel = matrixRoomID,
                IrcChannel = ircChannel
            };
            try {
                if (!string.IsNullOrWhiteSpace(message) && !encrypted && !string.IsNullOrWhiteSpace(senderUserID)) {
                    result.SenderUserID = senderUserID.Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "");
                    if (result.SenderUserID.ToLower().Trim() == settings_MatrixNick.ToLower().Trim())
                        result.DoubleRelayDetected = true;
                    if (matrixRoomID == defaultMatrixRoomID && !result.DoubleRelayDetected) {
                        result.SendMessage = true;
                        result.SenderUserID = senderUserID.Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "");
                        var splt = message.Split(' ');
                        if (message.Contains("<") && message.Contains(">")) {
                            try {
                                if (splt[1] == ">" && splt[2].StartsWith("<") && splt[2].Contains(">")) {
                                    result.IsMention = true;
                                    result.MentioningTo = splt[2].Replace("<", "").Replace(">", "").Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "");
                                }
                            } catch (Exception ex) {
                                ExceptionHelper.HandleException(ex, "GetMessageDetail");
                            }
                        }
                        if (result.IsMention) {
                            result.Message = result.SenderUserID + " to " + result.MentioningTo + ": " + message;
                            result.RawMessage = message;
                        } else {
                            if (message.Length > 3 && message.Substring(0, 3) == "> <") {
                                var msg2 = message.Substring(2, message.Length - 2);
                                var splt2 = msg2.Split(' ');
                                result.ReplyToNickname = splt2[0].Replace("<", "").Replace(">", "").Replace(":matrix.org", "").Replace(":myportal.social", "").Replace("@", "");
                                var splt3 = msg2.Split("\n\n");
                                if (result.ReplyToNickname != "runningliberachat") { // Dirty hack for "runningliberachat"
                                    result.Message = result.SenderUserID + ": " + result.ReplyToNickname + ": " + splt3[1];
                                } else {
                                    result.Message = result.SenderUserID + ": " + splt3[1];
                                }
                                result.RawMessage = splt3[1];
                            } else {
                                result.Message = result.SenderUserID + ": " + message;
                                result.RawMessage = message;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(result.Message)) result.SendMessage = false;
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.MatrixProtocol.Wrapper.GetMessageDetails");
            }
            return result;
        }
    }
}