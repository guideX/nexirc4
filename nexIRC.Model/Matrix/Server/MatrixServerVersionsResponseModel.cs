namespace nexIRC.Model.Matrix.Server {
    /// <summary>
    /// Matrix Server Versions Response Model
    /// </summary>
    public class MatrixServerVersionsResponseModel {
        /// <summary>
        /// Versions
        /// </summary>
        public List<string> versions { get; set; }
        /// <summary>
        /// Unstable Features
        /// </summary>
        public UnstableFeaturesModel unstable_features { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MatrixServerVersionsResponseModel() {
            versions = new List<string>();
            unstable_features = new UnstableFeaturesModel();
        }
    }
}