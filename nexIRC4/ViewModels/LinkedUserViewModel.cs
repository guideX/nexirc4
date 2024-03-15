using nexIRC.IrcProtocol;
using nexIRC.MatrixProtocol.Wrapper;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Linked User View Model
    /// </summary>
    public class LinkedUserViewModel : TabItemViewModel {
        /// <summary>
        /// Client
        /// </summary>
        private Client _ircClient;
        /// <summary>
        /// Matrix Client
        /// </summary>
        private MatrixWrapper _matrixClient;
        /// <summary>
        /// Main View Model
        /// </summary>
        private MainViewModel _mainViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="matrixClient"></param>
        public LinkedUserViewModel(Client client, MainViewModel mainViewModel) {
            //var chan = client.Channels.Where(c => c.Name == Properties.Settings.Default.DefaultChannel);
            _mainViewModel = mainViewModel;
            _ircClient = client;
            //_matrixClient = matrixClient;
            //_matrixClient.MatrixRoomEvent += _matrixClient_MatrixRoomEvent;
            //_matrixClient.MatrixConnected += _matrixClient_MatrixConnected;
            //_ircClient.ServerMessages.CollectionChanged += ServerMessages_CollectionChanged;
            //SendMessageCommand = new AsyncCommand(SendServerMessage);
        }
    }
}