using System.Collections.ObjectModel;
namespace nexIRC.Model.IrcProtocol {
    /// <summary>
    /// Channel Model
    /// </summary>
    public class ChannelModel {
        /// <summary>
        /// Users
        /// </summary>
        public ObservableCollection<ChannelUserModel> Users { get; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Topic
        /// </summary>
        public string? Topic { get; set; }
        /// <summary>
        /// Channel Model
        /// </summary>
        /// <param name="name"></param>
        /// <param name="topic"></param>
        public ChannelModel(string name) {
            Name = name;
            Users = new ObservableCollection<ChannelUserModel>();
        }
    }
}