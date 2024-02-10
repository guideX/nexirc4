using MvvmHelpers.Commands;
using nexIRC.Business.Helper;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Model;
using System;
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
        public QueryModel Query { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public QueryViewModel(QueryModel query) {
            try {
                Query = query;
                query.Messages.CollectionChanged += Messages_CollectionChanged;
                SendMessageCommand = new AsyncCommand(SendQueryMessage);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.QueryViewModel.QueryViewModel");
            }
        }
        /// <summary>
        /// Send Query Message
        /// </summary>
        /// <returns></returns>
        private async Task SendQueryMessage() {
            try {
                if (string.IsNullOrWhiteSpace(Message)) return;
                Messages.Add(Models.Message.Sent(new QueryMessageModel(App.Client.User, Message)));
                await App.Client.SendAsync(new PrivMsgMessage(Query.Nick, Message));
                Message = string.Empty;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.QueryViewModel.SendQueryMessage");
            }
        }
        /// <summary>
        /// Messages Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            try {
                foreach (QueryMessageModel message in e.NewItems) App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.QueryViewModel.Messages_CollectionChanged");
            }
        }
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Query.Nick;
    }
}