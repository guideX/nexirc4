using System;
using System.Threading.Tasks;
using System.Windows;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Connection;
using nexIRC.Messages;
using nexIRC.Model;
using nexIRC.Properties;
namespace nexIRC {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        /// <summary>
        /// Client
        /// </summary>
        public Client Client { get; private set; }
        /// <summary>
        /// Is Connected
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// Event Aggregator
        /// </summary>
        public static IEventAggregator EventAggregator { get; } = new EventAggregator();
        /// <summary>
        /// Create Client
        /// </summary>
        /// <returns></returns>
        public Client CreateClient() {
            if (IsConnected) 
                return null;
            var user = new UserModel(Settings.Default.Nick, Settings.Default.RealName);
            var connection = new TcpClientConnection(Settings.Default.ServerAddress, Convert.ToInt32(Settings.Default.ServerPort));
            connection.Connected += (s, e) => IsConnected = true;
            connection.Disconnected += async (s, e) => await Disconnected();
            Client = new Client(user, connection);
            Client.SetDispatcherInvoker(Dispatcher.Invoke);
            return Client;
        }
        /// <summary>
        /// Disconnected
        /// </summary>
        /// <returns></returns>
        private Task Disconnected() {
            IsConnected = false;
            return EventAggregator.PublishOnUIThreadAsync(new ClientDisconnectedMessage());
        }
    }
}