using MvvmHelpers.Commands;
using nexIRC.Business.Helper;
using nexIRC.IrcProtocol;
using nexIRC.IrcProtocol.Messages;
using nexIRC.Messages;
using nexIRC.Model;
using nexIRC.Properties;
//using nexIRC.Strava;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
namespace nexIRC.ViewModels {
    /// <summary>
    /// Channel View Model
    /// </summary>
    public class ChannelViewModel : TabItemViewModel {
        /// <summary>
        /// Channel
        /// </summary>
        public Channel Channel { get; }
        /// <summary>
        /// Sort Users Command
        /// </summary>
        public ICommand SortUsersCommand { get; }
        /// <summary>
        /// Open Query Command
        /// </summary>
        public ICommand OpenQueryCommand { get; }
        /// <summary>
        /// Matrix Client
        /// </summary>
        private MatrixProtocol.Wrapper.MatrixWrapper _matrixClient;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="matrixClient"></param>
        public ChannelViewModel(Channel channel, MatrixProtocol.Wrapper.MatrixWrapper matrixClient) {
            try {
                _matrixClient = matrixClient;
                Channel = channel;
                channel.Messages.CollectionChanged += Messages_CollectionChanged;
                SendMessageCommand = new AsyncCommand(SendChannelMessage);
                SortUsersCommand = new Command(SortUsers);
                OpenQueryCommand = new AsyncCommand<ChannelUserModel>(OpenQuery);
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel.Constructor");
            }
        }
        /// <summary>
        /// Sort Users
        /// </summary>
        /// <param name="items"></param>
        private void SortUsers(object items) {
            try {
                var view = CollectionViewSource.GetDefaultView(items) as ListCollectionView;
                if (view != null) view.CustomSort = new ChannelUserComparer();
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel.SortUsers");
            }
        }
        /// <summary>
        /// Open Query
        /// </summary>
        /// <param name="channelUser"></param>
        /// <returns></returns>
        private async Task OpenQuery(ChannelUserModel channelUser) {
            try {
                await App.EventAggregator.PublishOnUIThreadAsync(new OpenQueryMessage(channelUser.User));
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel.OpenQuery");
            }
        }
        /// <summary>
        /// Send Channel Message
        /// </summary>
        /// <returns></returns>
        private async Task SendChannelMessage() {
            try {
                if (string.IsNullOrWhiteSpace(Message)) return;
                Messages.Add(Models.Message.Sent(new ChannelMessage(App.Client.User, Channel, Message)));
                await App.Client.SendAsync(new PrivMsgMessage(Channel.Name, Message));
                Message = string.Empty;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel.SendChannelMessage");
            }
        }
        /// <summary>
        /// Send Channel Message
        /// </summary>
        /// <returns></returns>
        private async Task SendNewChannelMessage(string message) {
            try {
                if (string.IsNullOrWhiteSpace(message)) return;
                Message = message;
                Messages.Add(Models.Message.Sent(new ChannelMessage(App.Client.User, Channel, Message)));
                await App.Client.SendAsync(new PrivMsgMessage(Channel.Name, Message));
                Message = string.Empty;
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel.SendChannelMessage");
            }
        }
        /// <summary>
        /// Messages Collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            try {
                nexIRC.Views.MainWindow main = null;
                App.Dispatcher.Invoke(() => { main = (Views.MainWindow)App.MainWindow; });
                foreach (ChannelMessage message in e.NewItems) {
                    var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    foreach (Match m in linkParser.Matches(message.Text)) {
                        var source = new WebClient().DownloadString(m.Value);
                        var title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                        var shortUrl = (m.Value.Replace("https://", "").Replace("http://", "").Replace("www.", "")).Split('/')[0];
                        var msg = new ChannelMessage(App.Client.User, Channel, title);
                        SendNewChannelMessage(title + " - " + shortUrl).Wait();
                        if (Settings.Default.UseMatrix) 
                            _matrixClient.SendMessage(_matrixClient.CurrentChannelID, title + " - " + shortUrl);
                    }
                    if (message.Channel.Name.StartsWith("##runningtesting") && message.Text.Contains("!leaderboard")) {
                        //Task.Run(async () => { await ClubleaderboardScraper.Scrape(true); }).Wait();
                        //if (!string.IsNullOrWhiteSpace(ClubleaderboardScraper.Data)) {
                            //if (Settings.Default.UseMatrix) _matrixClient.SendMessage(_matrixClient.CurrentChannelID, ClubleaderboardScraper.Data);
                            //SendNewChannelMessage(ClubleaderboardScraper.Data).Wait();
                        //}
                        // CLIENT SECRET: 145805f47e8469a8f54386419b109e8f84810da0
                        // ACCESS TOKEN: 974942c733f216a8c34cdd4422afe35d4c35f666
                        //AutoLoginLeaderboardScraper.Scrape().Wait();
                        //if (!string.IsNullOrWhiteSpace(AutoLoginLeaderboardScraper.Data.ToString())) {
                        //if (Settings.Default.UseMatrix) _matrixClient.SendMessage(_matrixClient.CurrentChannelID, AutoLoginLeaderboardScraper.Data.ToString());
                        //SendNewChannelMessage(AutoLoginLeaderboardScraper.Data.ToString()).Wait();
                        //}
                    }
                    if (message.Channel.Name.StartsWith ("##running") && message.Text.Contains("!pace")) {
                        // Parameters:
                        //   --time:<time> // <time> is in minutes/seconds format, example: 50:52
                        //   --distance:<miles or km> // <miles> example: 4.52, <km> example: 3.1k
                        //   --method:<conversion method> // either eu or us (us is presumed)
                        //var splt = e.Details.Message.Split(' ')
                        var time = "";
                        double minutes = 0;
                        var distance = "";
                        var method = "us";
                        double result = 0;
                        if (message.Text.ToLower().Contains("--time:")) {
                            var time_splt1 = message.Text.SplitStringByOtherString("--time");
                            var time_splt2 = time_splt1[1].Split(' ');
                            if (time_splt2[0].Contains(":")) {
                                var time_split3 = time_splt2[0].Split(':');
                                minutes = time_split3[1].ToIntNotNullable();
                            } else if (time_splt2[0].IsNumeric()) {
                                time = time_splt2[0];
                            }
                        }
                        if (message.Text.ToLower().Contains("--distance:")) {
                            var distance_splt1 = message.Text.SplitStringByOtherString("--distance");
                            var distance_splt2 = distance_splt1[1].Split(' ');
                            distance = distance_splt2[0].Replace(":", "");
                        }
                        if (minutes != 0 && !string.IsNullOrWhiteSpace(distance)) {
                            switch (method) {
                                case "us":
                                    result = (minutes / Convert.ToDouble(distance));
                                    SendNewChannelMessage(String.Format("{0:0.00}", result) + " minutes per mile").Wait();
                                    break;
                                case "eu":
                                    result = (minutes / Convert.ToDouble(distance));
                                    SendNewChannelMessage(String.Format("{0:0.00}", result) + " minutes per km").Wait();
                                    break;
                            }
                        }
                    }
                    if (Settings.Default.UseMultipleNicknames && Settings.Default.UseMatrix) {
                        if (!main.IsUserInClientCollection(message.User.Nick, message.Channel.Name)) {
                            App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
                            _matrixClient.SendMessage(_matrixClient.CurrentChannelID, message.User.Nick + ": " + message.Text);
                        }
                    } else {
                        App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
                        if (Settings.Default.UseMatrix) _matrixClient.SendMessage(_matrixClient.CurrentChannelID, message.User.Nick + ": " + message.Text);
                    }
                }
            } catch (Exception ex) {
                ExceptionHelper.HandleException(ex, "nexIRC.ViewModels.ChannelViewModel");
            }
        }
        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Channel.Name;
    }
}