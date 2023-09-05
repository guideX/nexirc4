using MvvmHelpers.Commands;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Messages;
using System.Collections.Specialized;
using System.Threading.Tasks;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Query View Model
    /// </summary>
    public class QueryViewModel : TabItemViewModel {
        /// <summary>
        /// Query
        /// </summary>
        public Query Query { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public QueryViewModel(Query query) {
            Query = query;
            query.Messages.CollectionChanged += Messages_CollectionChanged;
            SendMessageCommand = new AsyncCommand(SendQueryMessage);
        }
        /// <summary>
        /// Send Query Message
        /// </summary>
        /// <returns></returns>
        private async Task SendQueryMessage() {
            if (string.IsNullOrWhiteSpace(Message)) {
                return;
            }
            Messages.Add(Models.Message.Sent(new QueryMessage(App.Client.User, Message)));
            await App.Client.SendAsync(new PrivMsgMessage(Query.Nick, Message));
            Message = string.Empty;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            foreach (QueryMessage message in e.NewItems) {
                App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
            }
        }

        public override string ToString() => Query.Nick;
    }
}