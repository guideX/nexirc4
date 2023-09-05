namespace nexIRC.Model {
    /// <summary>
    /// Priv Msg Param Model
    /// </summary>
    public class PrivMsgParamModel : AjaxResult {
        /// <summary>
        /// Nickname
        /// </summary>
        public string? Nickname { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string? MessageToSend { get; set; }
    }
}