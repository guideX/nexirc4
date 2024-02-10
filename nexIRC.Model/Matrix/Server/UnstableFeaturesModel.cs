using Newtonsoft.Json;
namespace nexIRC.Model.Matrix.Server {
    /// <summary>
    /// Unstable Features
    /// </summary>
    public class UnstableFeaturesModel {
        /// <summary>
        /// Org Matrix Label Based Filtering
        /// </summary>
        [JsonProperty("org.matrix.label_based_filtering")]
        public bool OrgMatrixLabelBasedFiltering { get; set; }
        /// <summary>
        /// Org matrix E2e Cross Signing
        /// </summary>
        [JsonProperty("org.matrix.e2e_cross_signing")]
        public bool OrgMatrixE2eCrossSigning { get; set; }
        /// <summary>
        /// /Org Matrix Msc 2432
        /// </summary>
        [JsonProperty("org.matrix.msc2432")] public bool OrgMatrixMsc2432 { get; set; }
        /// <summary>
        /// Uk half shot msc 2666
        /// </summary>
        [JsonProperty("uk.half-shot.msc2666")] public bool UkHalfShotMsc2666 { get; set; }
        /// <summary>
        /// IOElement e2e forced public
        /// </summary>
        [JsonProperty("io.element.e2ee_forced.public")]
        public bool IoElementE2eeForcedPublic { get; set; }
        /// <summary>
        /// Forced Private
        /// </summary>
        [JsonProperty("io.element.e2ee_forced.private")]
        public bool IoElementE2eeForcedPrivate { get; set; }
        /// <summary>
        /// Forced Trusted Private
        /// </summary>
        [JsonProperty("io.element.e2ee_forced.trusted_private")]
        public bool IoElementE2eeForcedTrustedPrivate { get; set; }
        /// <summary>
        /// Busy Presence
        /// </summary>
        [JsonProperty("org.matrix.msc3026.busy_presence")]
        public bool OrgMatrixMsc3026BusyPresence { get; set; }
    }
}